using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackJobAPI.TrackJobLineService;

namespace TrackJobAPI.Classes
{
    public class ClientLine
    {
        public static EntityKey[] read(string value)
        {
            KeyField keyField = new KeyField() { Field = "RecId", Value = value };
            EntityKey entityKey = new EntityKey();

            entityKey.KeyData = new KeyField[1] { keyField };

            EntityKey[] entityKeys = new EntityKey[1] { entityKey };


            return entityKeys;
        }
    }
}