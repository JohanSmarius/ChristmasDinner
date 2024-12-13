using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using Api.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton(s =>
{
    var cosmosClient = new CosmosClient("AccountEndpoint=https://festivecalendar.documents.azure.com:443/;AccountKey=MOqWoCM42A7ZqrL7UN5ZydezLnqHEcM4pAnxarufvjmoC4P7K1sMhDGIgHNycXXyfadFGcYzWo9iACDbqUK8Hg==;");
    return new FamilyService(cosmosClient, "ToDoList", "Items");
});

builder.Build().Run();
