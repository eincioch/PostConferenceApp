using System;
using System.Collections.Generic;

#nullable disable

namespace PostConferenceDAL.PostConferenceDbContext
{
    public partial class Attendee
    {
        public int AttendeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? Duration { get; set; }
        public bool? AttendeeEmailSent { get; set; }
        public string DiplomaUrl { get; set; }
    }
}
