using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LibraryApp.ViewModels
{
    public class BookViewModel
    {
        [HiddenInput]
        public int ID { set; get; }
        public string Name { set; get; }
        [Range(1,1000,ErrorMessage = "Книги нет в наличии")]
        public int Quantity { set; get; }

        public List<AuthorViewModel> Author { set; get; }
        public int Aut { set; get;}
        [Range(0.5,Int64.MaxValue,ErrorMessage ="Необходимо выбрать авторов")]
        
        public IEnumerable<int> Auts { set; get; }
       public  IEnumerable<SelectListItem> AuthorItems {
            get { return new SelectList(Author, "Id", "Name"); }
        }
        public BookViewModel()
        {
            Author = new List<AuthorViewModel>();
            
            
        }

    }
    public class AuthorViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
    }
    public class BookInfoModel
    {
        public string Book { set; get; }
        
       public List<Record> HistoryRecords { set; get; }
        public BookInfoModel()
        {
            HistoryRecords = new List<Record>();
        }

       
    }
    public class Record
    {
        public DateTime Date { set; get; }
        public string User { set; get; }
    }



}
