using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System.Data.Entity;

namespace RMarket.ClassLib.EFRepository
{
    public class EFConnectorInfoRepository : IConnectorInfoRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IQueryable<ConnectorInfo> ConnectorInfoes
        {
            get
            {
                return context.ConnectorInfoes;
            }
        }

        public ConnectorInfo Find(int id)
        {
            return context.ConnectorInfoes.Find(id);
        }

        public int Save(ConnectorInfo connectorInfo)
        {
            int res = 0;

            if (connectorInfo.Id == 0)
            {
                context.ConnectorInfoes.Add(connectorInfo);
            }
            else
            {
                context.Entry(connectorInfo).State = EntityState.Modified;
            }

            context.SaveChanges();

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            ConnectorInfo connectorInfo = context.ConnectorInfoes.Find(id);
            context.ConnectorInfoes.Remove(connectorInfo);
            context.SaveChanges();

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
