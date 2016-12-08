using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;


namespace LibraryApp.Data.Models
{
    [Table(Name = "User")]
    public class User
    {
        [Column(Name = "UserID", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID { set; get; }
        [Column]
        public string Name { set; get; }
        [Column]
        public string Email { set; get; }
        [Column(Name = "HistoryID")]
        public int? History { set; get; }
       
        
    }
}
