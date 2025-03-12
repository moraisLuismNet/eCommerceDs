using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;

namespace eCommerceDs.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<Group, GroupInsertDTO>().ReverseMap();
            CreateMap<Group, GroupUpdateDTO>().ReverseMap();
            CreateMap<Group, GroupItemDTO>().ReverseMap();
            CreateMap<Group, GroupRecordsDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreInsertDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreUpdateDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreGroupDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreTotalGroupsDTO>().ReverseMap();
            CreateMap<Record, RecordDTO>().ReverseMap();
            CreateMap<Record, RecordInsertDTO>().ReverseMap();
            CreateMap<Record, RecordUpdateDTO>().ReverseMap();
            CreateMap<Record, RecordItemDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserInsertDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();

        }

    }
}
