using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.UnitTests.Infrastructure
{
    public class UnitTestHelper
    {
        /// <summary>
        /// Возвращает абсолютный путь
        /// </summary>
        /// <param name="filePath">Относительный путь от папки проекта</param>
        /// <returns></returns>
        public static string GetTestDataPath(string filePath)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 2));
            return Path.Combine(projectPath, filePath);
        }
    }
}
