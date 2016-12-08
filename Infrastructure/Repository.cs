using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Data.Models;

namespace LibraryApp.Infrastructure
{
    public interface IRepository
    {
        IQueryable<User> Users { get; }
        IQueryable<Book> Books { get; }
        IQueryable<Author> Authors { get; }
        IQueryable<History> History { get; }
        IEnumerable<Book> GetBookForUser(int userid);

        bool CreateBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
        void AddAuthorToBook(int author, int book);

        bool RegisterUser(User user);
        bool UpdateUser(User user);
        bool TakeBook(int id, int userid);

        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(int id);


        
    }
}
