using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.ClassLib.Services
{
    public class SettingService : EntityServiceBase<Setting, SettingModel>, ISettingService
    {
        public SettingService(ISettingRepository settingRepository)
            :base(settingRepository)
        {
        }

    }
}
