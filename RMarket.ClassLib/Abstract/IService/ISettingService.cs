using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;

namespace RMarket.ClassLib.Abstract.IService
{
    public interface ISettingService : IEntityService<Setting,SettingModel>
    {
        IEntityInfo GetEntityInfo(SettingType settingType, int entityInfoId);
    }
}
