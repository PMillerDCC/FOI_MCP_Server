using FOI_MCP_Server.Tools;
using MCP_Project.Middleware;
using MCP_Project.Services;
using MCP_Project.Tools;
using MCP_Project.Data;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

builder.Services.AddSingleton<SpreadsheetService>();
builder.Services.AddSingleton<SpreadsheetDatabaseService>();
builder.Services.AddSingleton<IDataSourceRegistry, DataSourceRegistry>();
builder.Services.AddScoped<AuditService>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapGet("/", () => "FOI MCP Server Running");

app.MapMcp("/mcp");

app.Run();
