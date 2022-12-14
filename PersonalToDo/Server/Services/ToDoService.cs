using Microsoft.Azure.Cosmos;

namespace PersonalToDo.Server.Services
{
    public class ToDoService
    {
        private readonly CosmosDbService cosmosDbService;
        private readonly Container container;

        public ToDoService(CosmosDbService cosmosDbService)
        {
            // 작성한 CosmosDbService 선언
            this.cosmosDbService = cosmosDbService;
            // 컨테이너 가져오기
            container = cosmosDbService.GetContainer();
        }

        // CREATE
        public async Task CreateToDo(ToDoItem toDoitem)
        {
            // id는 GUID로
            toDoitem.Id = Guid.NewGuid().ToString();
            await container.AddItem<ToDoItem>(toDoitem);
        }

        // READ
        public async Task<List<ToDoItem>> GetToDos()
        {
            return await container.GetItemLinqQueryable<ToDoItem>().GetListFromFeedIteratorAsync();
        }

        // DELETE
        public async Task RemoveToDo(string Id)
        {
            await container.RemoveItem<ToDoItem>(Id);
        }

        // UPDATE
        public async Task EditToDo(ToDoItem toDoItem)
        {
            await container.EditItem<ToDoItem>(toDoItem);
        }
    }
}
