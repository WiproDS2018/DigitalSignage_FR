using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public sealed class SignageConstants
    {
        private SignageConstants() { }

        public const string SCENESUBMIT = "Submitted";
        public const string SCENESAVE = "Saved";
        public const string SCENEPUBLISH = "Published";
        public const string SCENEAPPROVE = "Approved";
        public const string SCENEREJECT = "Rejected";
        public const string SCENESCHEDULE = "Scheduled";

        public const string CAMPAIGNSCHEDULE = "Scheduled";
        public const string CAMPAIGNPUBLISH = "Published";
        public const string CAMPAIGNCANCEL = "Canceled";
        public const string CAMPAIGNACTIVATE = "Activated";

        public const string ROLEUSER = "User";
        public const string TEMPFOLDER = "Content\\FilesUpload";
        public const string DELETEERROR = "Error unable to delete...!";

        //Messages
        public const string SUCCESS = "Success";
        public const string ERROR = "Error.....!";
        public const string SAVEERROR = "Error unable to save...!";

        public const string PLAYEREXISTS = "Device serial no exists..!";
        public const string PLAYERGROUPEXIST = "Device Group already exists..!";
        public const string STATIONEXIST = " Device Group already exists..!";

        public const string SCENEEXISTS = "Content name already exists..!";
        public const string CAMPAIGNEXIST = "PlayList Already Exist..!";

        public const string INVALIDLOGIN = "Invalid user name or password, Try again!";
        public const string PASSWORDMISSMATCH = "Password and confirm password should be the same.";
        public const string SUCCESSUSER = "User registered successfully.";
        public const string USEREXISTS = "User name already exists..!";
        public const string IMAGEEXISTS = "Image is exists";

        public const string IMAGEUPLOAD = "IMAGE-UPLOAD";
        public const string IMAGETEMPLATE = "IMAGE-TEMPLATE";
        public const string PPT= "PPT";
        public const string VIDEO = "VIDEO";
        public const string WEBURL = "WEBURL";
        public const string VIDEOURL = "VIDEOURL";

        public const string CAMPAIGNFILTER = "PlayList";
        public const string STATIONFILTER = "Device Group";

        public const string SESSIONERROR = "Session Expired";
       

    }
}
