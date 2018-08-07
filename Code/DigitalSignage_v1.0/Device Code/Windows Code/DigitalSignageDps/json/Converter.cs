using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignageDps
{
    public partial class Settings
    {
        [JsonProperty("iotDpsIdScope")]
        public IotDpsIdScope IotDpsIdScope { get; set; }

        [JsonProperty("nodeServerUrl")]
        public IotDpsIdScope NodeServerUrl { get; set; }

        public static Settings FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json, Converter.Settings);

    }

    public partial class IotDpsIdScope
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

 

    public static class Serialize
    {
        public static string ToJson(this Settings self) => JsonConvert.SerializeObject(self,Converter.Settings);
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
