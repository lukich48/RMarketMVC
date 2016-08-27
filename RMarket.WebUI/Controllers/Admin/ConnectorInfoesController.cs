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
    public class ConnectorInfoesController : Controller
    {
        private IConnectorInfoRepository connectorInfoRepository;

        public ConnectorInfoesController(IConnectorInfoRepository connectorInfoRepository)
        {
            this.connectorInfoRepository = connectorInfoRepository;
        }

        // GET: ConnectorInfoes
        public ActionResult Index()
        {
            return View(connectorInfoRepository.Get().ToList());
        }

        // GET: ConnectorInfoes/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo entityInfo = connectorInfoRepository.GetById(id);
            if (entityInfo == null)
            {
                return HttpNotFound();
            }
            return View(entityInfo);
        }

        // GET: ConnectorInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConnectorInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TypeName,Name")] EntityInfo entityInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(entityInfo);
                return RedirectToAction("Index");
            }

            return View(entityInfo);
        }

        // GET: ConnectorInfoes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo EntityInfo = connectorInfoRepository.GetById(id);
            if (EntityInfo == null)
            {
                return HttpNotFound();
            }
            return View(EntityInfo);
        }

        // POST: ConnectorInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeName,Name")] EntityInfo entityInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(entityInfo);
                return RedirectToAction("Index");
            }
            return View(entityInfo);
        }

        // GET: ConnectorInfoes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntityInfo EntityInfo = connectorInfoRepository.GetById(id);
            if (EntityInfo == null)
            {
                return HttpNotFound();
            }
            return View(EntityInfo);
        }

        // POST: ConnectorInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            connectorInfoRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
