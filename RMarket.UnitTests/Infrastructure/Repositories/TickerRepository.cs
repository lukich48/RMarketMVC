using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Repositories
{
    public class TickerRepository: RepositoryBase<Ticker>, ITickerRepository
    {
        public TickerRepository ()
        {
            context = new List<Ticker>
            {
                new Ticker {Id=1, Name="SBER", Code="SBER", QtyInLot=10},
                new Ticker {Id=2,  Name="GAZP", Code="GAZP"},
                new Ticker {Id=3, Name="AVAZ", Code="AVAZ"},
            };
        }
    }
}
