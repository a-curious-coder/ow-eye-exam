using CanYouC_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace CanYouC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeLeaseController : ControllerBase
    {
        private readonly RestClient _client;
        public NoticeLeaseController()
        {
            _client = new RestClient("https://localhost:7203");
        }

        // GET: api/<NoticeLeaseController>
        [HttpGet(Name = "GetNoticeLeaseSchedules")]
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