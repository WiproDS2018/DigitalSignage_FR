// Copyright (c) Microsoft. All rights reserved.

using IoTCoreDefaultApp.Config;
using IoTCoreDefaultApp.IoT;
using IoTCoreDefaultApp.Json;
using IoTCoreDefaultApp.Player;
using IoTCoreDefaultApp.Utils;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace IoTCoreDefaultApp
{
    public sealed partial class MainPage : Page
    {
        public static string WebApiEndPoint;
        public static MainPage Current;
        private CoreDispatcher MainPageDispatcher;
        private ConnectedDevicePresenter connectedDevicePresenter;
        private const string CommandLineProcesserExe = "c:\\windows\\system32\\cmd.exe";
        private const string RegKeyQueryCmdArg = "/c \"reg query HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\IoT /v IsMakerImage /z\"";
        private const string ExpectedResultPattern = @"\s*IsMakerImage\s*REG_DWORD\s*\(4\)\s*0x1";
        private const uint CmdLineBufSize = 8192;

        public CoreDispatcher UIThreadDispatcher
        {
            get
            {
                return MainPageDispatcher;
            }

            set
            {
                MainPageDispatcher = value;
            }
        }
        private bool haveBeenWarned = false;

        public MainPage()
        {
            MaximizeWindowOnLoad();
            this.InitializeComponent();
            Config.Environment.ConfigureEnvironment();
            PlaylistModifier.RemoveAllExpiredEntries();
            
            Window.Current.CoreWindow.KeyUp += Window_KeyUp;
            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;

            string currentSetting = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.SettingsFile).AsTask().Result;
            EndPointSettings settings = JsonConvert.DeserializeObject<EndPointSettings>(currentSetting);
            if (settings.EndPoint != null && settings.EndPoint != "")
            {
                WebAddress.Text = settings.EndPoint;
                WebApiEndPoint = settings.EndPoint;
            }
            MainPageDispatcher = Window.Current.Dispatcher;

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.DataContext = LanguageManager.GetInstance();

            //UpdateMakerImageSecurityNotice();

            this.Loaded += async (sender, e) =>
            {
                Log.Write("MainPage Loaded");
                SignagePlayer player = new SignagePlayer(defaultImage,imageViewer, videoPlayer, webBrowser, MainPageDispatcher);
                player.StartPlayer();
                await MainPageDispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    UpdateBoardInfo();
                    UpdateNetworkInfo();
                    UpdateConnectedDevices();
                    UpdatePackageVersion();
                });
               
                
            };
            Log.DeletePreviousLogFiles();
            DeviceMessenger.StartMessagingService();
            //Geolocator.RequestAccessAsync();
        }

        //private async void UpdateMakerImageSecurityNotice()
        //{
        //    if (await IsMakerImager())
        //    {
        //        await MainPageDispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
        //        {
        //            SecurityNoticeRow.Visibility = Visibility.Visible;
        //        });
        //    }
        //}

        //private async Task<bool> IsMakerImager()
        //{
        //    var cmdOutput = string.Empty;

        //    var standardOutput = new InMemoryRandomAccessStream();
        //    var options = new ProcessLauncherOptions
        //    {
        //        StandardOutput = standardOutput
        //    };

        //    try
        //    {
        //        var result = await ProcessLauncher.RunToCompletionAsync(CommandLineProcesserExe, RegKeyQueryCmdArg, options);

        //        if (result.ExitCode == 0)
        //        {
        //            using (var outStreamRedirect = standardOutput.GetInputStreamAt(0))
        //            {
        //                using (var dataReader = new DataReader(outStreamRedirect))
        //                {
        //                    uint bytesLoaded = 0;
        //                    while ((bytesLoaded = await dataReader.LoadAsync(CmdLineBufSize)) > 0)
        //                    {
        //                        cmdOutput += dataReader.ReadString(bytesLoaded);
        //                    }
        //                }
        //            }
        //        }

        //        Match match = Regex.Match(cmdOutput, ExpectedResultPattern, RegexOptions.IgnoreCase);
        //        if (match.Success)
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Could not read the value
        //        Log.Write("Could not read maker image value in registry");
        //        Log.Write(ex.ToString());
        //    }

        //    return false;
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(Constants.HasDoneOOBEKey))
            {
                ApplicationData.Current.LocalSettings.Values[Constants.HasDoneOOBEKey] = Constants.HasDoneOOBEValue;
            }

            base.OnNavigatedTo(e);
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await MainPageDispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                UpdateNetworkInfo();
            });
        }

        private void UpdateBoardInfo()
        {
            BoardName.Text = DeviceInfoPresenter.GetBoardName();
            BoardName.Text = "Digital Signage Player";
            BoardImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));

            ulong version = 0;
            if (!ulong.TryParse(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion, out version))
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                OSVersion.Text = loader.GetString("OSVersionNotAvailable");
            }
            else
            {
                OSVersion.Text = String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}",
                    (version & 0xFFFF000000000000) >> 48,
                    (version & 0x0000FFFF00000000) >> 32,
                    (version & 0x00000000FFFF0000) >> 16,
                    version & 0x000000000000FFFF);
            }
        }

        private void UpdatePackageVersion()
        {
            AppxVersion.Text = String.Format(CultureInfo.InvariantCulture, "v{0}.{1}.{2}.{3}",
              Package.Current.Id.Version.Major,
              Package.Current.Id.Version.Minor,
              Package.Current.Id.Version.Build,
              Package.Current.Id.Version.Revision);
        }

        private void WindowsOnDevices_Click(object sender, RoutedEventArgs e)
        {
            NavigationUtils.NavigateToScreen(typeof(WebBrowserPage), Constants.WODUrl);
        }

        private async void UpdateNetworkInfo()
        {
            var deviceInformation = new EasClientDeviceInformation();
           // var base64Guid = deviceInformation.Id.ToString();
            var base64Guid = Config.Environment.DeviceId;


            // Replace URL unfriendly characters with better ones
            //base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

            // Remove the trailing ==
            //base64Guid= base64Guid.Substring(0, base64Guid.Length - 12).ToLower();
            //base64Guid = base64Guid.Substring(0, 4) + "-" + base64Guid.Substring(4, 4) + "-" + base64Guid.Substring(8, 4);
            this.DeviceName.Text = base64Guid ;// TpmDeviceGenerator.GetGenerator().RegistrationId;
            this.IPAddress1.Text = NetworkPresenter.GetCurrentIpv4Address();
            this.NetworkName1.Text = NetworkPresenter.GetCurrentNetworkName();
            this.NetworkInfo.ItemsSource = await NetworkPresenter.GetNetworkInformation();
        }

        private void UpdateConnectedDevices()
        {
            connectedDevicePresenter = new ConnectedDevicePresenter(MainPageDispatcher);
            this.ConnectedDevices.ItemsSource = connectedDevicePresenter.GetConnectedDevices();
        }

        private void CloseNoticeButton_Click(object sender, RoutedEventArgs e)
        {
            SecurityNoticeRow.Visibility = Visibility.Collapsed;
        }

        private void SecurityNoticeLearnMoreButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationUtils.NavigateToScreen(typeof(WebBrowserPage), Constants.IoTCoreManufacturingGuideUrl);
        }
        void MaximizeWindowOnLoad()
        {
            var view = DisplayInformation.GetForCurrentView();

            // Get the screen resolution (APIs available from 14393 onward).
            var resolution = new Size(view.ScreenWidthInRawPixels, view.ScreenHeightInRawPixels);

            // Calculate the screen size in effective pixels. 
            // Note the height of the Windows Taskbar is ignored here since the app will only be given the maxium available size.
            var scale = view.ResolutionScale == ResolutionScale.Invalid ? 1 : view.RawPixelsPerViewPixel;
            var bounds = new Size(resolution.Width / scale, resolution.Height / scale);

            ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
       
            if (e.VirtualKey.Equals(VirtualKey.F1)|| e.VirtualKey == VirtualKey.F8)
            {
                Log.Write("F1 or F8 key pressed");

                if (!haveBeenWarned && player.Visibility==Visibility.Visible)
                {
                    haveBeenWarned = true;
                    MessageBox.Text = "If You Go To App Settings you must restart the Device\n If you are sure Press F1 Again";

                    MessageGrid.Visibility = Visibility.Visible;
                    new Task(() => {
                        Task.Delay(5000).Wait();
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                            MessageGrid.Visibility = Visibility.Collapsed;
                            haveBeenWarned = false;
                        }).AsTask().Wait();
                    }).Start();
                }
                else
                {
                    MessageGrid.Visibility = Visibility.Collapsed;   
                    if (player.Visibility == Visibility.Visible)
                    {
                        player.Visibility = Visibility.Collapsed;
                        HeaderRow.Visibility = Visibility.Visible;
                        ContentRow.Visibility = Visibility.Visible;
                        // weather.Visibility = Visibility.Collapsed;

                    }
                }
            }
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string prevAddress = "";
            if (WebAddressButton.Content.ToString() == "Edit")
            {
                WebAddress.Focus(FocusState.Programmatic);
                WebAddressButton.Content = "Save";
                WebAddress.IsReadOnly = false;
                prevAddress = WebAddress.Text.Replace("https:","http");
            }
            else
            {
                WebAddressButton.Content = "Edit";
                WebAddress.IsReadOnly = true;
                EndPointSettings settings = new EndPointSettings();
                string address = WebAddress.Text;
                settings.DeviceId = Config.Environment.DeviceId;
                if (address.StartsWith("https://") || address.StartsWith("http:") || address.StartsWith("HTTPS:") || address.StartsWith("HTTP:"))
                {
                    settings.EndPoint = address.Replace("https", "http");
                    WebApiEndPoint = settings.EndPoint;
                }
                else
                {
                    settings.EndPoint = "http://" + address;
                    WebApiEndPoint = settings.EndPoint;
                }
                if (prevAddress == settings.EndPoint) return;
                string json = JsonConvert.SerializeObject(settings);

                // write string to a file
                var file = ApplicationData.Current.LocalFolder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting).AsTask().Result;
                FileIO.WriteTextAsync(file, json).AsTask().Wait();
                var detailsFile= ApplicationData.Current.LocalFolder.CreateFileAsync("DeviceDetails.json", CreationCollisionOption.ReplaceExisting).AsTask().Result;
                if (File.Exists(detailsFile.Path)) File.Delete(detailsFile.Path);
                PlaylistModifier.RemoveAllFromPlayList();
                try
                {
                    MessageBox.Text = "Please Wait While The Application Restarts";
                    MessageGrid.Visibility = Visibility.Visible;
                    ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(4));
                    
                }
                catch (Exception ex)
                {
                    // Catching the exception ensures this doesn't crash the app on non-IOT devices
                    Debug.WriteLine("Couldn't begin shutdown: " + ex.Message);
                }
            }
        }

        private void DeviceName_FocusEngaged(Control sender, FocusEngagedEventArgs args)
        {
            var packet = new DataPackage();
            packet.SetText (DeviceName.Text);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(packet);
        }
    }
}
