using AccessData.Entities;
using AutoMapper;
using Common.Classes.BussinesLogic;
using System.Collections.Generic;

namespace App.Config.Dependencies
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Move, MoveDTO>().ReverseMap();
            CreateMap<MoveDetail, MoveDetailDTO>().ReverseMap();
        }
    }
}