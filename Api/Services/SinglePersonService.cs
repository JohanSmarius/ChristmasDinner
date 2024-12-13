using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Api.Models;

namespace Api.Services
{
    public class SinglePersonService
    {
        private readonly Container _container;

        public SinglePersonService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddSinglePersonAsync(SinglePerson singlePerson)
        {
            if (string.IsNullOrEmpty(singlePerson.id))
            {
                singlePerson.id = System.Guid.NewGuid().ToString();
            }

            await _container.CreateItemAsync(singlePerson, new PartitionKey(singlePerson.id));
        }

        public async Task<IEnumerable<SinglePerson>> GetSinglePersonsAsync()
        {
            var query = _container.GetItemQueryIterator<SinglePerson>();
            var results = new List<SinglePerson>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }
    }
}
