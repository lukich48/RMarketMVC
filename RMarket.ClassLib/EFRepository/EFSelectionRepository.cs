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
using RMarket.ClassLib.EntityModels;

namespace RMarket.ClassLib.EFRepository
{
    public class EFSelectionRepository:ISelectionRepository
    {
        private RMarketContext context = CurrentRepository.Context;

        public IEnumerable<SelectionModel> Get()
        {
            return Current.Mapper.Map<IQueryable<Selection>, IEnumerable<SelectionModel>>(Context.Selections);
        }

        public IEnumerable<SelectionModel> Get(Expression<Func<IQueryable<Selection>, IQueryable<Selection>>> expression)
        {
            IQueryable<Selection> dataCollection = expression.Compile()(Context.Selections);

            return Current.Mapper.Map<IQueryable<Selection>, IEnumerable<SelectionModel>>(dataCollection);
        }

        public IEnumerable<TResult> Get<TResult>(Expression<Func<IQueryable<Selection>, IQueryable<TResult>>> expression)
        {
            IEnumerable<TResult> dataCollection = expression.Compile()(Context.Selections).ToList();

            return dataCollection;
        }

        public SelectionModel GetById(int id, bool includeAll)
        {
            Selection data = null;

            if (includeAll)
                data = Context.Selections.IncludeAll().SingleOrDefault(i => i.Id == id);
            else
                data = Context.Selections.Find(id);

            SelectionModel instance = Current.Mapper.Map<Selection, SelectionModel>(data);

            return instance;
        }

        public SelectionModel GetById(int id, params Expression<Func<Selection, object>>[] includeProperties)
        {
            Selection data = null;

            if (includeProperties.Length > 0)
                data = Context.Selections.IncludeProperties(includeProperties).SingleOrDefault(i => i.Id == id);
            else
                data = Context.Selections.Find(id);

            SelectionModel instance = Current.Mapper.Map<Selection, SelectionModel>(data);

            return instance;
        }

        public IQueryable<Selection> Selections
        {
            get
            {
                return Context.Selections;//.Include(m => m.StrategyInfo).Include(m => m.Ticker).Include(m => m.TimeFrame);
            }
        }

        public RMarketContext Context
        {
            get
            {
                return context;
            }

            set
            {
                context = value;
            }
        }

        public Selection Find(int id)
        {
            return Context.Selections.Find(id);
        }

        public SelectionModel FindModel(int id)
        {
            Selection data = Context.Selections.Find(id);

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
                Context.Selections.Add(selection);
                res = 1;
            }
            else //Update
            {
                Context.Selections.Add(selection);
                res = 2;
            }

            Context.SaveChanges();

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
                Context.Selections.Add(dto);
                res = 1;
            }
            else //Update
            {
                Context.Selections.Add(dto);
                res = 2;
            }

            Context.SaveChanges();

            return res;
        }

        #region IDisposable
        public void Dispose()
        {
            Context.Dispose();
        }
        #endregion
    }
}
