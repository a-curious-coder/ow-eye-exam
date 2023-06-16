using CanYouC_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonException = System.Text.Json.JsonException;

namespace CanYouC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeLeaseController : ControllerBase
    {
        private readonly RestClient _client;
        public NoticeLeaseController()
        {
            // TODO: read base URL from config file
            _client = new RestClient("https://localhost:7203");
        }

        // Add description
        [HttpGet("GetNoticeLeaseSchedules", Name = "GetNoticeLeaseSchedules")]
        public IEnumerable<NoticeLeaseSchedule> GetNoticeLeases()
        {
            List<NoticeLeaseSchedule> noticeLeaseSchedules = new();

            // Perform GET request to /schedules using RestSharp
            var request = new RestRequest("results");
            // Add basic authentication header
            // TODO: read username and password from config file
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("testy:mcTestFace")));

            RestResponse response = _client.ExecuteGet(request);

            // Serialize response to a list of NoticeLeaseSchedule objects
            if (response.IsSuccessful)
            {
                try
                {
                    noticeLeaseSchedules = JsonConvert.DeserializeObject<List<NoticeLeaseSchedule>>(response.Content);
                }
                // Catch any errors that occur during deserialization
                catch (JsonException e)
                {
                    Console.WriteLine(e.Message);
                }
                // Catch any other errors
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return noticeLeaseSchedules;
        }

        // GET: api/<NoticeLeaseController>
        [HttpGet("GetNoticeLeaseSchedulesFromRawScheduleData", Name = "GetNoticeLeaseSchedulesFromRawScheduleData")]
        public IEnumerable<NoticeLeaseSchedule> GetNoticeLeaseFromRaw()
        {
            List<RawSchedule> rawSchedules = new();
            // Create GET request to /schedules using RestSharp
            var request = CreateGet("schedules");

            // Execute GET request
            RestResponse response = _client.ExecuteGet(request);

            // Serialize response to a list of NoticeLeaseSchedule objects
            if (response.IsSuccessful)
            {
                try
                {
                    rawSchedules = JsonConvert.DeserializeObject<List<RawSchedule>>(response.Content);
                }
                // Catch any errors that occur during deserialization
                catch (JsonException e)
                {
                    Console.WriteLine(e.Message);
                }
                // Catch any other errors
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            // Order rawSchedules by EntryNumber
            rawSchedules.Sort((x, y) => x.EntryNumber.CompareTo(y.EntryNumber));

            // Create list of NoticeLease objects
            List<NoticeLeaseSchedule> noticeLeaseSchedules = new();

            for(int i = 0; i < rawSchedules.Count; i++)
            {
                // Only process schedules of type "SCHEDULE OF NOTICES OF LEASES"
                if (rawSchedules[i].EntryType.ToUpper() == "SCHEDULE OF NOTICES OF LEASES")
                {
                    noticeLeaseSchedules.Add(processRawData(rawSchedules[i]));
                }
            }

            // Reorder the list of NoticeLease objects by EntryNumber
            noticeLeaseSchedules.Sort((x, y) => x.EntryNumber.CompareTo(y.EntryNumber));

            // Return the list of Raw objects
            return noticeLeaseSchedules;
        }

        private RestRequest CreateGet(string name)
        {
            var request = new RestRequest(name);
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("testy:mcTestFace")));
            return request;
        }

        private NoticeLeaseSchedule processRawData(RawSchedule rawSchedule)
        {
            NoticeLeaseSchedule noticeLease = new()
            {
                
                EntryNumber = int.Parse(rawSchedule.EntryNumber),
                EntryDate = new DateOnly(rawSchedule.EntryDate)
            };

            noticeLease.Parse(rawSchedule.EntryText);
            //{
            //   "entryNumber":"2",
            //   "entryDate":"",
            //   "entryType":"Schedule of Notices of Leases",
            //   "entryText":[
            //      "15.11.2018      Ground Floor Premises         10.10.2018      TGL513556  ",
            //      "Edged and                                     from 10                    ",
            //      "numbered 2 in                                 October 2018               ",
            //      "blue (part of)                                to and                     ",
            //      "including 19               ",
            //      "April 2028"
            //   ]
            //}

            //{
            //  "entryNumber": 0,
            //  "entryDate": {
            //                  "year": 0,
            //    "month": 0,
            //    "day": 0,
            //    "dayOfWeek": 0,
            //    "dayOfYear": 0,
            //    "dayNumber": 0
            //  },
            //  "registrationDateAndPlanRef": "string",
            //  "propertyDescription": "string",
            //  "dateOfLeaseAndTerm": "string",
            //  "lesseesTitle": "string",
            //  "notes": [
            //    "string"
            //  ]
            //}
            return noticeLease;
        }
    }
}