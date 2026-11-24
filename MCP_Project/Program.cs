using FOI_MCP_Server.Tools;
using MCP_Project.Middleware;
using MCP_Project.Services;
using MCP_Project.Tools;
using MCP_Project.Data;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Persist audit and operational data in SQL so tool usage can be traced.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// MCP is exposed over stateless HTTP because each tool invocation is independent
// and does not require server-side session state.
builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

// Both spreadsheet and database services are registered because FOI workflows
// query across two data sources: a contracts workbook and SQL datasets.
builder.Services.AddSingleton<SpreadsheetService>();
builder.Services.AddSingleton<SpreadsheetDatabaseService>();
builder.Services.AddSingleton<IDataSourceRegistry, DataSourceRegistry>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddSingleton<DatabaseService>();


var app = builder.Build();

// Authentication middleware must run before MCP endpoint mapping so every tool
// call is authenticated using the configured API key.
app.UseMiddleware<ApiKeyMiddleware>();

app.MapGet("/", () => "FOI MCP Server Running");

app.MapMcp("/mcp");

app.Run();
