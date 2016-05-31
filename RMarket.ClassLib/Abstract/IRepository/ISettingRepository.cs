using RMarket.ClassLib.Entities;
using RMarket.ClassLib.EntityModels;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    public interface ISettingRepository
    {
        IQueryable<Setting> Settings { get; }
        Setting Find(int id);
        SettingModel FindModel(int id);
        int Save(Setting setting, IEnumerable<ParamEntity> entityParams);
        int Save(SettingModel setting);
        int Remove(int id);

    }
}
