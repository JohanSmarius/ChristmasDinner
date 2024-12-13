using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class FamilyFunction
    {
        private readonly ILogger<FamilyFunction> _logger;

        public FamilyFunction(ILogger<FamilyFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetFamily")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

    }
}
