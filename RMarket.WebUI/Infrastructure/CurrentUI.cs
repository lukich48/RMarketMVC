using RMarket.ClassLib.Models;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMarket.WebUI.Infrastructure
{
    public static class CurrentUI
    {
        private static SessionHelper _sessionHelper;
        public static SessionHelper SessionHelper
        {
            get
            {
                if (_sessionHelper == null)
                    _sessionHelper = new SessionHelper();
                return _sessionHelper;
            }
            set
            {
                _sessionHelper = value;
            }
        }

        /// <summary>
        /// Запущенные стратегии
        /// </summary>
        public static List<AliveResult> AliveResults
        {
            get
            {
                return SessionHelper.Get<List<AliveResult>>("AliveResults");
            }
            set
            {
                SessionHelper.Set("AliveResults", value);
            }
        }

    }
}