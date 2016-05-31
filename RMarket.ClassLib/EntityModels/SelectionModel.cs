using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Infrastructure;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.EntityModels
{
    [MetadataType(typeof(Selection_metadata))]
    public class SelectionModel
    {
        private StrategyInfo _strategyInfo;
        private Ticker _ticker;
        private TimeFrame _timeFrame;
        private IEnumerable<Instance> _instances;

        public IStrategyInfoRepository strategyInfoRepository = CurrentRepository.StrategyInfoRepository;
        public ITickerRepository tickerRepository = CurrentRepository.TickerRepository;
        public ITimeFrameRepository timeFrameRepository = CurrentRepository.TimeFrameRepository;
        public IInstanceRepository instanceRepository = CurrentRepository.InstanceRepository;


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

        public StrategyInfo StrategyInfo { get; set; }

        public Ticker Ticker { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public SelectionModel()
        {
            SelectionParams = new List<ParamSelection>();
        }

        public void LoadNavigationProperties()
        {
            if (StrategyInfo == null && StrategyInfoId != 0)
                StrategyInfo = strategyInfoRepository.Find(StrategyInfoId);

            if (Ticker == null && TickerId != 0)
                Ticker = tickerRepository.Find(TickerId);

            if (TimeFrame == null && TimeFrameId != 0)
                TimeFrame = timeFrameRepository.Find(TimeFrameId);

        }


    }
}
