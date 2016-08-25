using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.Concrete.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarker.Concrete.DataProviders.Infrastructure
{
    public class DataProvidersInfoContextInicializer: IContextInitializer<ConnectorInfo>
    {
        public IEnumerable<ConnectorInfo> Get()
        {
            return new List<ConnectorInfo>
            {
                new ConnectorInfo {TypeName=typeof(QuikProvider).AssemblyQualifiedName, Name="QuikProvider" },
                new ConnectorInfo {TypeName="RMarket.ClassLib.Connectors.CsvFileConnector, RMarket.ClassLib", Name="CsvFileProvider" },

            };
        }
    }
}
