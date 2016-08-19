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
    public class TimeFramesController : Controller
    {
        ITimeFrameRepository timeFrameRepository;

        public TimeFramesController(ITimeFrameRepository timeFrameRepository)
        {
            this.timeFrameRepository = timeFrameRepository;
        }

        // GET: TimeFrames
        public ActionResult Index()
        {
            return View(timeFrameRepository.Get().ToList());
        }

        // GET: TimeFrames/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeFrame timeFrame = timeFrameRepository.GetById(id);
            if (timeFrame == null)
            {
                return HttpNotFound();
            }
            return View(timeFrame);
        }

        // GET: TimeFrames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimeFrames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ToMinute,CodeFinam")] TimeFrame timeFrame)
        {
            if (ModelState.IsValid)
            {
                timeFrameRepository.Save(timeFrame);
                return RedirectToAction("Index");
            }

            return View(timeFrame);
        }

        // GET: TimeFrames/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeFrame timeFrame = timeFrameRepository.GetById(id);
            if (timeFrame == null)
            {
                return HttpNotFound();
            }
            return View(timeFrame);
        }

        // POST: TimeFrames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ToMinute,CodeFinam")] TimeFrame timeFrame)
        {
            if (ModelState.IsValid)
            {
                timeFrameRepository.Save(timeFrame);
                return RedirectToAction("Index");
            }
            return View(timeFrame);
        }

        // GET: TimeFrames/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeFrame timeFrame = timeFrameRepository.GetById(id);
            if (timeFrame == null)
            {
                return HttpNotFound();
            }
            return View(timeFrame);
        }

        // POST: TimeFrames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            timeFrameRepository.Remove(id);
            return RedirectToAction("Index");
        }

    }
}
