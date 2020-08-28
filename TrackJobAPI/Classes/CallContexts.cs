using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TrackJobAPI.TrackJobServices;
namespace TrackJobAPI.Classes
{
    public class CallContexts
    {
        public static CallContext get()
        {
            CallContext callContext = new CallContext();

            callContext.MessageId = Guid.NewGuid().ToString();
            callContext.LogonAsUser = string.Format(@"{0}\{1}", UserAccount.Domain, UserAccount.Username);
            callContext.Language = "en-us";
 
            return callContext;
        }
    }
}