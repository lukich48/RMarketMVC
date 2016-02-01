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
    public  class EFOrderRepository: IOrderRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IQueryable<Order> Orders
        {
            get
            {
                return context.Orders;
            }
        }

        public Order Find(int id)
        {
            return context.Orders.Find(id);
        }

        public int Save(Order order)
        {
            int res = 0;

            if (order.Id == 0)
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
            else
            {
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
            }

            return res;
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
