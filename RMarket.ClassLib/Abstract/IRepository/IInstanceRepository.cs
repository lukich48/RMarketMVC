using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using RMarket.ClassLib.EntityModels;

namespace RMarket.ClassLib.Abstract
{
    public interface IInstanceRepository:IEntityService<Instance,InstanceModel>, IDisposable
    {
    }
}
