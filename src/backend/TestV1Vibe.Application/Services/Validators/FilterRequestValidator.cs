using FluentValidation;
using TestV1Vibe.Application.DTOs;
using TestV1Vibe.Domain.Entities;

namespace TestV1Vibe.Application.Services.Validators;

public class FilterRequestValidator : AbstractValidator<FilterRequestEntityDto>
{
    public FilterRequestValidator()
    {
        RuleFor(x => x.Cliente).NotEmpty().WithMessage("O campo 'Cliente' é obrigatório.");
        RuleFor(x => x.Bairro).NotEmpty().WithMessage("O campo 'Bairro' é obrigatório.");
        RuleFor(x => x.Referencia).NotEmpty().WithMessage("O campo 'Referência' é obrigatório").MinimumLength(3).WithMessage("A referência deve ter no mínimo 3 caracteres.");
        RuleFor(x => x.RuaCruzamento).NotEmpty().WithMessage("O campo 'RuaCruzamento' é obrigatório").MinimumLength(3).WithMessage("Rua ou cruzamento deve ter no mínimo 3 caracteres.");
    }
}
