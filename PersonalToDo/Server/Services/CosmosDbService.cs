using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace PersonalToDo.Server.Services
{
    // 연결할 CosmosDB의 정보를 담은 객체
    public class CosmosDbServiceOptions
    {
        public string? Account { get; set; }
        public string? Key { get; set; }
        public string? DatabaseName { get; set; }
        public string? ContainerName { get; set; }
    }

    // 이 클래스에서 CosmosDB 연결을 위한 로직 작성
    public class CosmosDbService
    {
        // 로직에 쓰일 변수 생성
        private readonly CosmosClient client;
        private readonly string databaseName;
        private readonly string containerName;

        public CosmosDbService(IOptions<CosmosDbServiceOptions> options)
        {
            client = new CosmosClient(options.Value.Account, options.Value.Key);
            databaseName = options.Value.DatabaseName;
            containerName = options.Value.ContainerName;
        }

        public Container GetContainer()
        {
            var container = client.GetContainer(databaseName, containerName);
            return container;
        }
    }

    public static class CosmosDbServiceExtensions
    {
        // GET
        public static async Task<T> GetItem<T>(this Container container, string id) where T : CosmosModelBase
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            try
            {
                var respoonse = await container.ReadItemAsync<T>(id, new PartitionKey(id));
                return respoonse.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // CREATE
        public static async Task AddItem<T>(this Container container, T model) where T : CosmosModelBase
        {
            await container.CreateItemAsync(model, new PartitionKey(model.Id));
        }

        // UPDATE
        public static async Task EditItem<T>(this Container container, T model) where T : CosmosModelBase
        {
            await container.UpsertItemAsync(model, new PartitionKey(model.Id));
        }

        // DELETE
        public static async Task RemoveItem<T>(this Container container, string id) where T : CosmosModelBase
        {
            await container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public static IQueryable<T> OfCosmosItemType<T>(this IQueryable<T> query) where T : CosmosModelBase
        {
            var typeName = query.ElementType.Name;
            return query.Where(x => x.ClassType == typeName);
        }

        public static async Task<List<T>> GetListFromFeedIteratorAsync<T>(this IQueryable<T> query) where T : CosmosModelBase
        {
            var result = new List<T>();
            using (var iterator = query.ToFeedIterator())
            {
                while (iterator.HasMoreResults)
                {
                    foreach (var item in await iterator.ReadNextAsync())
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }
    }
}
