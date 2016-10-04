using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Abstract;

namespace RMarket.ClassLib.Services
{
    public class OptimizationService : EntityServiceBase<OptimizationSetting, OptimizationSettingModel>, IOptimizationSettingService
    {
        public OptimizationService(IOptimizationSettingRepository settingRepository)
            :base(settingRepository)
        {
        }

    }
}
