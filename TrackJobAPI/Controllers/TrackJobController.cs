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
using TrackJobAPI.TrackJobServices;

namespace TrackJobAPI.Controllers
{
    //[RoutePrefix("TrackJob")]
    public class TrackJobController : ApiController
    {

        [Route("api/trackjob/data")]
        [HttpGet]
        public IHttpActionResult GetData()
        {
            List<TrackJob> trackJobList = new List<TrackJob>();

            DataSet dataSet = ExecuteStaticQuery.Get("STMTrackJobQry");

            foreach (DataTable table in dataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    string bu = Enum.GetName(typeof(AxdEnum_STMMBU), row["STMMBU"]);
                    string typeroom = Enum.GetName(typeof(AxdEnum_STMTypeRoomJIS), row["STMTypeRoom"]);
                    string status = Enum.GetName(typeof(AxdEnum_STMTrackStatus), row["STMTrackStatus"]);

                    trackJobList.Add(new TrackJob
                    {
                        RecId = row["RecId"].ToString(),
                        TrackId = row["StmTrackId"].ToString(),
                        BU = bu,
                        CreateDate = Convert.ToDateTime(row["CreatedDate"]),
                        FinishDate = Convert.ToDateTime(row["FinishDate"]),
                        Name = row["Name"].ToString(),
                        ProjId = row["ProjId"].ToString(),
                        RefTrackId = row["RefTrackId"].ToString(),
                        Stage = row["ProjStage"].ToString(),
                        StartDate = Convert.ToDateTime(row["StartDate"]),
                        TypeRoom = typeroom,
                        Unit = Convert.ToInt32(row["Unit"]),
                        ProjAmount = Convert.ToDouble(row["ProjAmount"]),
                        TrackStatus = status
                    });
                }
            }
            return Json(trackJobList);
        }

        // GET api/values
        [Route("api/trackjob/find")]
        [HttpPost]
        public IHttpActionResult Find(Element element)
        {

            List<TrackJob> trackList = new List<TrackJob>();
            AxdSTMTrackJob trackJob = new AxdSTMTrackJob();
            CriteriaElement criteriaElement = new CriteriaElement();

            criteriaElement.DataSourceName = element.DataSourceName;// "STMTrackJobTable";
            criteriaElement.FieldName = element.FieldName;// "StmTrackId";
            criteriaElement.Operator = Operator.Equal;
            criteriaElement.Value1 = element.Value;// "TK000001";

            QueryCriteria criteria = new QueryCriteria();
            criteria.CriteriaElement = new CriteriaElement[] { criteriaElement };

            using (var client = new STMTrackJobServiceClient())
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                trackJob = client.find(CallContexts.get(), criteria);
            }

            for (int i = 0; i < trackJob.STMTrackJobTable.Length; i++)
            {
                string bu = Enum.GetName(typeof(AxdEnum_STMMBU), trackJob.STMTrackJobTable[i].STMMBU);
                string typeroom = Enum.GetName(typeof(AxdEnum_STMTypeRoomJIS), trackJob.STMTrackJobTable[i].STMTypeRoom);
                string status = Enum.GetName(typeof(AxdEnum_STMTrackStatus), trackJob.STMTrackJobTable[i].STMTrackStatus);

                trackList.Add(new TrackJob
                {
                    RecId = trackJob.STMTrackJobTable[i].RecId.ToString(),
                    TrackId = trackJob.STMTrackJobTable[i].StmTrackId == null ? "" : trackJob.STMTrackJobTable[i].StmTrackId.ToString(),
                    BU = bu,
                    CreateDate = Convert.ToDateTime(trackJob.STMTrackJobTable[i].CreatedDate),
                    FinishDate = Convert.ToDateTime(trackJob.STMTrackJobTable[i].FinishDate),
                    Name = trackJob.STMTrackJobTable[i].Name == null ? "" : trackJob.STMTrackJobTable[i].Name.ToString(),
                    ProjId = trackJob.STMTrackJobTable[i].ProjId == null ? "" : trackJob.STMTrackJobTable[i].ProjId.ToString(),
                    RefTrackId = trackJob.STMTrackJobTable[i].RefTrackId == null ? "" : trackJob.STMTrackJobTable[i].RefTrackId.ToString(),
                    Stage = trackJob.STMTrackJobTable[i].ProjStage == null ? "" : trackJob.STMTrackJobTable[i].ProjStage.ToString(),
                    StartDate = Convert.ToDateTime(trackJob.STMTrackJobTable[i].StartDate),
                    TypeRoom = typeroom,
                    Unit = Convert.ToInt32(trackJob.STMTrackJobTable[i].Unit),
                    ProjAmount = Convert.ToDouble(trackJob.STMTrackJobTable[i].ProjAmount),
                    TrackStatus = status
                });
            }

            return Json(trackList);
        }

        [Route("api/trackjob/Create")]
        [HttpPost]
        public void Create(TrackJob element)
        {

            using (var client = new STMTrackJobServiceClient())
            {
                string startDate = element.StartDate.ToShortDateString();
                string finishDate = element.FinishDate.ToShortDateString();
                string createDate = element.CreateDate.ToShortDateString();

                AxdEntity_STMTrackJobTable entity = new AxdEntity_STMTrackJobTable();
                entity._DocumentHash = entity._DocumentHash;
                entity.RecId = Convert.ToInt64(element.RecId);
                entity.RecIdSpecified = true;
                entity.RefTrackId = element.RefTrackId;
                entity.Name = element.Name;
                entity.ProjId = element.ProjId;
                entity.ProjStage = element.Stage;
                entity.STMMBU = (AxdEnum_STMMBU)Enum.Parse(typeof(AxdEnum_STMMBU), element.BU);
                entity.StmTrackId = element.TrackId;
                entity.STMTypeRoom = (AxdEnum_STMTypeRoomJIS)Enum.Parse(typeof(AxdEnum_STMTypeRoomJIS), element.TypeRoom);
                entity.ProjAmount = Convert.ToDecimal(element.ProjAmount);
                entity.STMTrackStatus = (AxdEnum_STMTrackStatus)Enum.Parse(typeof(AxdEnum_STMTrackStatus), element.TrackStatus);
                if (startDate != "01/01/1900" && startDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entity.StartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (finishDate != "01/01/1900" && finishDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entity.FinishDate = DateTime.ParseExact(finishDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (createDate != "01/01/1900" && createDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entity.CreatedDate = DateTime.ParseExact(createDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                var trackJob = new AxdSTMTrackJob()
                {
                    STMTrackJobTable = new AxdEntity_STMTrackJobTable[1] { entity }
                };

                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                client.create(CallContexts.get(), trackJob);


            }
        }

        [Route("api/trackjob/Delete")]
        [HttpDelete]
        public void Delete(Element element)
        {
            using (var client = new STMTrackJobServiceClient())
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                client.delete(CallContexts.get(), Client.read(element.RecId));
            }
        }

        [Route("api/trackjob/Edit")]
        [HttpPost]
        public void Edit(TrackJob element)
        {
            AxdSTMTrackJob trackJob = new AxdSTMTrackJob();

            using (var client = new STMTrackJobServiceClient())
            {
                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                trackJob = client.read(CallContexts.get(), Client.read(element.RecId));
            }

            using (var client = new STMTrackJobServiceClient())
            {
                AxdEntity_STMTrackJobTable entity = trackJob.STMTrackJobTable[0];
                var trackJobQry = new AxdSTMTrackJob()
                {
                    ClearNilFieldsOnUpdate = trackJob.ClearNilFieldsOnUpdate,
                    ClearNilFieldsOnUpdateSpecified = true,
                    DocPurpose = trackJob.DocPurpose,
                    DocPurposeSpecified = true,
                    SenderId = trackJob.SenderId
                };

                string startDate = element.StartDate.ToShortDateString();
                string finishDate = element.FinishDate.ToShortDateString();
                string createDate = element.CreateDate.ToShortDateString();

                AxdEntity_STMTrackJobTable entityUpdate = new AxdEntity_STMTrackJobTable();
                entityUpdate._DocumentHash = entity._DocumentHash;
                entityUpdate.RecId = Convert.ToInt64(element.RecId);
                entityUpdate.RecIdSpecified = true;
                entityUpdate.RefTrackId = "";
                entityUpdate.Name = element.Name;
                entityUpdate.ProjId = element.ProjId;
                entityUpdate.ProjStage = element.Stage;
                entityUpdate.STMMBU = (AxdEnum_STMMBU)Enum.Parse(typeof(AxdEnum_STMMBU), element.BU);
                entityUpdate.StmTrackId = element.TrackId;
                entityUpdate.STMTypeRoom = (AxdEnum_STMTypeRoomJIS)Enum.Parse(typeof(AxdEnum_STMTypeRoomJIS), element.TypeRoom);
                entityUpdate.ProjAmount = Convert.ToDecimal(element.ProjAmount);
                entityUpdate.STMTrackStatus = (AxdEnum_STMTrackStatus)Enum.Parse(typeof(AxdEnum_STMTrackStatus), element.TrackStatus);
                if (startDate != "01/01/1900" && startDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entityUpdate.StartDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (finishDate != "01/01/1900" && finishDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entityUpdate.FinishDate = DateTime.ParseExact(finishDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (createDate != "01/01/1900" && createDate != "01/01/0544")//(soDaily.DueDate != DateTime.MinValue)
                {
                    entityUpdate.CreatedDate = DateTime.ParseExact(createDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                trackJobQry.STMTrackJobTable = new AxdEntity_STMTrackJobTable[1] { entityUpdate };

                client.ClientCredentials.Windows.ClientCredential.Domain = UserAccount.Domain;
                client.ClientCredentials.Windows.ClientCredential.UserName = UserAccount.Username;
                client.ClientCredentials.Windows.ClientCredential.Password = UserAccount.Password;
                client.update(CallContexts.get(), Client.read(element.RecId), trackJobQry);
            }
        }

        [Route("api/trackjob/bu")]
        [HttpGet]
        public IHttpActionResult BU()
        {
            List<BusinessUnit> enumType = new List<BusinessUnit>();
            foreach (AxdEnum_STMMBU val in Enum.GetValues(typeof(AxdEnum_STMMBU)))
            {
                enumType.Add(new BusinessUnit
                {
                    BU = val.ToString()
                });
            }

            return Json(enumType);
        }

        [Route("api/trackjob/typeroom")]
        [HttpGet]
        public IHttpActionResult TypeRoom()
        {
            List<TypeRoom> enumType = new List<TypeRoom>();

            foreach (AxdEnum_STMTypeRoomJIS val in Enum.GetValues(typeof(AxdEnum_STMTypeRoomJIS)))
            {
                enumType.Add(new TypeRoom
                {
                    Type = val.ToString()
                });
            }

            return Json(enumType);
        }

        [Route("api/trackjob/trackstatus")]
        [HttpGet]
        public IHttpActionResult TrackStatus()
        {
            List<TrackStatus> enumType = new List<TrackStatus>();
            foreach (AxdEnum_STMTrackStatus val in Enum.GetValues(typeof(AxdEnum_STMTrackStatus)))
            {
                enumType.Add(new TrackStatus
                {
                    Status = val.ToString()
                });
            }

            return Json(enumType);
        }
    }
}
