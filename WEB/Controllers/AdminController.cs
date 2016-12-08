using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApp.Services;
using LibraryApp.Data.Models;
using LibraryApp.Infrastructure;
using LibraryApp.ViewModels;
using System.Xml.Linq;

namespace WEB.Controllers
{
    /// <summary>
    /// Предназнчен для управления книгами и авторами
    /// </summary>
    [Authorize]
    public class AdminController : Controller
    {
        private BookManager _bManager;
        private AuthorManager _aManager;
        public AdminController(IRepository repo)
        {
            _bManager = new BookManager(repo);
            _aManager = new AuthorManager(repo);
        }
        public ViewResult CreateBook()
        {
            
            SelectList authors = new SelectList(_aManager.Authors.OrderBy(x=>x.Name),"ID","Name");
            ViewBag.Authors = authors;
            BookViewModel model = new BookViewModel();
            return View(model);
        }
        [HttpPost] 
        public ActionResult CreateBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
               // model.Author = new List<AuthorViewModel>();
                AuthorViewModel mod = new AuthorViewModel();
                mod = _aManager.Authors.FirstOrDefault(x => x.ID == model.Aut);
                model.Author.Add(mod);
                string result = _bManager.AddBook(model);
                ViewData["Result"] = result;
                return RedirectToAction("Index", "Home");
            }
            else return View(model);
        }
        public ViewResult UpdateBook(int id)
        {
            SelectList authors = new SelectList(_aManager.Authors.OrderBy(x => x.ID), "ID", "Name");
            ViewBag.Authors = authors;
            BookViewModel model = _bManager.Books.FirstOrDefault(x=>x.ID == id);

            if (model != null)
            {
                return View(model);
            }
            else return View("Index");
        }
        [HttpPost]
        public ActionResult UpdateBook(BookViewModel model)
        {
            
            
                List<AuthorViewModel> mod = new List<AuthorViewModel>();
                AuthorViewModel temp = new AuthorViewModel();
                foreach (var item in model.Auts)
                {
                    temp = _aManager.Authors.Where(x => x.ID == item).FirstOrDefault();
                    if (temp != null)
                        mod.Add(temp);

                }
                model.Author.Clear();
                foreach (var item in mod)
                    model.Author.Add(item);
            ModelState["Auts"].Errors.Clear();
            if (ModelState.IsValid)
            {
                _bManager.UpdateBook(model);
                return RedirectToAction("Index","Home");
            }
            else return RedirectToAction("UpdateBook");
        }
        
        public ActionResult DeleteBook(int id)
        {
            var book = _bManager.Books.FirstOrDefault(x => x.ID == id);
            if(book!=null)
            {
                _bManager.RemoveBook(id);
                return RedirectToAction("Index","Home");
            }
            else return RedirectToAction("Index", "Home");
        }
    
        // GET: Admin
        public ActionResult Index()
        {
            return View();
            
        }
        public ViewResult CreateAuthor()
        {
            AuthorViewModel model = new AuthorViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateAuthor(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewData["Result"] = _aManager.AddAuthor(model);
                return RedirectToAction("CreateBook");


            }
            else return View(model);

        }
    }
}