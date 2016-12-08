using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
namespace LibraryApp.Data.Models
{
    [Table(Name = "Book")]
    public class Book
    {
        [Column(Name = "BookID", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID { set; get; }
        [Column]
       public  string Name { set; get; }
        [Column]
        public int Quantity { set; get; }
        [Column]
       public  int? History { set; get; }  
        public List<Author> Authors { set; get; }  
        public Book()
        {
            Authors = new List<Author>();
        }
    }
}
