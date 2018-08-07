using Microsoft.Azure.Devices.Provisioning.Security;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignageDps
{
    class TpmGenerator
    {
        private string GlobalDeviceEndpoint = Enviornment.GlobalDeviceProvisioningEndPoint;
        private string manufacturerId;
        private string manufacturerIdTxt;
        private static TpmGenerator generator = null;
        private string registrationId;
        private string endorsementKey;
        private SecurityProviderTpm securityProvider;
        public bool TpmAvailable = false;
        public static TpmGenerator GetGenerator()
        {
            if (generator == null) generator = new TpmGenerator();
            return generator;
        }
        private TpmGenerator()
        {
            registrationId = GetMachineGuid();
            Logger.LogToConnector($"System GUID : {registrationId}");
            try
            {
                securityProvider = new SecurityProviderTpmHsm(registrationId);
                endorsementKey = Convert.ToBase64String(securityProvider.GetEndorsementKey());
                Logger.LogToConnector($"TPM GENERTATED KEY : {endorsementKey}");
                TpmAvailable = true;
            }
            catch (Exception)
            {
                TpmAvailable = false;
                //SecurityProviderTpmSimulator.StartSimulatorProcess();
                //securityProvider = new SecurityProviderTpmSimulator(RegistrationId);
            }
          
        }
        public string RegistrationId
        {
            get
            {
                return registrationId;
            }
        }
        public string EndorsementKey
        {
            get
            {
                return endorsementKey;
            }
        }
        public SecurityProviderTpm SecurityProvider
        {
            get
            {
                return securityProvider;
            }
        }
        public string GetMachineGuid()
        {


            return getGuid();
        }

        private string getUniqueID(string drive)
        {
            if (drive == string.Empty)
            {
                //Find first drive
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                //C:\ -> C
                drive = drive.Substring(0, drive.Length - 2);
            }

            string volumeSerial = getVolumeSerial(drive);
            string cpuID = getCPUID();

            //Mix them up and remove some useless 0's
            return cpuID.Substring(13) + "-" + cpuID.Substring(1, 4) + "-" + volumeSerial + "-" + cpuID.Substring(4, 4);
        }

        private string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }
        private string getGuid()
        {
            try
            {
                RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey sqlsrvKey = localMachineX64View.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
                return (string)sqlsrvKey.GetValue("MachineGuid");
            }
            catch (Exception)
            {
                RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                RegistryKey sqlsrvKey = localMachineX64View.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography");
                return (string)sqlsrvKey.GetValue("MachineGuid");
            }
        }
    }
}
