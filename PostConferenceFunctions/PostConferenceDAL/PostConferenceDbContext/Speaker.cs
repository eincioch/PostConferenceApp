using System;
using System.Collections.Generic;

#nullable disable

namespace PostConferenceDAL.PostConferenceDbContext
{
    public partial class Speaker
    {
        public int SpeakerId { get; set; }
        public int WebinarId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
