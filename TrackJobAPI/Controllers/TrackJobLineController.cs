using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrackJobAPI.Classes;
using TrackJobAPI.Models;
using TrackJobAPI.TrackJobLineService;

namespace TrackJobAPI.Controllers
{
    //[RoutePrefix("TrackJob")]
   
    public class TrackJobLineController : ApiController
    {
        [HttpGet]
        [Route("api/trackjobline/data")]
        public IHttpActionResult GetDataLine()
        {
            List<TrackJobLine> trackJobList = new List<TrackJobLine>();
            CultureInfo us = new CultureInfo("en-US");
            try
            {
                DataSet dsTable = ExecuteStaticQuery.Get("STMTrackJob");
                var data = (from a in dsTable.Tables[0].AsEnumerable()
                            join b in dsTable.Tables[1].AsEnumerable()
                                on a.Field<string>("StmTrackId") equals b.Field<string>("StmTrackId")
                            where b.Field<DateTime>("ActEndDate").ToString("dd/MM/yyyy", us) == "01/01/1900"
                            orderby b.Field<DateTime>("PlanStartDate"), a.Field<string>("ProjId"), b.Field<string>("UserId")
                            select new TrackJobLine()
                            {
                                ProjId = a.Field<string>("ProjId").ToString(),
                                ProjName = a.Field<string>("Name").ToString(),
                                RecId = b.Field<Int64>("RecId").ToString(),
                                ActStartDate = b.Field<DateTime>("ActStartDate"),
                                ActEndDate = b.Field<DateTime>("ActEndDate"),
                                Delay = b.Field<int>("Delay"),
                                DelayPlan = b.Field<int>("DelayPlan"),
                                OperName = b.Field<string>("OperName").ToString(),
                                OperNo = b.Field<string>("OperNo").ToString(),
                                PlanDays = b.Field<int>("PlanDays"),
                                PlanEndDate = b.Field<DateTime>("PlanEndDate"),
                                PlanStartDate = b.Field<DateTime>("PlanStartDate"),
                                Remark = b.Field<string>("Remark").ToString(),
                                SideDesign = b.Field<string>("SideDesign").ToString(),
                                StmTrackId = b.Field<string>("StmTrackId").ToString(),
                                TypeDesign = b.Field<string>("TypeDesign").ToString(),
                                UserId = b.Field<string>("UserId").ToString(),
                                UserName = b.Field<string>("UserName").ToString(),
                            }).ToList();
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        
        [HttpPost]
        [Route("api/trackjobline/Edit")]
        public void Edit(TrackJobLine element)
        {
            AxdSTMTrackJobLineAPI trackJob = new AxdSTMTrackJobLineAPI();
            CultureInfo us = new CultureInfo("en-US");
            using (var client = new STMTrackJobLineAPIServiceClient())
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                trackJob = client.read(CallContextsLine.get(), ClientLine.read(element.RecId));
            }

            using (var client = new STMTrackJobLineAPIServiceClient())
            {
                AxdEntity_STMTrackJobLine entity = trackJob.STMTrackJobLine[0];
                var trackJobQry = new AxdSTMTrackJobLineAPI()
                {
                    ClearNilFieldsOnUpdate = trackJob.ClearNilFieldsOnUpdate,
                    ClearNilFieldsOnUpdateSpecified = true,
                    DocPurpose = trackJob.DocPurpose,
                    DocPurposeSpecified = true,
                    SenderId = trackJob.SenderId
                };

                string actStartDate = element.ActStartDate.ToString("dd/MM/yyyy", us);
                string actEndDate = element.ActEndDate.ToString("dd/MM/yyyy", us);
                string planStartDate = element.PlanStartDate.ToString("dd/MM/yyyy", us);
                string planEndDate = element.PlanEndDate.ToString("dd/MM/yyyy", us);

                AxdEntity_STMTrackJobLine entityUpdate = new AxdEntity_STMTrackJobLine();
                entityUpdate._DocumentHash = entity._DocumentHash;
                entityUpdate.RecId = Convert.ToInt64(element.RecId);
                entityUpdate.RecIdSpecified = true;
                
                entityUpdate.Delay = element.Delay;
                entityUpdate.DelaySpecified = true;
                entityUpdate.DelayPlan = element.DelayPlan;
                entityUpdate.DelayPlanSpecified = true;
                entityUpdate.OperName = element.OperName;
                entityUpdate.OperNo = element.OperNo;
                entityUpdate.PlanDays = element.PlanDays;
                entityUpdate.PlanDaysSpecified = true;
                entityUpdate.Remark = element.Remark;
                entityUpdate.SideDesign = element.SideDesign;
                entityUpdate.StmTrackId = element.StmTrackId;
                entityUpdate.TypeDesign = element.TypeDesign;
                entityUpdate.UserId = element.UserId;
                entityUpdate.UserName = element.UserName;
       

                if (element.PlanStartDate != default(DateTime))
                {
                    entityUpdate.PlanStartDate = DateTime.ParseExact(planStartDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                    entityUpdate.PlanStartDateSpecified = true;
                }

                if (element.PlanEndDate != default(DateTime))
                {
                    entityUpdate.PlanEndDate = DateTime.ParseExact(planEndDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                    entityUpdate.PlanEndDateSpecified = true;
                }

                if (element.ActStartDate != default(DateTime))
                {
                    entityUpdate.ActStartDate = DateTime.ParseExact(actStartDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                    entityUpdate.ActStartDateSpecified = true;
                }

                if (element.ActEndDate != default(DateTime))
                {
                    entityUpdate.ActEndDate = DateTime.ParseExact(actEndDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                    entityUpdate.ActEndDateSpecified = true;
                }

                trackJobQry.STMTrackJobLine = new AxdEntity_STMTrackJobLine[1] { entityUpdate };

                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                client.update(CallContextsLine.get(), ClientLine.read(element.RecId), trackJobQry);
            }
            
        }

        [Route("api/trackjobline/find")]
        [HttpPost]
        public IHttpActionResult Find(Element element)
        {

            List<TrackJobLine> trackJobList = new List<TrackJobLine>();
            AxdSTMTrackJobLineAPI trackJob = new AxdSTMTrackJobLineAPI();
            CriteriaElement[] criteriaElement = new CriteriaElement[6];

            criteriaElement[0].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[0].FieldName = "UserId";// "StmTrackId";
            criteriaElement[0].Operator = Operator.Equal;
            criteriaElement[0].Value1 = element.Value;// "TK000001";

            criteriaElement[1].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[1].FieldName = "UserName";// "StmTrackId";
            criteriaElement[1].Operator = Operator.Equal;
            criteriaElement[1].Value1 = element.Value;// "TK000001";

            criteriaElement[2].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[2].FieldName = "Remark";// "StmTrackId";
            criteriaElement[2].Operator = Operator.Equal;
            criteriaElement[2].Value1 = element.Value;// "TK000001";

            criteriaElement[3].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[3].FieldName = "SideDesign";// "StmTrackId";
            criteriaElement[3].Operator = Operator.Equal;
            criteriaElement[3].Value1 = element.Value;// "TK000001";

            criteriaElement[4].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[4].FieldName = "TypeDesign";// "StmTrackId";
            criteriaElement[4].Operator = Operator.Equal;
            criteriaElement[4].Value1 = element.Value;// "TK000001";

            criteriaElement[5].DataSourceName = "STMTrackJobLine";// "STMTrackJobTable";
            criteriaElement[5].FieldName = "OperName";// "StmTrackId";
            criteriaElement[5].Operator = Operator.Equal;
            criteriaElement[5].Value1 = element.Value;// "TK000001";

            QueryCriteria criteria = new QueryCriteria();
            criteria.CriteriaElement = criteriaElement;

            using (var client = new STMTrackJobLineAPIServiceClient())
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                trackJob = client.find(CallContextsLine.get(), criteria);
            }

            for (int i = 0; i < trackJob.STMTrackJobLine.Length; i++)
            {

                trackJobList.Add(new TrackJobLine
                {
                    RecId = trackJob.STMTrackJobLine[i].RecId.ToString(),
                    ActStartDate = Convert.ToDateTime(trackJob.STMTrackJobLine[i].ActStartDate),
                    ActEndDate = Convert.ToDateTime(trackJob.STMTrackJobLine[i].ActEndDate),
                    Delay = Convert.ToInt32(trackJob.STMTrackJobLine[i].Delay),
                    DelayPlan = Convert.ToInt32(trackJob.STMTrackJobLine[i].DelayPlan),
                    OperName = trackJob.STMTrackJobLine[i].OperName == null ? "" : trackJob.STMTrackJobLine[i].OperName.ToString(),
                    OperNo = trackJob.STMTrackJobLine[i].OperNo == null ? "" : trackJob.STMTrackJobLine[i].OperNo.ToString(),
                    PlanDays = Convert.ToInt32(trackJob.STMTrackJobLine[i].PlanDays),
                    PlanEndDate = Convert.ToDateTime(trackJob.STMTrackJobLine[i].PlanEndDate),
                    PlanStartDate = Convert.ToDateTime(trackJob.STMTrackJobLine[i].PlanStartDate),
                    Remark = trackJob.STMTrackJobLine[i].Remark == null ? "" : trackJob.STMTrackJobLine[i].Remark.ToString(),
                    SideDesign = trackJob.STMTrackJobLine[i].SideDesign == null ? "" : trackJob.STMTrackJobLine[i].SideDesign.ToString(),
                    StmTrackId = trackJob.STMTrackJobLine[i].StmTrackId == null ? "" : trackJob.STMTrackJobLine[i].StmTrackId.ToString(),
                    TypeDesign = trackJob.STMTrackJobLine[i].TypeDesign == null ? "" : trackJob.STMTrackJobLine[i].TypeDesign.ToString(),
                    UserId = trackJob.STMTrackJobLine[i].UserId == null ? "" : trackJob.STMTrackJobLine[i].UserId.ToString(),
                    UserName = trackJob.STMTrackJobLine[i].UserName == null ? "" : trackJob.STMTrackJobLine[i].UserName.ToString()
                });
            }

            return Json(trackJobList);
        }
    }
}
