using AgendaApp.Server.Models;
using AutoMapper;

namespace AgendaApp.Server.DTOs
{
    public class ContatoProfile : Profile
    {
        public ContatoProfile()
        {
            CreateMap<Contato, ContatoDTO>().ReverseMap();
        }

    }
}
