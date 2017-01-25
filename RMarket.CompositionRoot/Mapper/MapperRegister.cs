using AutoMapper;
using RMarket.ClassLib.Helpers.Extentions.IEnumerableExtension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.CompositionRoot.Mapper
{
    public class MapperRegister
    {
        /// <summary>
        /// Инициализация автомаппера
        /// </summary>
        /// <param name="profiles"></param>
        public MapperAutoMapper SetMapperConfiguration()
        {
            //Обойдем сборку и зарегистрируем все профили 
            //todo: переделать на метод AddProfiles
            var profiles = new ConcurrentBag<Profile>();
            Parallel.ForEach(AppDomain.CurrentDomain.GetAssemblies(), ass =>
            {
                try
                {
                    ass.GetTypes()
                        .AsParallel()
                        .Where(t => typeof(Profile).IsAssignableFrom(t))
                        .ForEach(t =>
                        {
                            var prof = (Profile)Activator.CreateInstance(t);
                            profiles.Add(prof);
                        });
                }
                catch (Exception)
                {
                    // Не удалось загрузить одну из связанных сборок
                    //EventLog.WriteEntry(this.GetType().Namespace, exception.ToString(),
                    //    EventLogEntryType.FailureAudit);
                }
            });

            var customMapper = new MapperAutoMapper(profiles);
            //RMarket.ClassLib.Infrastructure.AmbientContext.MyMapper.Current = customMapper;
            return customMapper;
        }

    }
}
