using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Infrastructure;
using LibraryApp.ViewModels;
using LibraryApp.Data.Models;

namespace LibraryApp.Services
{
  public  class UserManager
    {
        private IRepository repo;
        public UserManager(IRepository repository)
        {
            repo = repository;
        }
        public IEnumerable<UserViewModel> Users
        {
            get
            {
                AutoMapper.Mapper.Initialize(cfg => {
                    cfg.CreateMap<User, UserViewModel>().ReverseMap();
                });
                IEnumerable<UserViewModel> model = AutoMapper.Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(repo.Users.ToList());
                return model;
            }
        }

        public bool Register(RegisterModel model)
        {
            AutoMapper.Mapper.Initialize(cfg =>
                cfg.CreateMap<User, RegisterModel>().ReverseMap());
            User user = AutoMapper.Mapper.Map<RegisterModel, User>(model);
            if (repo.RegisterUser(user))
            {
                return true;
            }
            else return false;
        }
        public bool IsRegistered(string email)
        {
            var user = repo.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                return true;
            }
            else return false;
        }
        public bool Take(string username, int bookID)
        {
            
            var user = repo.Users.FirstOrDefault(x=>x.Email==username);
            if (repo.TakeBook(bookID, user.ID))
            {
                return true;
            }
            else return false;
        }
        
    }
}
