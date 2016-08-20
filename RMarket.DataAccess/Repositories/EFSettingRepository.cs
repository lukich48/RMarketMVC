using RMarket.ClassLib.Entities;
using System;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.DataAccess.Context;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Models;

namespace RMarket.DataAccess.Repositories
{
    public class EFSettingRepository: EFRepositoryBase<Setting>, ISettingRepository
    {
        public EFSettingRepository(RMarketContext context)
            :base(context)
        { }

        /// <summary>
        /// Возвращает объект по типу настройки и идентификатору. Генерирует исключение, если неверно задан settingType
        /// </summary>
        /// <param name="settingType"></param>
        /// <param name="entityInfoId"></param>
        /// <returns></returns>
        public IEntityInfo GetEntityInfo(SettingType settingType, int entityInfoId)
        {
            IEntityInfo entityInfo = null;
            switch (settingType)
            {
                case SettingType.ConnectorInfo:
                    entityInfo = context.ConnectorInfoes.Find(entityInfoId);
                    break;
                default:
                    throw new CustomException($"SettingType: {settingType} is not defined!");

            }

            return entityInfo;
        }

    }
}
