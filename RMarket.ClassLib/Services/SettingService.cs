using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract;

namespace RMarket.ClassLib.Services
{
    public class SettingService : EntityServiceBase<Setting, SettingModel>, ISettingService
    {
        private readonly ISettingRepository settingRepository;

        public SettingService(ISettingRepository settingRepository)
            :base(settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        public IEntityInfo GetEntityInfo(SettingType settingType, int entityInfoId)
        {


            return settingRepository.GetEntityInfo(settingType, entityInfoId);
        }

    }
}
