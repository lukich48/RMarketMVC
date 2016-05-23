﻿using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{

    [MetadataType(typeof(Setting_metadata))]
    public class SettingModel
    {
        private StrategyInfo _strategyInfo;

        public IStrategyInfoRepository strategyInfoRepository = Current.StrategyInfoRepository;


        public int Id { get; set; }

        public string Name { get; set; }

        public int? StrategyInfoId { get; set; }

        public SettingType TypeSetting { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ParamEntity> EntityParams { get; set; }

        public StrategyInfo StrategyInfo
        {
            get
            {
                if (_strategyInfo == null && StrategyInfoId != 0 && StrategyInfoId != null)
                    _strategyInfo = strategyInfoRepository.Find((int)StrategyInfoId);
                return _strategyInfo;
            }
            set
            {
                _strategyInfo = value;
            }
        }


        public SettingModel()
        {
            EntityParams = new List<ParamEntity>();
        }

    }
}
