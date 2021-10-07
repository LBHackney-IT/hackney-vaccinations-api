using System.Linq;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;

namespace LbhNotificationsApi.V1.Extensions
{
    public static class ProblemDetailsOptionsExtensions
    {
        public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
            });
    }
}
