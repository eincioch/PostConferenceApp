using System;
using System.Collections.Generic;

#nullable disable

namespace PostConferenceDAL.PostConferenceDbContext
{
    public partial class WebinarFeedback
    {
        public int WebinarFeedbackId { get; set; }
        public int WebinarId { get; set; }
        public byte Rating { get; set; }
        public string Comments { get; set; }
        public string NextTopics { get; set; }
        public DateTime FeedbackDateTime { get; set; }
    }
}
