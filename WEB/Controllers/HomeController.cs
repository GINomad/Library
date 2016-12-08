using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Linq;
using LibraryApp.Infrastructure;
using LibraryApp.Services;
using PagedList;
using Postal;

using LibraryApp.ViewModels;
using System.Net.Mail;
using System.Net;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {

        private BookManager _bManager;
        private UserManager _uManager;
        public HomeController(IRepository repo)
        {
            _bManager = new BookManager(repo);
            _uManager = new UserManager(repo);
        }
        public ActionResult Index(int ?page, string searchString, bool? Available, string sortOrder, string currentFilter)
        {
            string search = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "Author desc" : "Author";
            if (Request.HttpMethod == "GET")
            {
                search = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = search;
            ViewBag.Available = Available;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            IEnumerable<BookViewModel> books = _bManager.Books;
            switch (sortOrder)
            {
                case "Name desc":
                    books = books.OrderByDescending(x => x.Name);
                    break;
                case "Author":
                    books = books.OrderBy(x => x.Author.First().Name);
                    break;
                case "Author desc":
                    books = books.OrderByDescending(x => x.Author.First().Name);
                    break;
                default:
                    books = books.OrderBy(x => x.Name);
                    break;
            }
            if (Available.HasValue && Available.Value == true)
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    books = _bManager.BookByUser(searchString);
                    if (books != null)

                        return View(books.Where(x=>x.Quantity > 0).ToPagedList(pageNumber, pageSize));

                    else return View(books.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(books.Where(x=>x.Quantity >0 ).ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                
                if (!String.IsNullOrEmpty(searchString))
                {
                    books = _bManager.BookByUser(searchString);
                    if (books != null)
                        return View(books.ToPagedList(pageNumber, pageSize));
                    else return View(books.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(books.ToPagedList(pageNumber, pageSize));
                }
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Take(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var book = _bManager.Books.FirstOrDefault(x => x.ID == id);
                if (book != null)
                {
                    return View(book);
                }
                else return View("Index");
            }
            else return RedirectToAction("Login", "Account");
        }
        [HttpPost]
       public ActionResult Take(BookViewModel mod)
        {
            if(User.Identity.IsAuthenticated)
            {
                
                if (ModelState.IsValid)
                {
                    var book = _bManager.Books.FirstOrDefault(x => x.ID == mod.ID);
                    if (book != null)
                    {
                        _uManager.Take(User.Identity.Name.ToString(), mod.ID);
                        var smtpClient = new SmtpClient();
                        
                        //smtpClient.EnableSsl = true;
                        var msg = new MailMessage();
                        msg.To.Add(User.Identity.Name);
                        msg.Subject = "Test";
                        msg.Body = "This is just a test email";
                        smtpClient.Send(msg);
                        return RedirectToAction("Index");

                    }
                }
                else return View("Index");
                    
                
            }
            return RedirectToAction("Login", "Index");
        }

        public ViewResult Details(int id)
        {
            BookViewModel book = _bManager.Books.FirstOrDefault(x=>x.ID == id);
            if (book != null)
            {
                BookInfoModel info = _bManager.GetBookInfo(book);
                return View(info);
            }
            else return View("Index");
            
        }


        public ActionResult Contact()
        {
            
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}