using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;

namespace Api
{
    public class SinglePersonFunction
    {
        private readonly ILogger<SinglePersonFunction> _logger;
        private readonly SinglePersonService _singlePersonService;

        public SinglePersonFunction(ILogger<SinglePersonFunction> logger, SinglePersonService singlePersonService)
        {
            _logger = logger;
            _singlePersonService = singlePersonService;
        }

        [Function("GetSinglePersons")]
        public async Task<IActionResult> GetSinglePersons([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var singlePersons = await _singlePersonService.GetSinglePersonsAsync();
            return new OkObjectResult(singlePersons);
        }

        [Function("AddSinglePerson")]
        public async Task<IActionResult> AddSinglePerson([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var singlePerson = await req.ReadFromJsonAsync<SinglePerson>();
            await _singlePersonService.AddSinglePersonAsync(singlePerson);
            return new OkResult();
        }
    }
}
