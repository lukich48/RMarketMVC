using System;
using System.Collections.Generic;
using RMarket.ClassLib.Models;


namespace RMarket.ClassLib.Abstract
{
    public interface IDataProvider: IDependency
    {
        event EventHandler<TickEventArgs> TickPoked;

        void StartServer();
        void StopServer();
    }

}
