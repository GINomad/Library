using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LibraryApp.Data.Models
{
    [Table(Name ="Author'sBooks")]
  public class AuthorsBook
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { set; get; }
        [Column]
        public int BookID { set; get; }
        [Column]
        public int AuthorID { set; get; }
    }
}
