using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.ViewModels;
using LibraryApp.Data.Models;

namespace Services
{
    public static class AutoMapperConfig
    {
        
        public static void RegisterMappings()
        {
            var config  = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>().ReverseMap();
                cfg.CreateMap<User, UserViewModel>().ReverseMap();
                cfg.CreateMap<Book, BookViewModel>()
                .ForMember(x => x.Author, x => x.MapFrom(m => m.Authors)).ReverseMap();
            }
             );
            

        }

    }
    
}
