using FOI_MCP_Server.Tools;
using MCP_Project.Middleware;
using MCP_Project.Services;
using MCP_Project.Tools;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        // Stateless mode is recommended for servers that don't need
        // server-to-client requests like sampling or elicitation.
        // See the Sessions documentation for details.
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

builder.Services.AddSingleton<SpreadsheetService>();
builder.Services.AddSingleton<SpreadsheetDatabaseService>();

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<GetFoiRequest>()
    .WithTools<DatabaseTool>()
    .WithTools<ContractTools>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGet("/", () => "FOI MCP Server Running");

app.MapMcp("/mcp");

app.Run();
