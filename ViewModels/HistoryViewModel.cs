using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.ViewModels
{
    public class HistoryViewModel
    {
        public int ID { set; get; }
        public int UserID { set; get; }
        public int BookID { set; get; }
        public DateTime DateTime { set; get; }
        
    }
}
