using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    public class CustomException: Exception
    {
        public CustomException(string massage)
            :base(massage)
        {
            
        }
    }
}
