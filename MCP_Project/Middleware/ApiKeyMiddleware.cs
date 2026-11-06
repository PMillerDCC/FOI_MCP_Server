namespace MCP_Project.Middleware
{
    public class ApiKeyMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        private const string HeaderName = "X-API-Key";

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IConfiguration configuration)
        {
            Console.WriteLine($"Request received: {context.Request.Path}");
            Console.WriteLine("API Key Middleware Executed");

            _logger.LogInformation(
                "Request received. Method: {Method}, Path: {Path}, IP: {IP}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress);

            _logger.LogInformation(
                "Headers: {Headers}",
                string.Join(", ", context.Request.Headers.Keys));

            if (!context.Request.Headers.TryGetValue(
                    HeaderName,
                    out var suppliedKey))
            {
                context.Response.StatusCode = 401;

                _logger.LogWarning(
                    "Authentication failed. Invalid API key. Path: {Path}, RemoteIP: {IP}",
                    context.Request.Path,
                    context.Connection.RemoteIpAddress);

                await context.Response.WriteAsync(
                    "Invalid API Key");

                return;
            }

            var configuredKey =
                configuration["Authentication:ApiKey"];

            if (!string.Equals(
                suppliedKey,
                configuredKey,
                StringComparison.Ordinal))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync(
                    "Invalid API Key");

                return;
            }

            _logger.LogInformation(
                "Authentication successful. Path: {Path}, RemoteIP: {IP}",  
                context.Request.Path, 
                context.Connection.RemoteIpAddress);

            await _next(context);

        }
    }
}
