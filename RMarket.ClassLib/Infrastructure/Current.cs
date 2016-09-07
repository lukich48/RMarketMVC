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
        public static IMapper Mapper { get; set; }

    }
}
