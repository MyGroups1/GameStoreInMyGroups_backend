using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects.ForAuth;
using Shared.DataTransferObjects.ForShow;

namespace GameStore;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();

            CreateMap<User,UserDto>();
            // CreateMap<User, UserForUpdateMe>();
            //
            //
            // CreateMap<Post, PostDto>()
            //     .ForMember(pd => pd.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
            //     .ForMember(pd=>pd.FirstName,p=>p.MapFrom(x=>x.User.FirstName))
            //     .ForMember(pd=>pd.LastName,p=>p.MapFrom(x=>x.User.LastName))
            //     .ReverseMap();
            // CreateMap<PostForCreationDto, Post>();
            //    
            // CreateMap<PostForUpdateDto, Post>();
            //
            // CreateMap<Category, CategoryDto>().ReverseMap();
            // CreateMap<CategoryForCreationDto, Category>();
            // CreateMap<CategoryForUpdateDto, Category>();
            //
            // CreateMap<Comment, CommentDto>();



        }
        
       
    }
