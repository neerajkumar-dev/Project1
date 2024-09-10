using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace Project1.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
          using (ProductDbEntities db = new ProductDbEntities())
            {
                List<Table> ProductList= (from data in db.Tables select data).ToList();
                return View(ProductList);
            }
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View(new Table());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Table product,HttpPostedFileBase postedFile)
        {
            try
            {
                // TODO: Add insert logic here

                string extension = Path.GetExtension(postedFile.FileName);
                if(extension.Equals(".jpg")|| extension.Equals(".png"))
                {
                    string filename = "IMG-" + DateTime.Now.ToString("yyyyyMMddhhmmssffff")+extension;
                    string savepath = Server.MapPath("~/Content/Images/");
                    postedFile.SaveAs(savepath+filename);
                    product.Prod_Pic = filename;
                    using(ProductDbEntities db= new ProductDbEntities())
                    {
                        db.Tables.Add(product);
                        db.SaveChanges();
                    }
                }
                else
                {
                    return Content("<h1>You can only upload jpg or png file!!</h1>");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            using(ProductDbEntities db = new ProductDbEntities())
            {
                //Table product = (from data in db.Tables where data.Id == id select data).Single();
                Table product = db.Tables.Find(id);
                return View(product);
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Table product,HttpPostedFileBase postedFile)
        {
            try
            {
                // TODO: Add update logic here
                string filename = "";
                if(postedFile != null)
                {
                    string extension= Path.GetExtension(postedFile.FileName);
                    if (extension.Equals(".jpg") || extension.Equals(".png"))
                    {
                        filename = "IMG-" + DateTime.Now.ToString("yyyyyMMddhhmmssffff") + extension;
                        string savepath = Server.MapPath("~/Content/Images/");
                        postedFile.SaveAs(savepath + filename);
                    }

                }
                using (ProductDbEntities db= new ProductDbEntities())
                {
                    Table tbl=db.Tables.Find(product.Id);
                    tbl.Prod_Name = product.Prod_Name;
                    tbl.Prod_Price = product.Prod_Price;
                    tbl.Prod_qty = product.Prod_qty;
                    if (!filename.Equals(""))
                    {
                        tbl.Prod_Pic = filename;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (ProductDbEntities db= new ProductDbEntities())
            {
                db.Tables.Remove(db.Tables.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
