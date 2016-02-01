using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Helpers.Extentions;

namespace RMarket.ClassLib.EFRepository
{
    public class EFInstanceRepository : IInstanceRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IQueryable<Instance> Instances 
        {
            get
            {
                return context.Instances;//.Include(m=>m.StrategyInfo).Include(m => m.Ticker).Include(m => m.TimeFrame);
            }
        }

        public Instance Find(int id)
        {
            return context.Instances.Find(id);
        }

        public InstanceModel FindModel(int id)
        {
            Instance data = context.Instances.Find(id);

            if (data == null)
                return null;

            InstanceModel instance = new InstanceModel();
            instance.FillFields(data, d => new { d.Ticker, d.TimeFrame, d.StrategyInfo, d.Selection }); 
            instance.StrategyParams = StrategyHelper.GetStrategyParams(data);

            return instance;
        }


        public int Save(Instance instance, IEnumerable<ParamEntity> strategyParams)
        {
            int res = 0;

            if (strategyParams != null)
            {
                string jsonParam = Serializer.Serialize(strategyParams); 
                instance.StrParams = jsonParam;
            }

            instance.CreateDate = DateTime.Now;

            if (instance.Id == 0) //Insert
            {
                instance.GroupID = Guid.NewGuid();
                context.Instances.Add(instance);
                res = 1;
            }
            else //Update
            {
                context.Instances.Add(instance);
                res = 2;
            }

            
            context.SaveChanges();

            return res;
        }

        public int Save(InstanceModel instance)
        {
            int res = 0;

            instance.CreateDate = DateTime.Now;

            Instance dto = new Instance();
            dto.FillFields(instance, d=> new { d.Ticker, d.TimeFrame, d.StrategyInfo, d.Selection });
            dto.StrParams = Serializer.Serialize(instance.StrategyParams);

            if (instance.Id == 0) //Insert
            {
                dto.GroupID = Guid.NewGuid();
                context.Instances.Add(dto);
                res = 1;
            }
            else //Update
            {
                context.Instances.Add(dto);
                res = 2;
            }

            context.SaveChanges();

            return res;
        }

        #region IDisposable
        public void Dispose()
        {
            context.Dispose();
        }
        #endregion
    }
}
