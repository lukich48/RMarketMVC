using System;
using System.Collections.Generic;
using RMarket.ClassLib.Entities;
using System.Linq;

namespace RMarket.ClassLib.Abstract
{
    public interface ITimeFrameRepository:IDisposable
    {
        IQueryable<TimeFrame> TimeFrames { get; }
        TimeFrame Find(int id);
        int Save(TimeFrame timeFrame);
        int Remove(int id);
    }
}
