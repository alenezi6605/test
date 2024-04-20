
using Paging;
using System.Text.Json.Serialization;

namespace TestsService.Domain.Common
{
    public class EntityResponseModel
    {
        public object Data { get; set; }

        //ignore pagination property in api response if it's null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Pagination Pagination { get; set; }


        //ignore pagination property in api response if it's null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Included { get; set; }

        //ignore pagination property in api response if it's null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Paging { get; set; }
    }
}
