using Microsoft.AspNetCore.Mvc;
using RestSharp;
using CanYouC_.Models;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanYouC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly RestClient _client;
        public ScheduleController()
        {
            _client = new RestClient("https://localhost:7203");
        }

        // GET: api/<ScheduleController>
        [HttpGet(Name = "GetRawSchedules")]
        public IEnumerable<Raw> Get()
        {
            List<Raw> rawSchedules = new();
            // Perform GET request to /schedules using RestSharp
            var request = new RestRequest("schedules");
            RestResponse response = _client.ExecuteGet(request);

            // Serialize the response to a list of Raw objects
            if (response.IsSuccessful)
            {
                rawSchedules = JsonConvert.DeserializeObject<List<Raw>>(response.Content);
            }

            // Return the list of Raw objects
            return rawSchedules;
        }
    }
}
