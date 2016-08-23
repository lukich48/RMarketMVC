using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Infrastructure.AmbientContext
{
    /// <summary>
    /// Не пользуйтесь им если нет на то крайней необходимости!
    /// </summary>
    public abstract class Resolver
    {
        private static Resolver _current;
        public static Resolver Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        public abstract object Resolve(Type type);
    }
}
