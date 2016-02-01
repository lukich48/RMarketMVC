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
    public class InstanceModel
    {
        private StrategyInfo _strategyInfo;
        private Ticker _ticker;
        private TimeFrame _timeFrame;
        private Selection _selection;

        public IStrategyInfoRepository strategyInfoRepository = CurrentRepository.StrategyInfoRepository;
        public ITickerRepository tickerRepository = CurrentRepository.TickerRepository;
        public ITimeFrameRepository timeFrameRepository = CurrentRepository.TimeFrameRepository;
        public ISelectionRepository selectionRepository = CurrentRepository.SelectionRepository;


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

        public int? SelectionId { get; set; }

        public List<ParamEntity> StrategyParams { get; set; }

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

        public Selection Selection
        {
            get
            {
                if (_selection == null && SelectionId != 0 && SelectionId != null)
                    _selection = selectionRepository.Find((int)SelectionId);
                return _selection;
            }
            set
            {
                _selection = value;
            }
        }


        public InstanceModel()
        {
            StrategyParams = new List<ParamEntity>();
        }

    }
}
