using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;

namespace Api
{
    public class FamilyFunction
    {
        private readonly ILogger<FamilyFunction> _logger;
        private readonly FamilyService _familyService;

        public FamilyFunction(ILogger<FamilyFunction> logger, FamilyService familyService)
        {
            _logger = logger;
            _familyService = familyService;
        }

        [Function("GetFamily")]
        public async Task<IActionResult> GetFamily([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var families = await _familyService.GetFamiliesAsync();
            return new OkObjectResult(families);
        }

        [Function("AddFamily")]
        public async Task<IActionResult> AddFamily([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var family = await req.ReadFromJsonAsync<Family>();
            await _familyService.AddFamilyAsync(family);
            return new OkResult();
        }
    }
}
