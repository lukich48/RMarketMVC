using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure
{
    public static class SessionHelper
    {
        public static HttpSessionStateBase session;

        static SessionHelper()
        {
            if(HttpContext.Current!=null)
                session = new HttpSessionStateWrapper(HttpContext.Current.Session);
        }

        public static T Get<T>(string key) where T :new()
        {
            if (session[key] == null)
            {
                session[key] = new T();
            }       
           
            return (T)session[key];
        }

        public static void Set(string key, object value)
        {
            session[key] = value;
        }
    }
}