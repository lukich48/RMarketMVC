using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Models;
using System.Data.Entity;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Infrastructure;

namespace RMarket.ClassLib.EFRepository
{
    public class EFSettingRepository : ISettingRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public IQueryable<Setting> Settings
        {
            get
            {
                return context.Settings;//.Include(m => m.StrategyInfo);
            }
        }

        public Setting Find(int id)
        {
            return context.Settings.Find(id);
        }

        public SettingModel FindModel(int id)
        {
            Setting dto = context.Settings.Find(id);

            if (dto == null)
                return null;

            SettingModel setting = new SettingModel();
            setting.CopyObject(dto, d => new { d.StrategyInfo});
            IEnumerable<ParamEntity> savedParams = Serializer.Deserialize<IEnumerable<ParamEntity>>(dto.StrParams);

            IEntityInfo entityInfo = SettingHelper.GetEntityInfo(setting.TypeSetting, setting.EntityInfoId);
            setting.EntityParams = EntityHelper.GetEntityParams<ParamEntity>(entityInfo, savedParams);

            return setting;

        }

        public int Save(Setting setting, IEnumerable<ParamEntity> entityParams)
        {
            int res = 0;

            if (entityParams != null)
            {
                string jsonParam = Serializer.Serialize(entityParams);
                setting.StrParams = jsonParam;
            }

            setting.CreateDate = DateTime.Now;

            if (setting.Id == 0) //Insert
            {
                context.Settings.Add(setting);
                res = 1;
            }
            else //Update
            {
                context.Entry(setting).State = EntityState.Modified;
                res = 2;
            }

            context.SaveChanges();

            return res;
        }

        public int Save(SettingModel setting)
        {
            int res = 0;

            setting.CreateDate = DateTime.Now;

            Setting dto = new Setting();
            dto.CopyObject(setting, d => new { d.StrategyInfo });
            dto.StrParams = Serializer.Serialize(setting.EntityParams);

            if (setting.Id == 0) //Insert
            {
                context.Settings.Add(dto);
                res = 1;
            }
            else //Update
            {
                context.Entry(dto).State = EntityState.Modified;
                res = 2;
            }

            context.SaveChanges();

            return res;
        }

        public int Remove(int id)
        {
            int res = 0;

            Setting setting = context.Settings.Find(id);
            context.Settings.Remove(setting);
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
