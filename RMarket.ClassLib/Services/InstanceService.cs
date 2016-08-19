using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Abstract.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.ClassLib.Services
{
    public class InstanceService: EntityServiceBase<Instance, InstanceModel>, IInstanceService
    {
        public InstanceService(IInstanceRepository instanceRepository
            )
            :base(instanceRepository)
        {
        }

    }
}
