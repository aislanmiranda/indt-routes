using FluentValidation;

namespace Application.Requests;

public class CreateRouteRequest
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int Cost { get; set; }
}

public class CreateRouteRequestValidator : AbstractValidator<CreateRouteRequest>
{
    public CreateRouteRequestValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origem é obrigatória.")
            .Length(3).WithMessage("Origem deve ter 3 caracteres.");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destino é obrigatório.")
            .Length(3).WithMessage("Destino deve ter 3 caracteres.");

        RuleFor(x => x.Cost)
            .GreaterThan(0).WithMessage("Custo deve ser maior que zero.");
    }
}