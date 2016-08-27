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
    public class EntityInfoesController : Controller
    {
        private IEntityInfoRepository strategyInfoRepository;

        public EntityInfoesController(IEntityInfoRepository strategyInfoRepository)
        {
            this.strategyInfoRepository = strategyInfoRepository;
        }

        // GET: EntityInfoes
        public ActionResult Index()
        {
            return View(strategyInfoRepository.Get().ToList());
        }

        // GET: EntityInfoes/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo entityInfo = strategyInfoRepository.GetById(id);
            if (entityInfo == null)
            {
                return HttpNotFound();
            }
            return View(entityInfo);
        }

        // GET: EntityInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntityInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TypeName,Name")] EntityInfo entityInfo)
        {
            if (ModelState.IsValid)
            {
                strategyInfoRepository.Save(entityInfo);
                return RedirectToAction("Index");
            }

            return View(entityInfo);
        }

        // GET: EntityInfoes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo entityInfo = strategyInfoRepository.GetById(id);
            if (entityInfo == null)
            {
                return HttpNotFound();
            }
            return View(entityInfo);
        }

        // POST: EntityInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeName,Name")] EntityInfo entityInfo)
        {
            if (ModelState.IsValid)
            {
                strategyInfoRepository.Save(entityInfo);
                return RedirectToAction("Index");
            }
            return View(entityInfo);
        }

        // GET: EntityInfoes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo entityInfo = strategyInfoRepository.GetById(id);
            if (entityInfo == null)
            {
                return HttpNotFound();
            }
            return View(entityInfo);
        }

        // POST: EntityInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            strategyInfoRepository.Remove(id);
            return RedirectToAction("Index");
        }

    }
}
