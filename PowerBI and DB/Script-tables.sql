Create table Webinar
(
WebinarID Int IDENTITY (1,1) NOT NULL,
WebinarName Varchar(200),
OnlineMeetingJoinUrl Varchar(MAX),
StartDateTime DateTime,
EndDateTime DateTime,
LiveDuration Int,
WebinarStatusReport Bit,
FeedbackFormUrl Varchar(MAX),
DiplomaTemplateUrl Varchar(MAX),
CONSTRAINT PK_Webinar PRIMARY KEY CLUSTERED (WebinarID)
);

Create table Speaker
(
SpeakerID Int IDENTITY (1,1) NOT NULL,
WebinarID Int,
FullName Varchar(200),
Email Varchar(200),
CONSTRAINT PK_Speaker PRIMARY KEY CLUSTERED (SpeakerID)
);


Create table Attendee
(
AttendeeID Int IDENTITY (1,1) NOT NULL,
WebinarID Int,
FullName Varchar(200),
Email Varchar(200),
Duration Int,
AttendeeEmailSent Bit,
DiplomaUrl Varchar(MAX),
CONSTRAINT PK_Attendee PRIMARY KEY CLUSTERED (AttendeeID)
);

Create table WebinarFeedback
(
WebinarFeedbackID Int IDENTITY (1,1) NOT NULL,
WebinarID Int,
Rating TinyInt,
Comments Varchar(MAX),
NextTopics Varchar(MAX),
FeedbackDateTime DateTime,
CONSTRAINT PK_WebinarFeedback PRIMARY KEY CLUSTERED (WebinarFeedbackID)
);