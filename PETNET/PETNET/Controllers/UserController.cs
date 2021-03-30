using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PETNET.Models;

namespace PETNET.Controllers
{
    public class UserController : Controller
    {
        PetnetDB db = new PetnetDB();

        public ActionResult Index(int id)
        {
            var user = db.Users.Where(u => u.UserID == id).SingleOrDefault();

            if (Convert.ToInt32(Session["userid"]) != user.UserID)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user,string Password)
        {
         
            
            var login = db.Users.Where(u => u.UserName == user.UserName).SingleOrDefault();
           
                if (login.UserName == user.UserName && login.Password ==user.Password)
                {
                    Session["userid"] = login.UserID;
                    Session["username"] = login.UserName;
                    Session["roleid"] = login.RoleID;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "HATALI KULLANICI ADI YA DA  ŞİFRE GİRDİNİZ TEKRAR DENEYİNİZ!!!";
                   return View();
                }
          
 
        }

        public ActionResult Logout()
        {

            Session["userid"] = null;
            Session["username"] = null;
            Session["roleid"] = null;

            return RedirectToAction("Index", "Home");

        }


        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "Role1");
            return View();
        }

        [HttpPost]

        public ActionResult Create(User user, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);

                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UsersPhoto/" + newPhoto);
                    user.Photo = "/Uploads/UsersPhoto/" + newPhoto;

                    if (user.Bloggers == true)
                    {
                        user.RoleID = 3;
                    }
                    else
                    {
                        user.RoleID = 2;
                    }

                    Session["userid"] = user.UserID;
                    Session["username"] = user.UserName;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Bu alan boş geçilemez.Lütfen Fotoğraf ekleyiniz.");
                }

            }

            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var editUser = db.Users.Where(u => u.UserID == id).SingleOrDefault();
            return View(editUser);
        }

        [HttpPost]

        public ActionResult Edit(User user,int id,HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                var users = db.Users.Where(u => u.UserID == id).SingleOrDefault();
                if (Photo != null)
                {
                    if(System.IO.File.Exists(Server.MapPath(user.Photo)))
                    {
                        System.IO.File.Delete(Server.MapPath(user.Photo));
                    }
                    WebImage img = new WebImage(Photo.InputStream);
                    FileInfo photoInfo = new FileInfo(Photo.FileName);

                    string newPhoto = Guid.NewGuid().ToString() + photoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UsersPhoto/" + newPhoto);
                    users.Photo = "/Uploads/UsersPhoto/" + newPhoto;
                   
                }
                users.NameSurname = user.NameSurname;
                users.UserName = user.UserName;
                users.Password = user.Password;
                users.Email = user.Email;
                users.Country = user.Country;
                db.SaveChanges();
                Session["username"] = user.UserName;
                return RedirectToAction("Index", "Home", new { id = users.UserID });
            }
            
            return View();
        }

       
    }
}