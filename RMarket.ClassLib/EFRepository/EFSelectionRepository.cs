using RMarket.ClassLib.Abstract;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Helpers;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RMarket.ClassLib.Helpers.Extentions;

namespace RMarket.ClassLib.EFRepository
{
    public class EFSelectionRepository:ISelectionRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IQueryable<Selection> Selections
        {
            get
            {
                return context.Selections;//.Include(m => m.StrategyInfo).Include(m => m.Ticker).Include(m => m.TimeFrame);
            }
        }

        public Selection Find(int id)
        {
            return context.Selections.Find(id);
        }

        public SelectionModel FindModel(int id)
        {
            Selection data = context.Selections.Find(id);

            if (data == null)
                return null;

            SelectionModel selection = new SelectionModel();
            selection.CopyObject(data, d => new { d.Ticker, d.TimeFrame, d.StrategyInfo });
            selection.SelectionParams = StrategyHelper.GetStrategyParams(data).ToList();

            return selection;
        }

        public int Save(Selection selection, IEnumerable<ParamSelection> selectionParams)
        {
            int res = 0;

            if (selectionParams != null)
            {
                string jsonParam = Serializer.Serialize(selectionParams);
                selection.StrParams = jsonParam;
            }

            selection.CreateDate = DateTime.Now;

            if (selection.Id == 0) //Insert
            {
                selection.GroupID = Guid.NewGuid();
                context.Selections.Add(selection);
                res = 1;
            }
            else //Update
            {
                context.Selections.Add(selection);
                res = 2;
            }

            context.SaveChanges();

            return res;
        }

        public int Save(SelectionModel selection)
        {
            int res = 0;

            selection.CreateDate = DateTime.Now;

            Selection dto = new Selection();
            dto.CopyObject(selection, d => new { d.Ticker, d.TimeFrame, d.StrategyInfo, d.Instances});
            dto.StrParams = Serializer.Serialize(selection.SelectionParams);

            if (selection.Id == 0) //Insert
            {
                dto.GroupID = Guid.NewGuid();
                context.Selections.Add(dto);
                res = 1;
            }
            else //Update
            {
                context.Selections.Add(dto);
                res = 2;
            }

            context.SaveChanges();

            return res;
        }

        #region IDisposable
        public void Dispose()
        {
            context.Dispose();
        }
        #endregion
    }
}
