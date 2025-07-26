
using FluentValidation;

namespace Application.Requests;

public class SearchFlightsRequest
{
	public string Origin { get; set; } = string.Empty;
	public string Destination { get; set; } = string.Empty;
}

public class SearchFlightsRequestValidator : AbstractValidator<SearchFlightsRequest>
{
    public SearchFlightsRequestValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origem é obrigatória.")
            .Length(3).WithMessage("Origem deve ter 3 caracteres.");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destino é obrigatório.")
            .Length(3).WithMessage("Destino deve ter 3 caracteres.");
    }
}