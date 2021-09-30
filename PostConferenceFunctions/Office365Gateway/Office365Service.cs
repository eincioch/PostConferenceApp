using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Office365Gateway.Models;

namespace Office365Gateway
{
    public class Office365Service
    {
        private AppToken appToken;
        private Dictionary<string, string> securityHeaders;
        private string userId;

        public Office365Service(string client_id, string client_secret)
        {
            securityHeaders = new Dictionary<string, string>()
            {
                { "client_id", client_id },
                { "scope", "https://graph.microsoft.com/.default" },
                { "client_secret", client_secret },
                { "grant_type", "client_credentials" }
            };
        }

        public async Task GetAppToken(string tenantId)
        {
            var getTokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            
            using (var tokenClient = new HttpClient())
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, getTokenUrl)
                {
                    Content = new FormUrlEncodedContent(securityHeaders)
                };

                var tokenResponse = await tokenClient.SendAsync(tokenRequest);

                if (tokenResponse.IsSuccessStatusCode)
                {
                    var streamToken = await tokenResponse.Content.ReadAsStreamAsync();
                    appToken = await JsonSerializer.DeserializeAsync<AppToken>(streamToken);
                }
            }
        }

        public async Task<OnlineMeeting> GetOnlineMeeting(string onlineMeetingJoinUrl, string userId)
        {
            var graphOnlineMeetingUrl = $"https://graph.microsoft.com/beta/users/{userId}/onlineMeetings?$filter=JoinWebUrl eq '{onlineMeetingJoinUrl}'";

            using (var graphClient = new HttpClient())
            {
                var graphRequest = new HttpRequestMessage(HttpMethod.Get, graphOnlineMeetingUrl);
                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appToken.access_token);
                graphRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var graphResponse = await graphClient.SendAsync(graphRequest);

                if (graphResponse.IsSuccessStatusCode)
                {
                    var streamGraph = await graphResponse.Content.ReadAsStreamAsync();
                    var meetingInfo = await JsonSerializer.DeserializeAsync<OnlineMeetingsInfo>(streamGraph);
                    return meetingInfo?.value.FirstOrDefault();
                }
            }

            return default(OnlineMeeting);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetMeetingAttendees(OnlineMeeting meeting, string userId)
        {
            var attendanceReporturl = $"https://graph.microsoft.com/beta/users/{userId}/onlineMeetings/{meeting.id}/meetingAttendanceReport";

            using (var graphClient = new HttpClient())
            {
                var graphRequest = new HttpRequestMessage(HttpMethod.Get, attendanceReporturl);
                graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", appToken.access_token);

                var graphResponse = await graphClient.SendAsync(graphRequest);

                if (graphResponse.IsSuccessStatusCode)
                {
                    var streamGraph = await graphResponse.Content.ReadAsStreamAsync();
                    var attendanceReport = await JsonSerializer.DeserializeAsync<AttendanceReport>(streamGraph);
                    return attendanceReport?.attendanceRecords;
                }
            }

            return default(IEnumerable<AttendanceRecord>);
        }
    }
}
