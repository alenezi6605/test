using System.Text.Json.Serialization;

namespace TestsService.AppCore.Respositories.Models
{
   
    public class IncludedResponse
    {
        //ignore pagination property in api response if it's null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<DeliveryShiftDto> DeliveryShifts { get; set; }

        //ignore pagination property in api response if it's null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<PortionDto> Portions { get; set; }

    }

    public class DeliveryShiftDto
    {
        public uint DeliveryShiftId { get; set; }

        public string Title { get; set; }

        public string Timing { get; set; }
    }

    public class PortionDto
    {
        public uint Id { get; set; }

        public string Name { get; set; }
    }
}
