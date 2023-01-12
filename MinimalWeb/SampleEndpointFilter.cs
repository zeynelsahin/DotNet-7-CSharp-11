namespace MinimalWeb;

public class SampleEndpointFilter : IEndpointFilter
{
    private readonly ILogger _logger;

    protected SampleEndpointFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SampleEndpointFilter>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        _logger.LogInformation("Request for {RequestPath} received at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());
        var result = await next(context);
        _logger.LogInformation("Request for {RequestPath} handled at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());
        return result;
    }
}

class MyEndpointFilter : SampleEndpointFilter
{
    public MyEndpointFilter(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}