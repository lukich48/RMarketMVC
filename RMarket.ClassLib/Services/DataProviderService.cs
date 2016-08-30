using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract;

namespace RMarket.ClassLib.Services
{
    public class DataProviderService : EntityServiceBase<DataProviderSetting, DataProviderSettingModel>, IDataProviderSettingService
    {
        public DataProviderService(IDataProviderSettingRepository settingRepository)
            :base(settingRepository)
        {
        }

    }
}
