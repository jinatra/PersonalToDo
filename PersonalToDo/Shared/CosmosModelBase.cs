using Newtonsoft.Json;

namespace PersonalToDo.Shared
{
    public abstract class CosmosModelBase
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        public abstract string ClassType { get; }
    }
}
