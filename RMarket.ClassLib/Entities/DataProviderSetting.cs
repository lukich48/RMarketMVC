﻿using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{

    [MetadataType(typeof(DataProviderSetting_metadata))]
    public class DataProviderSetting : IEntityData, IEntitySetting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EntityInfoId { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string StrParams { get; set; }

        public EntityInfo EntityInfo { get; set; }
    }
}