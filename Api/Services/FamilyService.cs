using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Api.Models;

namespace Api.Services
{
    public class FamilyService
    {
        private readonly Container _container;

        public FamilyService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddFamilyAsync(Family family)
        {
            if (string.IsNullOrEmpty(family.id))
            {
                family.id = System.Guid.NewGuid().ToString();
            }

            await _container.UpsertItemAsync(family, new PartitionKey(family.id));
        }

        public async Task<IEnumerable<Family>> GetFamiliesAsync()
        {
            var query = _container.GetItemQueryIterator<Family>();
            var results = new List<Family>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<IEnumerable<Family>> GetAvailableFamiliesAsync(string town)
        {
            var query = _container.GetItemQueryIterator<Family>($"SELECT * FROM c WHERE c.Town = '{town}' AND c.NumberOfSeats > 0");
            var results = new List<Family>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
