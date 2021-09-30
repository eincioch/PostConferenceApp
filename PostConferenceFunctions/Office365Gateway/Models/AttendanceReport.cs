using System;
using System.Collections.Generic;

namespace Office365Gateway.Models
{
    public class AttendanceReport
    {
        public List<AttendanceRecord> attendanceRecords { get; set; }
        public int totalParticipantCount { get; set; }
    }

    public class AttendanceRecord
    {
        public string emailAddress { get; set; }
        public int totalAttendanceInSeconds { get; set; }
        public string role { get; set; }
        public AttendeeIdentity identity { get; set; }
        public Attendanceinterval[] attendanceIntervals { get; set; }
    }

    public class AttendeeIdentity
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public object tenantId { get; set; }
    }

    public class Attendanceinterval
    {
        public DateTime joinDateTime { get; set; }
        public DateTime leaveDateTime { get; set; }
        public int durationInSeconds { get; set; }
    }
}
