using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PETNET.Models;

namespace PETNET.Controllers
{
    public class HomeController : Controller
    {
       PetnetDB db=new PetnetDB();


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sahiplenme()
        {
          
            return View(db.Animals.ToList());
        }

        public ActionResult Sahiplendirme()
        {


            ViewBag.UserId = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        [HttpPost]

        public ActionResult Sahiplendirme(User user,Animal animal, HttpPostedFileBase Photo)
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
                    animal.UserID = Convert.ToInt32(Session["userid"]);

                }

                db.Animals.Add(animal);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }


            return View(animal);
        }
        public ActionResult Forum()
        {
            return View(db.Blogs.ToList());
        }

        public ActionResult ForumCreate()
        {
            ViewBag.CategorieID = new SelectList(db.Categories, "CategorieID", "CategorieName");
            ViewBag.UserId = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult ForumCreate(User user,Blog blog, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);

                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/BlogsPhoto/" + newPhoto);
                    blog.Photo = "/Uploads/BlogsPhoto/" + newPhoto;

                    blog.UserID = Convert.ToInt32(Session["userid"]);

                }

                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }


            return View(blog);
          
        }

        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult Kayit()
        {
            return View();
        }
        public ActionResult Giris()
        {
            return View();
        }
        public ActionResult ChangeLanguage(string lang)
        {
            new SiteLanguage().SetLanguage(lang);
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return RedirectToAction("Index", controllerName);
        }

    }
}