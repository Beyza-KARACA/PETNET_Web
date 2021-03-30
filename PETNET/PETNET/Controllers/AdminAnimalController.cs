using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PETNET.Models;

namespace PETNET.Controllers
{
    public class AdminAnimalController : Controller
    {
       PetnetDB db = new PetnetDB();
        
        public ActionResult Index()
        {
            var animals = db.Animals.Include(a => a.User);
            return View(animals.ToList());
        }

       public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Animal animal,HttpPostedFileBase Photo)
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            try
            {
                if (ModelState.IsValid)
                {
                    if (Photo != null)
                    {
                        WebImage img = new WebImage(Photo.InputStream);
                        FileInfo photoInfo = new FileInfo(Photo.FileName);

                        string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                        img.Resize(800, 350);
                        img.Save("~/Uploads/AnimalsPhoto/" + newPhoto);
                        animal.Photo = "/Uploads/AnimalsPhoto/" + newPhoto;


                    }
                    db.Animals.Add(animal);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
           

            return View(animal);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", animal.UserID);
            return View(animal);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnimalID,UserID,Name,Gender,Type,Kind,Photo")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", animal.UserID);
            return View(animal);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = db.Animals.Find(id);
            db.Animals.Remove(animal);
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
