using RMarket.ClassLib.Entities;

namespace RMarket.ClassLib.Abstract.IRepository
{
    public interface ISettingRepository: IEntityRepository<Setting>
    {
        IEntityInfo GetEntityInfo(SettingType settingType, int entityInfoId);
    }
}
