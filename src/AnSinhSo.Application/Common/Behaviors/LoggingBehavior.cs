using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AnSinhSo.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Starting Request: {RequestName}", requestName);

        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();

        if (response.IsSuccess)
        {
            _logger.LogInformation("Completed Request: {RequestName} successfully in {ElapsedMilliseconds} ms", requestName, timer.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogWarning("Completed Request: {RequestName} with Failure in {ElapsedMilliseconds} ms. Error: {ErrorCode} - {ErrorMessage}", 
                requestName, timer.ElapsedMilliseconds, response.Error.Code, response.Error.Message);
        }

        return response;
    }
}
