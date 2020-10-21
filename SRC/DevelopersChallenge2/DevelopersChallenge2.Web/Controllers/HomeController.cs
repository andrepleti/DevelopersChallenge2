using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DevelopersChallenge2.Web.Data.Context;
using DevelopersChallenge2.Web.Models;
using DevelopersChallenge2.Web.Util;

namespace DevelopersChallenge2.Web.Controllers
{
    public class HomeController : Controller
    {
        private DevelopersChallenge2Context db = new DevelopersChallenge2Context();

        // GET: TransactionBanks
        public ActionResult Index()
        {
            decimal total = 0;

            if (db.TransactionsBank.Any(x => x.Reconciliation))
                total = db.TransactionsBank.Where(x => x.Reconciliation).Sum(x => x.Value);

            ViewBag.Total = "Total amount reconciliation" + total;
            return View(db.TransactionsBank.ToList());
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] files)
        {
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {
                List<TransactionBank> list = new List<TransactionBank>();
                //iterating through multiple file collection   
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                        file.SaveAs(path);
                        List<TransactionBank> listAux = ReadOfx.toXElement(path);
                        foreach (TransactionBank item in listAux)
                        {
                            if (!list.Any(x => x.DateOperation == item.DateOperation && x.Value == item.Value && x.Description == item.Description))
                                list.Add(item);
                        }
                    }

                }

                List<TransactionBank> listDb = db.TransactionsBank.ToList();

                foreach (TransactionBank item in list)
                {
                    if (!listDb.Any(x => x.DateOperation == item.DateOperation && x.Value == item.Value && x.Description == item.Description))
                        db.TransactionsBank.Add(item);
                }

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: TransactionBanks/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: TransactionBanks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionBank transactionBank = db.TransactionsBank.Find(id);
            if (transactionBank == null)
            {
                return HttpNotFound();
            }
            return View(transactionBank);
        }

        // POST: TransactionBanks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeOperation,Description,DateOperation,Value,Reconciliation")] TransactionBank transactionBank)
        {
            if (ModelState.IsValid)
            {
                TransactionBank obj = db.TransactionsBank.Where(x => x.Id == transactionBank.Id).FirstOrDefault();
                obj.Reconciliation = transactionBank.Reconciliation;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transactionBank);
        }

        // GET: TransactionBanks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionBank transactionBank = db.TransactionsBank.Find(id);
            if (transactionBank == null)
            {
                return HttpNotFound();
            }
            return View(transactionBank);
        }

        // POST: TransactionBanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionBank transactionBank = db.TransactionsBank.Find(id);
            db.TransactionsBank.Remove(transactionBank);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
