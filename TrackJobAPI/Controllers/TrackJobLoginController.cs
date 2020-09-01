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
   
    public class TrackJobLoginController : ApiController
    {
        [HttpPost]
        [Route("api/login/user")]
        public IHttpActionResult GetUser(Login login)
        {
            try
            {
                DataSet dsUser = ExecuteStaticQuery.Get("STMTrackJobUser");

                var acc = (from a in dsUser.Tables[0].AsEnumerable()
                           where a.Field<string>("UserName").ToString() == login.UserName 
                                && a.Field<string>("Password").ToString() == login.Password
                           select a).ToList();

                if (login.UserName == "admin" && login.Password == "admin0001")
                {
                    return Json("OK");
                }
                else if (acc.Count == 0)
                {
                    return Json("NOK");
                }
                else 
                    return Json(acc);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    }
}
