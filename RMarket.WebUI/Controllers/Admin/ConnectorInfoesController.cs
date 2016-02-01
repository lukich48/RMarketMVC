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
            return View(connectorInfoRepository.ConnectorInfoes.ToList());
        }

        // GET: ConnectorInfoes/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectorInfo connectorInfo = connectorInfoRepository.Find(id);
            if (connectorInfo == null)
            {
                return HttpNotFound();
            }
            return View(connectorInfo);
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
        public ActionResult Create([Bind(Include = "Id,TypeName,Name")] ConnectorInfo connectorInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(connectorInfo);
                return RedirectToAction("Index");
            }

            return View(connectorInfo);
        }

        // GET: ConnectorInfoes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectorInfo ConnectorInfo = connectorInfoRepository.Find(id);
            if (ConnectorInfo == null)
            {
                return HttpNotFound();
            }
            return View(ConnectorInfo);
        }

        // POST: ConnectorInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeName,Name")] ConnectorInfo connectorInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(connectorInfo);
                return RedirectToAction("Index");
            }
            return View(connectorInfo);
        }

        // GET: ConnectorInfoes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectorInfo ConnectorInfo = connectorInfoRepository.Find(id);
            if (ConnectorInfo == null)
            {
                return HttpNotFound();
            }
            return View(ConnectorInfo);
        }

        // POST: ConnectorInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            connectorInfoRepository.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                connectorInfoRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
