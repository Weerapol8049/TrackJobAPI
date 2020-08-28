using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Classes
{
    public class BusinessUnit
    {
        [JsonProperty(PropertyName = "BU")]
        public string BU { get; set; }
    }
}