using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Classes
{
    public class TrackStatus
    {
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
    }
}