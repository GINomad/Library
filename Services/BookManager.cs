using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Infrastructure;
using LibraryApp.Data.Models;
using LibraryApp.ViewModels;


namespace LibraryApp.Services
{
    public class BookManager
    {
        private IRepository repo;
        private UserManager manager;
        public BookManager(IRepository repos)
        {
            repo = repos;
            manager = new UserManager(repos);
        }

        public IEnumerable<BookViewModel> Books { get
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Author, AuthorViewModel>().ReverseMap();
                    
                    cfg.CreateMap<Book, BookViewModel>()
                    .ForMember(x =>x.Author, x => x.MapFrom(m =>m.Authors)).ReverseMap();
                });
                IEnumerable<BookViewModel> model = AutoMapper.Mapper.Map<IEnumerable<Book>,IEnumerable<BookViewModel>>(repo.Books.ToList());
                return model;
            }
        }
        public IEnumerable<BookViewModel> BookByUser(string searchName)
        {
           
            var user = manager.Users.Where(x => x.Name.ToUpper() == searchName.ToUpper()).FirstOrDefault();
            if(user == null)
            {
                var us = manager.Users.Where(x => x.Email.ToUpper() == searchName.ToUpper()).FirstOrDefault();
                if(us== null)
                {
                    return null;
                }
                else
                {
                    AutoMapper.Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<Author, AuthorViewModel>().ReverseMap();

                        cfg.CreateMap<Book, BookViewModel>()
                        .ForMember(x => x.Author, x => x.MapFrom(m => m.Authors)).ReverseMap();
                    });
                    IEnumerable<BookViewModel> model = AutoMapper.Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(repo.GetBookForUser(us.ID));
                    return model;
                }
            }
            else
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Author, AuthorViewModel>().ReverseMap();

                    cfg.CreateMap<Book, BookViewModel>()
                    .ForMember(x => x.Author, x => x.MapFrom(m => m.Authors)).ReverseMap();
                });
                IEnumerable<BookViewModel> model = AutoMapper.Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(repo.GetBookForUser(user.ID));
                return model;
            }
           
           
            
            
        }
        public IEnumerable<HistoryViewModel> History
        {
            get
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<History, HistoryViewModel>().ReverseMap();
                }
                    );
                IEnumerable<HistoryViewModel> model = AutoMapper.Mapper.Map<IEnumerable<History>, IEnumerable<HistoryViewModel>>(repo.History.ToList());
                return model;
            }
        }

        public void AddAuthorToBook(int authorID, int bookID)
        {
            repo.AddAuthorToBook(authorID, bookID);
        }
        public BookInfoModel GetBookInfo(BookViewModel model)
        {
            BookInfoModel info = new BookInfoModel();

            var hist = History.Where(x => x.BookID == model.ID).OrderBy(x=>x.ID);
            info.Book = model.Name;
            foreach(var item in hist)
            {
                info.HistoryRecords.Add(new Record
                {
                    Date = item.DateTime,
                    User = manager.
                    Users.
                    Where(x => x.ID == item.UserID).FirstOrDefault().Name
                });
            }
            return info;

            
        }

        public string AddBook(BookViewModel book)
        {
            var _book = new Book();
            _book.Authors = new List<Author>();
            _book = AutoMapper.Mapper.Map<BookViewModel,Book>(book);
            _book.Authors = new List<Author>();
            var q = AutoMapper.Mapper.Map<IEnumerable<AuthorViewModel>,IEnumerable<Author>>(book.Author.ToList());
            _book.Authors.InsertRange(0, q);
            if (repo.CreateBook(_book))
            {
                return String.Format("Книга {0} успешно добавлена", _book.Name);
            }
            else
            {
                return String.Format("Ошибка!");
            }

        }
        public string UpdateBook(BookViewModel book)
        {
            var _book = new Book();
            _book.Authors = new List<Author>();
            _book = AutoMapper.Mapper.Map<BookViewModel, Book>(book);
            _book.Authors = new List<Author>();
            var q = AutoMapper.Mapper.Map<IEnumerable<AuthorViewModel>, IEnumerable<Author>>(book.Author.ToList());
            _book.Authors.InsertRange(0, q);
            if (repo.UpdateBook(_book))
            {
                return String.Format("Книга {0} успешно обновлена", book.Name);
            }
            else
            {
                return String.Format("Ошибка!");
            }
        }
        public void RemoveBook(int id)
        {
            repo.DeleteBook(id);
        }


    }

    
}
