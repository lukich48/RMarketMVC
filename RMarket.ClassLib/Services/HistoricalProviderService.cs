using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract;

namespace RMarket.ClassLib.Services
{
    public class HistoricalProviderService : EntityServiceBase<HistoricalProviderSetting, HistoricalProviderSettingModel>, IHistoricalProviderSettingService
    {
        public HistoricalProviderService(IHistoricalProviderSettingRepository settingRepository)
            :base(settingRepository)
        {
        }

    }
}
