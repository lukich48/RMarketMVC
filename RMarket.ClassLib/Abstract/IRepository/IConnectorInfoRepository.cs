using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface IConnectorInfoRepository : IDisposable
    {
        IQueryable<ConnectorInfo> ConnectorInfoes { get; }
        ConnectorInfo Find(int id);
        int Save(ConnectorInfo connectorInfo);
        int Remove(int id);
    }
}
