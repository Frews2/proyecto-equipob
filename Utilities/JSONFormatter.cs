/*
 Date: 08/05/2021
 Author(s): Ricardo Moguel Sanchez 
 Cesar Sergio Martinez Palacios
*/
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSPrivateLibrary.Utilities
{
    public static class JSONFormatter
    {
        private static bool isSuccess;
        private static JObject msData;
        private static JObject returnObject;

        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static JObject SuccessMessageFormatter(string successMessage, object modelObject)
        {
            msData = new JObject
            {
                { "message", successMessage }
            };


            string jsonMessage = JsonConvert.SerializeObject(modelObject, Formatting.Indented, serializerSettings);

            if (IsListObject(modelObject))
            {
                msData["result"] = JArray.Parse(jsonMessage);
            }
            else
            {
                msData["result"] = JObject.Parse(jsonMessage);
            }

            isSuccess = true;
            returnObject = new JObject
            {
                { "success", isSuccess },
                { "origin", "private_library_service" },
                { "data", msData }
            };

            return returnObject;
        }

        public static JObject ErrorMessageFormatter(string failureMessage)
        {
            msData = new JObject
            {
                { "message", failureMessage },
                { "result", null }
            };

            isSuccess = false;
            returnObject = new JObject
            {
                { "success", isSuccess },
                { "origin", "private_library_service" },
                { "data", msData }
            };

            return returnObject;
        }

        private static bool IsListObject(object modelObject)
        {
            return (modelObject.GetType().IsGenericType && modelObject is IEnumerable<object>);
        }
    }
}