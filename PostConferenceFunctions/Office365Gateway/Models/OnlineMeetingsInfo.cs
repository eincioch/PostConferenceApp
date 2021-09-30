using System;

namespace Office365Gateway.Models
{
    public class OnlineMeetingsInfo
    {
        public OnlineMeeting[] value { get; set; }
    }

    public class OnlineMeeting
    {
        public string id { get; set; }
        public DateTime creationDateTime { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string joinUrl { get; set; }
        public string joinWebUrl { get; set; }
        public object meetingCode { get; set; }
        public string subject { get; set; }
        public bool isBroadcast { get; set; }
        public string autoAdmittedUsers { get; set; }
        public object outerMeetingAutoAdmittedUsers { get; set; }
        public bool isEntryExitAnnounced { get; set; }
        public string allowedPresenters { get; set; }
        public string allowMeetingChat { get; set; }
        public bool allowTeamworkReactions { get; set; }
        public bool allowAttendeeToEnableMic { get; set; }
        public bool allowAttendeeToEnableCamera { get; set; }
        public bool recordAutomatically { get; set; }
        public object[] capabilities { get; set; }
        public object videoTeleconferenceId { get; set; }
        public object externalId { get; set; }
        public object broadcastSettings { get; set; }
        public object audioConferencing { get; set; }
        public object meetingInfo { get; set; }
        public Participants participants { get; set; }
        public Lobbybypasssettings lobbyBypassSettings { get; set; }
        public Chatinfo chatInfo { get; set; }
        public Joininformation joinInformation { get; set; }
        public string meetingAttendanceReportodatacontext { get; set; }
        public object meetingAttendanceReport { get; set; }
    }

    public class Participants
    {
        public Organizer organizer { get; set; }
        public object[] attendees { get; set; }
        public object[] producers { get; set; }
        public object[] contributors { get; set; }
    }

    public class Organizer
    {
        public string upn { get; set; }
        public string role { get; set; }
        public Identity identity { get; set; }
    }

    public class Identity
    {
        public object acsUser { get; set; }
        public object spoolUser { get; set; }
        public object phone { get; set; }
        public object guest { get; set; }
        public object encrypted { get; set; }
        public object onPremises { get; set; }
        public object acsApplicationInstance { get; set; }
        public object spoolApplicationInstance { get; set; }
        public object applicationInstance { get; set; }
        public object application { get; set; }
        public object device { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public object displayName { get; set; }
        public string tenantId { get; set; }
        public string identityProvider { get; set; }
    }

    public class Lobbybypasssettings
    {
        public string scope { get; set; }
        public bool isDialInBypassEnabled { get; set; }
    }

    public class Chatinfo
    {
        public string threadId { get; set; }
        public string messageId { get; set; }
        public object replyChainMessageId { get; set; }
    }

    public class Joininformation
    {
        public string content { get; set; }
        public string contentType { get; set; }
    }
}