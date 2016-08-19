using AutoMapper;
using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Abstract.IRepository;
using RMarket.ClassLib.EFRepository;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Infrastructure
{
    /// <summary>
    /// Контекст для репозиториев
    /// </summary>
    public static class CurrentRepository
    {
        public static Entities_old.RMarketContext Context
        {
            get
            {
                //IKernel kernel = new StandardKernel();
                //kernel.Bind<RMarketContext>().ToSelf();
                //return kernel.TryGet<RMarketContext>();
                return new Entities_old.RMarketContext();
            }
        }

        //private static IAliveStrategyRepository _aliveStrategyRepository;
        //public static IAliveStrategyRepository AliveStrategyRepository
        //{
        //    get
        //    {
        //        if (_aliveStrategyRepository == null)
        //        {
        //            _aliveStrategyRepository = new EFAliveStrategyRepository();
        //        }
        //        return _aliveStrategyRepository;
        //    }
        //    set
        //    {
        //        _aliveStrategyRepository = value;
        //    }
        //}

        //private static ICandleRepository _candleRepository;
        //public static ICandleRepository CandleRepository
        //{
        //    get
        //    {
        //        if (_candleRepository == null)
        //        {
        //            _candleRepository = new EFCandleRepository();
        //        }
        //        return _candleRepository;
        //    }
        //    set
        //    {
        //        _candleRepository = value;
        //    }
        //}

        //private static IConnectorInfoRepository _connectorInfoRepository;
        //public static IConnectorInfoRepository ConnectorInfoRepository
        //{
        //    get
        //    {
        //        if (_connectorInfoRepository == null)
        //        {
        //            _connectorInfoRepository = new EFConnectorInfoRepository();
        //        }
        //        return _connectorInfoRepository;
        //    }
        //    set
        //    {
        //        _connectorInfoRepository = value;
        //    }
        //}

        //private static IInstanceRepository _instanceRepository;
        //public static IInstanceRepository InstanceRepository
        //{
        //    get
        //    {
        //        if (_instanceRepository == null)
        //        {
        //            _instanceRepository = new EFInstanceRepository();
        //        }
        //        return _instanceRepository;
        //    }
        //    set
        //    {
        //        _instanceRepository = value;
        //    }
        //}

        //private static IOrderRepository _orderRepository;
        //public static IOrderRepository OrderRepository
        //{
        //    get
        //    {
        //        if (_orderRepository == null)
        //        {
        //            _orderRepository = new EFOrderRepository();
        //        }
        //        return _orderRepository;
        //    }
        //    set
        //    {
        //        _orderRepository = value;
        //    }
        //}


        //private static ISelectionRepository _selectionRepository;
        //public static ISelectionRepository SelectionRepository
        //{
        //    get
        //    {
        //        if (_selectionRepository == null)
        //        {
        //            _selectionRepository = new SelectionService();
        //        }
        //        return _selectionRepository;
        //    }
        //    set
        //    {
        //        _selectionRepository = value;
        //    }
        //}

        //private static ISettingRepository _settingRepository;
        //public static ISettingRepository SettingRepository
        //{
        //    get
        //    {
        //        if (_settingRepository == null)
        //        {
        //            _settingRepository = new EFSettingRepository();
        //        }
        //        return _settingRepository;
        //    }
        //    set
        //    {
        //        _settingRepository = value;
        //    }
        //}

        //private static IStrategyInfoRepository _strategyInfoRepository;
        //public static IStrategyInfoRepository StrategyInfoRepository
        //{
        //    get
        //    {
        //        if (_strategyInfoRepository == null)
        //        {
        //            _strategyInfoRepository = new EFStrategyInfoRepository();
        //        }
        //        return _strategyInfoRepository;
        //    }
        //    set
        //    {
        //        _strategyInfoRepository = value;
        //    }
        //}

        //private static ITickerRepository _tickerRepository;
        //public static ITickerRepository TickerRepository
        //{
        //    get
        //    {
        //        if (_tickerRepository == null)
        //        {
        //            _tickerRepository = new EFTickerRepository();
        //        }
        //        return _tickerRepository;
        //    }
        //    set
        //    {
        //        _tickerRepository = value;
        //    }
        //}

        private static ITickRepository _tickRepository;
        public static ITickRepository TickRepository
        {
            get
            {
                if (_tickRepository == null)
                {
                    _tickRepository = new EFTickRepository();
                }
                return _tickRepository;
            }
            set
            {
                _tickRepository = value;
            }
        }

        //private static ITimeFrameRepository _timeFrameRepository;
        //public static ITimeFrameRepository TimeFrameRepository
        //{
        //    get
        //    {
        //        if (_timeFrameRepository == null)
        //        {
        //            _timeFrameRepository = new EFTimeFrameRepository();
        //        }
        //        return _timeFrameRepository;
        //    }
        //    set
        //    {
        //        _timeFrameRepository = value;
        //    }
        //}

    }
}
