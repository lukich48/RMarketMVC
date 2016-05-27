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
using RMarket.ClassLib.Infrastructure;
using System.Linq.Expressions;

namespace RMarket.ClassLib.EFRepository
{
    public class EFSelectionRepository:ISelectionRepository
    {
        private RMarketContext context = RMarketContext.Current;

        public IEnumerable<SelectionModel> Get()
        {
            return Current.Mapper.Map<IQueryable<Selection>, IEnumerable<SelectionModel>>(context.Selections);
        }

        public IEnumerable<SelectionModel> Get(Expression<Func<IQueryable<Selection>, IQueryable<Selection>>> expression)
        {
            IQueryable<Selection> dataCollection = expression.Compile()(context.Selections);

            return Current.Mapper.Map<IQueryable<Selection>, IEnumerable<SelectionModel>>(dataCollection);
        }

        public IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<Selection>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(context.Selections).ToList();

            return dataCollection;
        }

        public SelectionModel GetById(int id, bool includeAll)
        {
            Selection data = null;

            if (includeAll)
                data = context.Selections.IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = context.Selections.Find(id);

            SelectionModel instance = Current.Mapper.Map<Selection, SelectionModel>(data);

            return instance;
        }

        public SelectionModel GetById(int id, params Expression<Func<Selection, object>>[] includeProperties)
        {
            Selection data = null;

            if (includeProperties.Length > 0)
                data = context.Selections.IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = context.Selections.Find(id);

            SelectionModel instance = Current.Mapper.Map<Selection, SelectionModel>(data);

            return instance;
        }

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
