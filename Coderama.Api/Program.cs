using Coderama.Api.DependencyInjection;
using Coderama.Api.Implementations.DocumentRepository;
using Coderama.Api.Implementations.DocumentStorage;
using Coderama.Api.Implementations.TransactionRepository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

// Idempotency layer
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();

builder.Services.AddSingleton<IDocumentRepository, DocumentRepository>();

builder.Services.AddSingleton<IDocumentStorage, FileStorage>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMappers();

builder.Services.AddFusionCache();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options.EnabledTargets = [ScalarTarget.Shell, ScalarTarget.CSharp, ScalarTarget.PowerShell];
        options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.Shell, ScalarClient.Curl);
    });
}

app.MapHealthChecks("/health");

app.UseAuthorization();

app.RegisterEndpoints(logger);

await app.RunAsync();
