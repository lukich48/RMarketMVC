using System;
using System.Collections.Generic;
using RMarket.ClassLib.Models;


namespace RMarket.ClassLib.Abstract
{
    public interface IDataProvider
    {
        event EventHandler<TickEventArgs> TickPoked;
        bool ServerIsStarted { get; }

        void StartServer();
        void StopServer();
    }

}
