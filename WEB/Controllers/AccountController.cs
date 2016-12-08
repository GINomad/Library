using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Infrastructure;
using LibraryApp.Services;
using LibraryApp.ViewModels;
using System.Web.Security;

namespace WEB.Controllers
{
    public class AccountController : Controller
    {
        UserManager _uManager;
        public AccountController(IRepository repo)
        {
            _uManager = new UserManager(repo);
        }



        // GET: Account
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                if(!_uManager.IsRegistered(model.Email))
                {
                    _uManager.Register(model);
                    var user = _uManager.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                    if(user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Пользователем с таким Email уже есть");
                }  
            }
            return View(model);
        }
        public ActionResult Login()
        {
            
            return View(new LoginModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            
            if(ModelState.IsValid)
            {
                if(_uManager.IsRegistered(model.Email))
                {
                    var user = _uManager.Users.FirstOrDefault(x=>x.Email==model.Email);
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("","Пользователя с таким логином не существует");
                }
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}