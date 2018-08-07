using Microsoft.ProjectOxford.Common.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition.Face
{
    class SignageRequest
    {
        private string age;
        private string gender;
        private List<string> urls = new List<string>();
        public SignageRequest(string age, string gender)
        {
            this.age = age;
            this.gender = gender;
        }
        public List<string> ProcessRequest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (age != "undefined" && gender != "undefined")
                this.urls = DatabaseUtil.GetSignageUrls(age, gender);
            stopwatch.Stop();
            Logger.LogToFaceRecog("Time For Database : " + stopwatch.ElapsedMilliseconds);
            return urls;
        }
        public List<string> GetUrls()
        {
            return urls;
        }
        public string GetAge()
        {
            return age;
        }
        public string GetGender()
        {
            return gender;
        }
        public override bool Equals(object o)
        {
            if (o == null && this == null) return true;
            if (o == null && this != null) return false;
            if (o != null && this == null) return false;
            SignageRequest other = (SignageRequest)o;
            if (gender != other.gender) return false;
            if (age == "undefined" && other.age == "undefined") return true;
            if (age != "undefined" && other.age == "undefined") return false;
            if (age == "undefined" && other.age != "undefined") return false;
            float floatThis = float.Parse(age);
            float floatOther = float.Parse(other.age);
            int intThis = ((int)floatThis) / 10;
            int intOther = ((int)floatOther) / 10;
            if (intThis == intOther) return true;
            return false;
        }
        public override int GetHashCode()
        {
            return (int)float.Parse(age);
        }
    }

}
