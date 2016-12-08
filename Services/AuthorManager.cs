using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Infrastructure;
using LibraryApp.ViewModels;
using LibraryApp.Data.Models;

namespace LibraryApp.Services
{
    public class AuthorManager
    {
        private IRepository repo;
        public AuthorManager(IRepository repository)
        {
            repo = repository;
        }
        public IEnumerable<AuthorViewModel> Authors {
            get
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Author, AuthorViewModel>().ReverseMap();
                    cfg.CreateMap<Book, BookViewModel>()
                    .ForMember(x => x.Author, x => x.MapFrom(m => m.Authors)).ReverseMap();
                });
                IEnumerable<AuthorViewModel> model = AutoMapper.Mapper.Map<IEnumerable<Author>,IEnumerable<AuthorViewModel>>(repo.Authors);
                return model;
            }
        }

        public string AddAuthor(AuthorViewModel model)
        {
            var _author = AutoMapper.Mapper.Map<Author>(model);
            if (repo.CreateAuthor(_author))
            {
                return String.Format("Автор {0} успешно добавлен", _author.Name);
            }
            else
            {
                return String.Format("Ошибка!");
            }
        }
    }
}
