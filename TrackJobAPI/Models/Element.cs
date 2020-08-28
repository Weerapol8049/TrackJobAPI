using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Models
{
    public class Element
    {
        [JsonProperty(PropertyName = "DataSourceName")]
        public string DataSourceName { get; set; }

        [JsonProperty(PropertyName = "FieldName")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "Operator")]
        public int Operator { get; set; }

        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "RecId")]
        public string RecId { get; set; }

    }
}