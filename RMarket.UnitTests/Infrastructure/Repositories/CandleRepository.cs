using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure.Repositories
{
    public class CandleRepository: RepositoryBase<Candle>, ICandleRepository
    {
        public CandleRepository()
        {
            string filePath = UnitTestHelper.GetTestDataPath(@"Infrastructure\files\SBER_10_160601_160601.csv");
            //запустить обход файла
            using (StreamReader sr = new StreamReader(filePath))
            {
                string strJson = sr.ReadToEnd();
                context = Serializer.Deserialize<List<Candle>>(strJson);
            }

        }
    }
}
