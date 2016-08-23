using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract;

namespace RMarket.ClassLib.Services
{
    public class SettingService : EntityServiceBase<DataProvider, DataProviderModel>, ISettingService
    {
        public SettingService(ISettingRepository settingRepository)
            :base(settingRepository)
        {
        }

    }
}
