using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure
{
    public class SessionHelper
    {
        private HttpSessionStateBase session;

        public SessionHelper()
            : this(new HttpSessionStateWrapper(HttpContext.Current.Session))
        { }

        public SessionHelper(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public T Get<T>(string key) where T :new()
        {
            if (session[key] == null)
            {
                session[key] = new T();
            }       
           
            return (T)session[key];
        }

        public void Set(string key, object value)
        {
            session[key] = value;
        }
    }
}