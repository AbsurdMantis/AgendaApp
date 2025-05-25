using FluentValidation;

namespace AgendaApp.Server.DTOs
{
    public class ContatoValidator : AbstractValidator<ContatoDTO>
    {
        public ContatoValidator()
        {

            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.").MaximumLength(150).WithMessage("Nome excede o padrão permitido.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email é obrigatório.").EmailAddress().WithMessage("Email inválido.");
            RuleFor(x => x.Telefone).NotEmpty().WithMessage("Telefone é obrigatório.");
        }
    }
}
