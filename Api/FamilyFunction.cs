using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using System.Linq;

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

        // Create a new function named GetFamiliesByTown with an HttpTrigger attribute and the type of get. The town is passsed as a query parameter.
        // The function should return a list of families that have available seats in the town specified.
        [Function("GetFamiliesByTown")]
        public async Task<IActionResult> GetFamiliesByTown([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var town = req.Query["town"];
            var families = await _familyService.GetAvailableFamiliesAsync(town);
            return new OkObjectResult(families);
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

        [Function("RegisterSinglePerson")]
        public async Task<IActionResult> RegisterSinglePerson([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var singlePerson = await req.ReadFromJsonAsync<SinglePerson>();

            var families = await _familyService.GetFamiliesAsync();
            var matchingFamilies = families.Where(f => f.Town == singlePerson.Town && f.Guests.Count < f.NumberOfSeats).ToList();

            if (matchingFamilies.Any())
            {
                var family = matchingFamilies.First();
                family.Guests.Add(new Guest { Name = singlePerson.Name, Age = singlePerson.Age });
                family.NumberOfSeats -= 1;
                await _familyService.AddFamilyAsync(family);
                return new OkObjectResult(family);
            }

            return new NotFoundResult();
        }
    }
}
