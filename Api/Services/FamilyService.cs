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
            await _container.CreateItemAsync(family, new PartitionKey(family.Id));
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
    }
}
