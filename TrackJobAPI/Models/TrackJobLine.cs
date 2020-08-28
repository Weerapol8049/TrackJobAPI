using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackJobAPI.Models
{
    public class TrackJobLine
    {
        [JsonProperty(PropertyName = "ProjId")]
        public string ProjId { get; set; }

        [JsonProperty(PropertyName = "ProjName")]
        public string ProjName { get; set; }

        [JsonProperty(PropertyName = "ActStartDate")]
        public DateTime ActStartDate { get; set; }

        [JsonProperty(PropertyName = "ActEndDate")]
        public DateTime ActEndDate { get; set; }

        [JsonProperty(PropertyName = "Delay")]
        public int Delay { get; set; }

        [JsonProperty(PropertyName = "DelayPlan")]
        public int DelayPlan { get; set; }

        [JsonProperty(PropertyName = "OperName")]
        public string OperName { get; set; }

        [JsonProperty(PropertyName = "OperNo")]
        public string OperNo { get; set; }

        [JsonProperty(PropertyName = "PlanDays")]
        public int PlanDays { get; set; }

        [JsonProperty(PropertyName = "PlanStartDate")]
        public DateTime PlanStartDate { get; set; }

        [JsonProperty(PropertyName = "PlanEndDate")]
        public DateTime PlanEndDate { get; set; }

        [JsonProperty(PropertyName = "Remark")]
        public string Remark { get; set; }

        [JsonProperty(PropertyName = "SideDesign")]
        public string SideDesign { get; set; }

        [JsonProperty(PropertyName = "StmTrackId")]
        public string StmTrackId { get; set; }

        [JsonProperty(PropertyName = "TypeDesign")]
        public string TypeDesign { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "RecId")]
        public string RecId { get; set; }
    }
}