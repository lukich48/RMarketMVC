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
    [MetadataType(typeof(Selection_metadata))]
    public class SelectionModel
    {
        private StrategyInfo _strategyInfo;
        private Ticker _ticker;
        private TimeFrame _timeFrame;
        private IEnumerable<Instance> _instances;

        public IStrategyInfoRepository strategyInfoRepository = Current.StrategyInfoRepository;
        public ITickerRepository tickerRepository = Current.TickerRepository;
        public ITimeFrameRepository timeFrameRepository = Current.TimeFrameRepository;
        public IInstanceRepository instanceRepository = Current.InstanceRepository;


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

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int AmountResults { get; set; }

        public List<ParamSelection> SelectionParams { get; set; }

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

        public IEnumerable<Instance> Instances
        {
            get
            {
                if (_instances == null)
                    _instances = instanceRepository.Instances.Where(i => i.SelectionId == Id).ToList();
                return _instances;
            }
            set
            {
                _instances = value;
            }
        }

        public SelectionModel()
        {
            SelectionParams = new List<ParamSelection>();
        }

    }
}
