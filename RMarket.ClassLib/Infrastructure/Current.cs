using AutoMapper;
using RMarket.ClassLib.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Infrastructure
{
    /// <summary>
    /// Контекст для хелперов
    /// </summary>
    public static class Current
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = new AutomapperConfigurations.BasicConfigurarion().CreateDefaultConfiguration().CreateMapper();
                }
                return _mapper;

            }
            set
            {
                _mapper = value;
            }
        }
        private static IMapper _mapper;


    }
}
