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
            DataProviderInfo dataProviderInfo = connectorInfoRepository.GetById(id);
            if (dataProviderInfo == null)
            {
                return HttpNotFound();
            }
            return View(dataProviderInfo);
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
        public ActionResult Create([Bind(Include = "Id,TypeName,Name")] DataProviderInfo dataProviderInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(dataProviderInfo);
                return RedirectToAction("Index");
            }

            return View(dataProviderInfo);
        }

        // GET: ConnectorInfoes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataProviderInfo DataProviderInfo = connectorInfoRepository.GetById(id);
            if (DataProviderInfo == null)
            {
                return HttpNotFound();
            }
            return View(DataProviderInfo);
        }

        // POST: ConnectorInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeName,Name")] DataProviderInfo dataProviderInfo)
        {
            if (ModelState.IsValid)
            {
                connectorInfoRepository.Save(dataProviderInfo);
                return RedirectToAction("Index");
            }
            return View(dataProviderInfo);
        }

        // GET: ConnectorInfoes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataProviderInfo DataProviderInfo = connectorInfoRepository.GetById(id);
            if (DataProviderInfo == null)
            {
                return HttpNotFound();
            }
            return View(DataProviderInfo);
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
