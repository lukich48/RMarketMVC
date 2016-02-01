using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Entities
{
    public class AliveStrategy
    {
        public int Id { get; set; }

        ///<summary>
        ///Совпадает с GroupID объекта Instance
        ///</summary>
        public Guid GroupID { get; set; }

        ///<summary>
        ///Активная стратегия (могут быть незакрытые ордера)
        ///</summary>
        public bool IsActive { get; set; }

        ///<summary>
        ///Стратегия торгует на рынок 
        ///</summary>
        public bool IsReal { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
