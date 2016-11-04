using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Abstract
{
    /// <summary>
    /// Экземпляры этого интерфейса регистрируются в контейнере
    /// Если нужна особенная регистрация создайте статический метод DependencyRegister(IServiceContainer container)
    /// </summary>
    public interface IDependency
    {
    }
}
