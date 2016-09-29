using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Repositories
{
    public class OrderRepository: RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository()
        {
            context = new List<Order>();
        }
    }
}
