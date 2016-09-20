using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMarket.WebUI.Controllers
{
    public class EntityParamsController : Controller
    {
        private readonly IEntityInfoRepository entityInfoRepository;

        public EntityParamsController(IEntityInfoRepository entityInfoRepository)
        {
            this.entityInfoRepository = entityInfoRepository;
        }

        //Новый экземпляр
        public PartialViewResult EditParamsNew(int entityInfoId)
        {
            EntityInfo entityInfo = entityInfoRepository.GetById(entityInfoId);
            IEnumerable<ParamEntity> entityParams = new SettingHelper().GetEntityParams<ParamEntity>(entityInfo);

            //Конвертим параметры в UI модель
            IEnumerable<ParamEntityUI> entityParamsUI = MyMapper.Current
                .Map<IEnumerable<ParamEntity>, IEnumerable<ParamEntityUI>>(entityParams);

            return PartialView("EditParams", entityParamsUI);
        }
    }
}