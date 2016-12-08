using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LibraryApp.Data.Models
{
    [Table(Name = "BooksForUser")]
    public class BooksAndUsers
    {
        [Column(Name = "ID", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID { set; get; }
        [Column(Name = "UserID")]
        public int UserID { set; get; }
        [Column(Name ="BookID")]
        public int BookID { set; get; }
    }
}
