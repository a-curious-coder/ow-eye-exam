using CanYouC_.Interfaces;
using CanYouC_.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using JsonException = System.Text.Json.JsonException;

namespace CanYouC_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeLeaseController : INoticeLeaseController
    {
        private readonly RestClient _client;
        public NoticeLeaseController()
        {
            // TODO: read base URL from config file
            _client = new RestClient("https://localhost:7203");
        }

        /// <summary>
        /// Get a list of NoticeLeaseSchedule objects from the API
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get's NoticeLeaseSchedules from processed raw schedule data
        /// </summary>
        /// <returns>Enumerated list of NoticeLeaseSchedule objects</returns>
        [HttpGet("GetNoticeLeaseSchedulesFromRawScheduleData", Name = "GetNoticeLeaseSchedulesFromRawScheduleData")]
        public IEnumerable<NoticeLeaseSchedule> GetNoticeLeaseFromRawSchedules()
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
                // NOTE: This check is performed to allow for scalability; if the program is expanded to process other types of schedules, this check will allow for that
                // Only process schedules of type "SCHEDULE OF NOTICES OF LEASES"
                if (rawSchedules[i].EntryType.ToUpper() == "SCHEDULE OF NOTICES OF LEASES")
                {
                    noticeLeaseSchedules.Add(ProcessRawData(rawSchedules[i]));
                }
            }

            // Reorder the list of NoticeLease objects by EntryNumber
            noticeLeaseSchedules.Sort((x, y) => x.EntryNumber.CompareTo(y.EntryNumber));

            // Return the list of Raw objects
            return noticeLeaseSchedules;
        }

        /// <summary>
        /// Create's the GET request for the API
        /// </summary>
        /// <param name="name"></param>
        /// <returns>RestSharp request designed to call external API</returns>
        private static RestRequest CreateGet(string name)
        {
            var request = new RestRequest(name);
            // TODO: Add credentials to config file
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("testy:mcTestFace")));
            return request;
        }

        /// <summary>
        /// Processes raw schedule entry text data into a NoticeLeaseSchedule object
        /// </summary>
        /// <param name="rawSchedule"></param>
        /// <returns></returns>
        private static NoticeLeaseSchedule ProcessRawData(RawSchedule rawSchedule)
        {
            NoticeLeaseSchedule noticeLease = new()
            {
                
                EntryNumber = int.Parse(rawSchedule.EntryNumber),
                EntryDate = new DateOnly(rawSchedule.EntryDate)
            };

            noticeLease.Parse(rawSchedule.EntryText);

            return noticeLease;
        }
    }
}