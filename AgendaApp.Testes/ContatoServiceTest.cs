using AgendaApp.Server.DTOs;
using AgendaApp.Server.Models;
using AgendaApp.Server.Repositories;
using AgendaApp.Server.Services;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace AgendaApp.Testes
{
    public class ContatoServiceTest
    {
        private readonly Mock<IContatoRepository> _repoMocks;
        private readonly IMapper _mapper;
        private readonly IValidator<ContatoDTO> _validator;
        private readonly IRabbitMQPublisher _publisher;
        private readonly ContatoService _contatoService;

        public ContatoServiceTest()
        {
            _repoMocks = new Mock<IContatoRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContatoProfile>());
            _mapper = config.CreateMapper();
            _validator = new ContatoValidator();
            _publisher = Mock.Of<IRabbitMQPublisher>();

            _contatoService = new ContatoService(_repoMocks.Object, _mapper, _validator, _publisher);
        }

        [Fact]
        //
        // Summary:
        //     Deve retornar o DTO quando houver sucesso
        //
        public async Task CreateContatoTest_1()
        {
            ContatoDTO mockDTO = new() { Nome = "Furina Testes", Email = "furinarconte@gmail.com", Telefone = "81400289922" };

            _repoMocks.Setup(r => r.AddAsync(It.IsAny<Contato>())).
                                    Callback<Contato>(c => c.Id = 123).
                                    Returns(Task.FromResult(mockDTO));
            _repoMocks.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _contatoService.AddContatoAsync(mockDTO);

            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Nome.Should().Be("Furina Testes");
            result.Email.Should().Be("furinarconte@gmail.com");
            result.Telefone.Should().Be("81400289922");

            _repoMocks.Verify(r => r.AddAsync(It.Is<Contato>(c => c.Nome == "Furina Testes")), Times.Once);
            _repoMocks.Verify(r => r.SaveChangesAsync(), Times.Once);

        }

        [Fact]
        //
        // Summary:
        //     Testar tratativa de field vázio
        //
        public async Task CreateContatoTest_2()
        {
            ContatoDTO mockDTO = new() { Nome = "", Email = "xilonen@gmail.com", Telefone = "11111111" };

            await Assert.ThrowsAsync<ValidationException>(() => _contatoService.AddContatoAsync(mockDTO));

            _repoMocks.Verify(r => r.AddAsync(It.IsAny<Contato>()), Times.Never);
        }
    }
}
