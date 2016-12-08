using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Infrastructure;
using LibraryApp.Data.Models;
using System.Data.Linq;

namespace LibraryApp.Services
{
   public  class SQLRepository :IRepository
    {
        private Table<User> users;
        private Table<Book> books;
        private Table<Author> authors;
        private Table<History> history;
        private Table<AuthorsBook> aBooks;
        private Table<BooksAndUsers> uBooks;

        private DataContext db;
        public SQLRepository(string connectionString)
        {
            db = new DataContext(connectionString);
            users = db.GetTable<User>();
            books = db.GetTable<Book>();
            authors = db.GetTable<Author>();
            history = db.GetTable<History>();
            aBooks = db.GetTable<AuthorsBook>();
            uBooks = db.GetTable<BooksAndUsers>();

        }

        public IQueryable<User> Users
        {
            get
            {
                return users;
            }
        }

        public IQueryable<Book> Books
        {
            get
            {
                List<AuthorsBook> aut = new List<AuthorsBook>();
                aut = aBooks.ToList();
               // List<Author> auts = new List<Author>();
                foreach (var item in books)
                {
                    foreach(var a in aut)
                    {
                        // auts.Add(authors.FirstOrDefault(x => x.ID == a.AuthorID && item.ID == a.BookID));
                        int temp=0;
                        if (a.BookID == item.ID)
                            temp = a.AuthorID;
                        if(temp != 0)
                        {
                            var writer = authors.FirstOrDefault(x => x.ID == temp);
                            if (writer != null)
                                item.Authors.Add(writer);
                        } 
                    }
                }
                return books;
            }
                
            
        }

        public IQueryable<Author> Authors
        {
            get
            {
                return authors;
            }
        }

        public IQueryable<History> History
        {
            get
            {
                return history;
            }
        }

        public IEnumerable<Book> GetBookForUser(int id)
        {
            var bTakenByUser = uBooks.Where(x => x.UserID == id);
            Book book = new Book();
            List<Book> list = new List<Book>();
           foreach(var item in bTakenByUser)
            {
                book = books.FirstOrDefault(x=>x.ID==item.BookID);
                list.Add(book);
            }

            
            return list;
        }

        #region CRUD Operations

        public bool CreateBook(Book book)
        {
            var newBook = books.FirstOrDefault(x=>x.ID == book.ID);
            if (newBook != null)
            {
                return false;
            }
            else
            {
                newBook = new Book();
                newBook.Name = book.Name;
                newBook.Quantity = book.Quantity;
                books.InsertOnSubmit(newBook);
                books.Context.SubmitChanges();

                foreach (var author in book.Authors)
                {
                    var updatedBook = books.OrderByDescending(x=>x.ID).FirstOrDefault();
                    
                    var AuthorsBooks = db.GetTable<AuthorsBook>().Where(x=>x.BookID == updatedBook.ID).FirstOrDefault(x => x.AuthorID == author.ID);
                    
                    if(AuthorsBooks == null)
                    {
                        AuthorsBooks = new AuthorsBook();
                        AuthorsBooks.BookID = updatedBook.ID;
                        AuthorsBooks.AuthorID = author.ID;
                        db.GetTable<AuthorsBook>().InsertOnSubmit(AuthorsBooks);
                    }
                }
                db.SubmitChanges();
                return true;
                
            } 

        }

        public bool UpdateBook(Book book)
        {
            var newBook = Books.FirstOrDefault(x => x.ID == book.ID);
            if (newBook != null)
            {
                newBook.Name = book.Name;
                newBook.Quantity = book.Quantity;
              
                foreach(var item in aBooks)
                {
                    foreach(var boo in newBook.Authors)
                    if (item.AuthorID == boo.ID  && item.BookID== book.ID)
                        {
                            aBooks.Context.GetTable<AuthorsBook>().DeleteOnSubmit(item);
                        }
                }
                db.SubmitChanges();
                newBook.Authors.Clear();
                foreach(var item in book.Authors)
                {
                    newBook.Authors.Add(item);
                }
                foreach (var bookItem in newBook.Authors)
                {
                    AuthorsBook _Book = new AuthorsBook();
                    _Book.AuthorID = bookItem.ID;
                    _Book.BookID = book.ID;
                    aBooks.Context.GetTable<AuthorsBook>().InsertOnSubmit(_Book);
                }
                db.SubmitChanges();

                return true;

            }
            else return false;

        }

        public bool DeleteBook(int id)
        {
            var book = books.FirstOrDefault(x => x.ID == id);
            var hist = history.Where(x => x.BookID == book.ID);
            if (book != null)
            {
                foreach(var item in hist)
                {
                    history.Context.GetTable<History>().DeleteOnSubmit(item);
                }
                books.Context.GetTable<Book>().DeleteOnSubmit(book);
             
                db.SubmitChanges();
                return true;
            }
            else return false;

        }

        public void AddAuthorToBook(int AuthorId, int BookID)
        {
            var author = authors.FirstOrDefault(x=>x.ID == AuthorId);
            var book = books.FirstOrDefault(x => x.ID == BookID);
            var _authorsBook = aBooks.Where(x => x.AuthorID == author.ID && x.BookID == book.ID);
            if(author != null && book != null)
            {
                if (_authorsBook == null)
                {
                    AuthorsBook temp = new AuthorsBook();
                    temp.BookID = book.ID;
                    temp.AuthorID = author.ID;
                    aBooks.Context.GetTable<AuthorsBook>().InsertOnSubmit(temp);
                 }
               
            }

        }
        public bool RegisterUser(User user)
        {
            
            var _user = users.FirstOrDefault(x => x.ID == user.ID);
            if(_user != null)
            {
                return false;
            }
            else
            {
                var email = users.FirstOrDefault(x => x.Email == user.Email);
                if (email== null)
                {


                    users.InsertOnSubmit(user);
                    db.SubmitChanges();
                    return true;
                }
                else return false;
            }

        }
        public bool TakeBook(int id, int userID)
        {
            var book = books.FirstOrDefault(x => x.ID == id);
            if (book != null && book.Quantity != 0)
            {
                var hist = new History();
                hist.UserID = userID;
                hist.DateTime = DateTime.Today;
                hist.BookID = book.ID;
                book.Quantity--;
                var booksForUsers = new BooksAndUsers();
                booksForUsers.BookID = book.ID;
                booksForUsers.UserID = userID;
                db.GetTable<BooksAndUsers>().InsertOnSubmit(booksForUsers);

                db.GetTable<History>().InsertOnSubmit(hist);
                db.SubmitChanges();
                return true;
            }
            else return false;
        }
        public bool ReturnBook(int id, int userID)
        {
            var book = books.FirstOrDefault(x => x.ID == id);
            if (book != null)
            {
                book.Quantity++;
                var booksForUsers = new BooksAndUsers();
                booksForUsers.BookID = book.ID;
                booksForUsers.UserID = userID;
                db.GetTable<BooksAndUsers>().DeleteOnSubmit(booksForUsers);
                db.SubmitChanges();
                return true;
            }
            else return false;
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool CreateAuthor(Author author)
        {
            var aut = authors.FirstOrDefault(x => x.ID == author.ID);
            if (aut == null)
            {
                authors.InsertOnSubmit(author);
                db.SubmitChanges();
                return true;
            }
            else return false;

        }

        public bool UpdateAuthor(Author author)
        {
            var aut = authors.FirstOrDefault(x => x.ID == author.ID);
            if (aut != null)
            {
                aut.Name = author.Name;
                authors.InsertOnSubmit(aut);
                db.SubmitChanges();
                return true;
            }
            else return false;
        }

        public bool DeleteAuthor(int id)
        {
            var aut = authors.FirstOrDefault(x => x.ID == id);
            if (aut != null)
            {
                authors.Context.GetTable<Author>().DeleteOnSubmit(aut);
                db.SubmitChanges();
                return true;
            }
            else return false;
        }
        #endregion
    }
}
