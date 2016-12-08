using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace LibraryApp.Data.Models
{
    [Table(Name = "Author")]
   public class Author
    {
        [Column(Name = "AuthorID", IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { set; get; }

        [Column]
        public string Name { set; get; }
    }
}
