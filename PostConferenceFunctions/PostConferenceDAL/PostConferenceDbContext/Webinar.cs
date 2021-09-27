using System;
using System.Collections.Generic;

#nullable disable

namespace PostConferenceDAL.PostConferenceDbContext
{
    public partial class Webinar
    {
        public int WebinarId { get; set; }
        public string OnlineMeetingJoinUrl { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? LiveDuration { get; set; }
        public bool? WebinarStatusReport { get; set; }
        public string FeedbackFormUrl { get; set; }
    }
}
