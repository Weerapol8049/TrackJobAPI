using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Models
{
    public class TrackJob
    {
        [JsonProperty(PropertyName = "RecId")]
        public string RecId { get; set; }

        [JsonProperty(PropertyName = "TrackId")]
        public string TrackId { get; set; }

        [JsonProperty(PropertyName = "BU")]
        public string BU { get; set; }

        [JsonProperty(PropertyName = "ProjId")]
        public string ProjId { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Stage")]
        public string Stage { get; set; }

        [JsonProperty(PropertyName = "TrackStatus")]
        public string TrackStatus { get; set; }

        [JsonProperty(PropertyName = "TypeRoom")]
        public string TypeRoom { get; set; }

        [JsonProperty(PropertyName = "Unit")]
        public double Unit { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "EndDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "FinishDate")]
        public DateTime FinishDate { get; set; }

        [JsonProperty(PropertyName = "CreateDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "RefTrackId")]
        public string RefTrackId { get; set; }

        [JsonProperty(PropertyName = "ProjAmount")]
        public double ProjAmount { get; set; }
    }
}