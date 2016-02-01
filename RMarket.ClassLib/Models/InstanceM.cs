using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Models
{
    [MetadataType(typeof(Instance_metadata))]
    public class InstanceM
    {
        private StrategyInfo _strategyInfo;
        private Ticker _ticker;
        private TimeFrame _timeFrame;

        public IStrategyInfoRepository strategyInfoRepository = CurrentRepository.StrategyInfoRepository;
        public ITickerRepository tickerRepository = CurrentRepository.TickerRepository;
        public ITimeFrameRepository timeFrameRepository = CurrentRepository.TimeFrameRepository;


        public int Id { get; set; }

        public string Name { get; set; }

        public int StrategyInfoId { get; set; }

        public int TickerId { get; set; }

        public int TimeFrameId { get; set; }

        public decimal Balance { get; set; }

        public decimal Slippage { get; set; }

        public decimal Rent { get; set; }

        public string Description { get; set; }

        public Guid GroupID { get; set; }

        public DateTime CreateDate { get; set; }

        public StrategyInfo StrategyInfo
        {
            get
            {
                if (_strategyInfo == null && StrategyInfoId != 0)
                    _strategyInfo = strategyInfoRepository.Find(StrategyInfoId);
                return _strategyInfo;
            }
            set
            {
                _strategyInfo = value;
            }
        }

        public Ticker Ticker
        {
            get
            {
                if (_ticker == null && TickerId != 0)
                    _ticker = tickerRepository.Find(TickerId);
                return _ticker;
            }
            set
            {
                _ticker = value;
            }
        }

        public TimeFrame TimeFrame
        {
            get
            {
                if (_timeFrame == null && TimeFrameId != 0)
                    _timeFrame = timeFrameRepository.Find(TimeFrameId);
                return _timeFrame;
            }
            set
            {
                _timeFrame = value;
            }
        }


        public IEnumerable<ParamEntity> StrategyParams { get; set; }

        public InstanceM()
        {
            StrategyParams = new List<ParamEntity>();
        }

    }
}
