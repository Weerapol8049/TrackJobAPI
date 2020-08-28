using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Classes
{
    public class TypeRoom
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
    }
}