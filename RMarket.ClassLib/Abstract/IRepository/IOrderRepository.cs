using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface IOrderRepository :IDisposable
    {
        IQueryable<Order> Orders { get; }
        Order Find(int id);
        int Save(Order order);
    }
}
