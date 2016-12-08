using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Data.Models
{
    [Table(Name = "BookHistory")]
    public class History
    {
        [Column(Name = "HistoryID", IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID { set; get; }
        [Column]
        public int UserID { set; get; }

        [Column(Name = "Datetime")]
        public DateTime DateTime { set; get; }

        [Column(Name = "BookID")]
        public int BookID { set; get; }
    }
}
