using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Infrastructure.AmbientContext;
using RMarket.ClassLib.Models;
using RMarket.WebUI.Models;
using RMarket.WebUI.Models.ParamforEdit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        /// <summary>
        /// Частичное представление выводит значение параметра с атрибутом ParameterAttribute
        /// </summary>
        /// <param name="paramEntityEdit">ключ - имя элемента</param>
        /// <returns></returns>
        public PartialViewResult EditorForObject(ParamEntityEditForObject paramEntityEdit)
        {
            // разные вью на разные типы значений
            object originValue = paramEntityEdit.ParamEntityUI.OriginValue;

            if (originValue is bool)
                return PartialView("EditorForBool", paramEntityEdit);
            else if (originValue is IEnumerable && !(originValue is string))
                return PartialView("EditorForCollection", paramEntityEdit);
            else if (originValue is Enum)
            {
                return EditForEnum(paramEntityEdit);
            }
            else
                return PartialView(paramEntityEdit);

        }

        public string GetDesctiptionForEnum(string enumValue, string typeName)
        {
            Type type = Type.GetType(typeName);
            var value = Enum.Parse(type, enumValue);
            return ((Enum)value).Description();
        }

        private PartialViewResult EditForEnum(ParamEntityEditForObject paramEntityEdit)
        {
            object originValue = paramEntityEdit.ParamEntityUI.OriginValue;

            var enumValues = Enum.GetValues(originValue.GetType());
            var enumValuesSelectList = new List<SelectListItem>();
            var listDescriptions = new List<SelectListItem>();
            foreach (var enumValue in enumValues)
            {
                enumValuesSelectList.Add(new SelectListItem
                {
                    Value = ((Enum)enumValue).ToString("F"),
                    Text = ((Enum)enumValue).Name(),
                    Selected = enumValue.ToString() == originValue.ToString()
                });

                listDescriptions.Add(new SelectListItem
                {
                    Value = ((Enum)enumValue).ToString("F"),
                    Text = ((Enum)enumValue).Description(),
                    Selected = enumValue == originValue
                });
            }

            var model = new ParamEntityUiForEnum
            {
                ControlId = paramEntityEdit.ControlId,
                ParamEntityUI = paramEntityEdit.ParamEntityUI,
                ValuesSelectList = enumValuesSelectList,
                //DescriptionCollection = listDescriptions
            };

            return PartialView("EditorForEnum", model);
        }
    }
}