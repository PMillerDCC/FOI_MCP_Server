namespace MCP_Project.Middleware
{
    /// <summary>
    /// Protects all incoming requests with a shared API key check.
    /// </summary>
    /// <remarks>
    /// This middleware provides a lightweight authentication boundary for the PoC MCP server
    /// where full user identity and role-based access are not yet in scope.
    /// </remarks>
    public class ApiKeyMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        // Single header contract used by clients and test automation.
        private const string HeaderName = "X-API-Key";

        /// <summary>
        /// Creates a middleware instance.
        /// </summary>
        /// <param name="next">Next component in the ASP.NET request pipeline.</param>
        /// <param name="logger">Structured logger for authentication diagnostics.</param>
        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Validates the caller's API key before allowing access to MCP endpoints.
        /// </summary>
        /// <param name="context">Current HTTP context.</param>
        /// <param name="configuration">Configuration source that contains the expected API key.</param>
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
            // Header names are logged for request troubleshooting; values are not logged.

            // Fail fast when no API key is supplied so expensive data operations are never executed.
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

            // The expected key is centrally configured to avoid key values in source code.
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
