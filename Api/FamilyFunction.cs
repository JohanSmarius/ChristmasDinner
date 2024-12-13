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

        [Function("ChooseFamily")]
        public async Task<IActionResult> ChooseFamily([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var requestData = await req.ReadFromJsonAsync<ChooseFamilyRequest>();

            var families = await _familyService.GetFamiliesAsync();
            var chosenFamily = families.FirstOrDefault(f => f.id == requestData.FamilyId);

            if (chosenFamily != null)
            {
                chosenFamily.Guests.Add(new Guest { Name = requestData.SinglePerson.Name, Age = requestData.SinglePerson.Age });
                chosenFamily.NumberOfSeats -= 1;
                await _familyService.AddFamilyAsync(chosenFamily);
                return new OkObjectResult(chosenFamily);
            }

            return new NotFoundResult();
        }
    }

    public class ChooseFamilyRequest
    {
        public string FamilyId { get; set; }
        public SinglePerson SinglePerson { get; set; }
    }
}
