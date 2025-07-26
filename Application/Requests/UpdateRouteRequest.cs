using FluentValidation;

namespace Application.Requests;

public class UpdateRouteRequest : CreateRouteRequest
{
    public int Id { get; set; }
}

public class UpdateRouteRequestValidator : AbstractValidator<UpdateRouteRequest>
{
    public UpdateRouteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id deve ser maior que zero.");

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