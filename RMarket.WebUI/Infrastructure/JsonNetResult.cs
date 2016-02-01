using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Text;

namespace RMarket.WebUI.Infrastructure
{
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings Settings {get;set;}

        public JsonNetResult()
        {
            this.ContentType = "application/json";
        }

        public JsonNetResult(object data, JsonRequestBehavior jsonRequestBehavior, JsonSerializerSettings settings = null)
        {
            this.ContentType = "application/json";
            this.Data = data;
            this.JsonRequestBehavior = jsonRequestBehavior;
            this.Settings = settings;
        }

        public JsonNetResult(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior jsonRequestBehavior)
        {
            this.ContentEncoding = contentEncoding;
            this.ContentType = !string.IsNullOrWhiteSpace(contentType) ? contentType : "application/json";
            this.Data = data;
            this.JsonRequestBehavior = jsonRequestBehavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data, Settings);
            response.Write(serializedObject);
        }
    }
}