using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Models
{
    public class Login
    {
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}