using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface IInstanceRepository:IDisposable
    {
        IQueryable<Instance> Instances { get; }
        Instance Find(int id);
        InstanceModel FindModel(int id);
        int Save(Instance instance, IEnumerable<ParamEntity> strategyParams);
        int Save(InstanceModel instance);
    }
}
