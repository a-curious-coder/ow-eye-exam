using Microsoft.AspNetCore.Mvc;
using RestSharp;
using CanYouC_.Models;
using Newtonsoft.Json;
using System.Text;
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
            // TODO: read base URL from config file
            _client = new RestClient("https://localhost:7203");
        }

        // GET: api/<ScheduleController>
        [HttpGet(Name = "GetRawSchedules")]
        public IEnumerable<RawSchedule> Get()
        {
            List<RawSchedule> rawSchedules = new();
            // Perform GET request to /schedules using RestSharp
            var request = new RestRequest("schedules");

            // Add basic authentication header
            // TODO: read username and password from config file
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("testy:mcTestFace")));

            RestResponse response = _client.ExecuteGet(request);

            if (response.IsSuccessful)
            {
                // Deserialize JSON data into our Raw Schedule objects
                rawSchedules = JsonConvert.DeserializeObject<List<RawSchedule>>(response.Content);
            } 
            else
            {
                // TODO: Handle error
                rawSchedules = new List<RawSchedule>();
            }

            // Return the list of Raw objects
            return rawSchedules;
        }
    }
}
