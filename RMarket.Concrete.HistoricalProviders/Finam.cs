using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using RMarket.ClassLib.Models;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.Concrete.HistoricalProviders
{
    public class Finam : HistoricalProviderBase
    {
        [Parameter(Description ="ключ - код инструмента, значение - код с сайта финам")]
        public Dictionary<string,string> TickerCodeFinams { get; set; }

        [Parameter(Description = "ключ - обозначение интервала(в БД), значение - код с сайта финам")]
        public Dictionary<string, string> TimeFrameCodeFinams { get; set; }


        public Finam(ICandleRepository candleRepository)
            :base(candleRepository)
        {
            TickerCodeFinams = new Dictionary<string, string>();

            TimeFrameCodeFinams = new Dictionary<string, string>(8);
            TimeFrameCodeFinams.Add("tick", "1");
            TimeFrameCodeFinams.Add("1", "2");
            TimeFrameCodeFinams.Add("2", "3");
            TimeFrameCodeFinams.Add("10", "4");
            TimeFrameCodeFinams.Add("15", "5");
            TimeFrameCodeFinams.Add("30", "6");
            TimeFrameCodeFinams.Add("60", "7");
            TimeFrameCodeFinams.Add("day", "8");
        }

        public override IEnumerable<Candle> DownloadCandles(DateTime dateFrom, DateTime dateTo, Ticker ticker, TimeFrame timeFrame)
        {
            string tickerCodeFinam;
            if (!TickerCodeFinams.TryGetValue(ticker.Code, out tickerCodeFinam))
                throw new CustomException($"не найден код финама для инструмента: {ticker.Code}");

            string timeFrameCodeFinam;
            if(!TimeFrameCodeFinams.TryGetValue(timeFrame.Name, out timeFrameCodeFinam))
                throw new CustomException($"не найден код финама для тайм фрейма: {timeFrame.Name}");

            List<Candle> listCandles = new List<Candle>();

            //Пример: http://195.128.78.52/SBER_141008_141008.txt?market=1&em=3&code=SBER&df=8&mf=9&yf=2014&from=08.10.2014&dt=8&mt=9&yt=2014&to=08.10.2014&p=7&f=SBER_141008_141008&e=.txt&cn=SBER&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=1&sep2=1&datf=1&at=1
            StringBuilder sb = new StringBuilder("http://195.128.78.52/SBER_141008_141008.txt?market=1");
            sb.Append("&em=" + tickerCodeFinam); //Код тикера Финам
            sb.Append("&code=" + ticker.Code);
            sb.Append("&df=" + dateFrom.Day);
            sb.Append("&mf=" + (dateFrom.Month - 1));
            sb.Append("&yf=" + dateFrom.Year);
            sb.Append("&from=" + dateFrom.ToString("dd.MM.yyyy"));
            sb.Append("&dt=" + dateTo.Day);
            sb.Append("&mt=" + (dateTo.Month - 1));
            sb.Append("&yt=" + dateTo.Year);
            sb.Append("&to=" + dateTo.ToString("dd.MM.yyyy"));
            sb.Append("&p=" + timeFrameCodeFinam); //Код интервала Финам
            sb.Append($"&f={ticker.Code}_{dateFrom.ToString("yyMMdd")}_{dateTo.ToString("yyMMdd")}"); //название выходного файла
            sb.Append("&e=.csv"); //.txt .csv
            sb.Append("&cn=" + ticker.Code);
            sb.Append("&dtf=1&tmf=1");
            sb.Append("&MSOR=0"); //0 - Время начало свечи, 1-окончания
            sb.Append("&mstime=on&mstimever=1&sep=1&sep2=1&datf=1&at=1");
            sb.Append("&fsp=1"); //Заполнять периоды без сделок
            WebRequest reqGet = WebRequest.Create(sb.ToString());

            WebResponse resp = reqGet.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            if (sr.Peek() >= 0)
            {
                Dictionary<string, int> head = new Dictionary<string, int>();
                string strLine;
                int count = 0;

                while ((strLine = sr.ReadLine()) != null)
                {
                    string[] strArray = strLine.Split(new char[] { ',' });

                    count++;
                    if (count == 1) //Шапка
                    {
                        //Заполнить шапку
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            //<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
                            strArray[i] = Regex.Replace(strArray[i], "[<>]", "");
                            head.Add(strArray[i], i);
                        }
                        continue;
                    }

                    if (strArray.Length != head.Count)
                        continue; //бывают траблы(

                    //Сформировать свечу
                    DateTime dateOpen = DateTime.ParseExact(strArray[head["DATE"]] + strArray[head["TIME"]], "yyyyMMddHHmmss", null);
                    decimal OpenPrice = Convert.ToDecimal(strArray[head["OPEN"]], new CultureInfo("en-US"));
                    decimal HighPrice = Convert.ToDecimal(strArray[head["HIGH"]], new CultureInfo("en-US"));
                    decimal LowPrise = Convert.ToDecimal(strArray[head["LOW"]], new CultureInfo("en-US"));
                    decimal ClosePrice = Convert.ToDecimal(strArray[head["CLOSE"]], new CultureInfo("en-US"));
                    int Volume = Convert.ToInt32(strArray[head["VOL"]]);

                    Candle candle = new Candle(ticker.Id, timeFrame.Id, dateOpen, OpenPrice, HighPrice, LowPrise, ClosePrice, Volume);

                    listCandles.Add(candle);
                }
            }

            return listCandles;
            //ClassLib.Helpers.Serializer.Serialize(listCandles);

        }

    }
}
