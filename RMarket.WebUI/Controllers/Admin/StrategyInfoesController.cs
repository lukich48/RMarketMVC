using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RMarket.ClassLib.Entities;
using RMarket.ClassLib.Abstract;

namespace RMarket.WebUI.Controllers.Admin
{
    public class StrategyInfoesController : Controller
    {
        private IStrategyInfoRepository strategyInfoRepository;

        public StrategyInfoesController(IStrategyInfoRepository strategyInfoRepository)
        {
            this.strategyInfoRepository = strategyInfoRepository;
        }

        // GET: StrategyInfoes
        public ActionResult Index()
        {
            return View(strategyInfoRepository.StrategyInfoes.ToList());
        }

        // GET: StrategyInfoes/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrategyInfo strategyInfo = strategyInfoRepository.Find(id);
            if (strategyInfo == null)
            {
                return HttpNotFound();
            }
            return View(strategyInfo);
        }

        // GET: StrategyInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StrategyInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TypeName,Name")] StrategyInfo strategyInfo)
        {
            if (ModelState.IsValid)
            {
                strategyInfoRepository.Save(strategyInfo);
                return RedirectToAction("Index");
            }

            return View(strategyInfo);
        }

        // GET: StrategyInfoes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrategyInfo strategyInfo = strategyInfoRepository.Find(id);
            if (strategyInfo == null)
            {
                return HttpNotFound();
            }
            return View(strategyInfo);
        }

        // POST: StrategyInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeName,Name")] StrategyInfo strategyInfo)
        {
            if (ModelState.IsValid)
            {
                strategyInfoRepository.Save(strategyInfo);
                return RedirectToAction("Index");
            }
            return View(strategyInfo);
        }

        // GET: StrategyInfoes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StrategyInfo strategyInfo = strategyInfoRepository.Find(id);
            if (strategyInfo == null)
            {
                return HttpNotFound();
            }
            return View(strategyInfo);
        }

        // POST: StrategyInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            strategyInfoRepository.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                strategyInfoRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
