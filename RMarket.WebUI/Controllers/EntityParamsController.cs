using RMarket.ClassLib.Abstract;
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
        private readonly IResolver resolver;

        public EntityParamsController(IEntityInfoRepository entityInfoRepository, IResolver resolver)
        {
            this.entityInfoRepository = entityInfoRepository;
            this.resolver = resolver;
        }

        //Новый экземпляр
        public PartialViewResult EditParamsNew(int entityInfoId)
        {
            EntityInfo entityInfo = entityInfoRepository.GetById(entityInfoId);
            object entity = resolver.Resolve<object>(Type.GetType(entityInfo.TypeName));
            IEnumerable<ParamEntity> entityParams = new SettingHelper().GetEntityParams<ParamEntity>(entity);

            //Конвертим параметры в UI модель
            IEnumerable<ParamEntityUI> entityParamsUI = MyMapper.Current
                .Map<IEnumerable<ParamEntity>, IEnumerable<ParamEntityUI>>(entityParams);

            return PartialView("EditParams", entityParamsUI);
        }
    }
}