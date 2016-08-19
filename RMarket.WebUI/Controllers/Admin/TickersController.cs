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
using RMarket.ClassLib.Abstract.IRepository;

namespace RMarket.WebUI.Controllers.Admin
{
    public class TickersController : Controller
    {
        private ITickerRepository tickerRepository;

        public TickersController(ITickerRepository tickerRepository)
        {
            this.tickerRepository = tickerRepository;
        }

        // GET: Tickers
        public ActionResult Index()
        {
            return View(tickerRepository.Get().ToList());
        }

        // GET: Tickers/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticker ticker = tickerRepository.GetById(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }
            return View(ticker);
        }

        // GET: Tickers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tickers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticker ticker)
        {
            if (ModelState.IsValid)
            {
                tickerRepository.Save(ticker);
                return RedirectToAction("Index");
            }

            return View(ticker);
        }

        // GET: Tickers/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticker ticker = tickerRepository.GetById(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }
            return View(ticker);
        }

        // POST: Tickers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ticker ticker)
        {
            if (ModelState.IsValid)
            {
                tickerRepository.Save(ticker);
                return RedirectToAction("Index");
            }
            return View(ticker);
        }

        // GET: Tickers/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticker ticker = tickerRepository.GetById(id);
            if (ticker == null)
            {
                return HttpNotFound();
            }
            return View(ticker);
        }

        // POST: Tickers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tickerRepository.Remove(id);
            return RedirectToAction("Index");
        }

    }
}
