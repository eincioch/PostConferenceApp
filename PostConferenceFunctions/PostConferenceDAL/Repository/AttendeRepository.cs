using System;
using PostConferenceDAL.PostConferenceDbContext;

namespace PostConferenceDAL.Repository
{
    public class AttendeeRepository : GenericRepository<Attendee, PostConferenceDatabaseContext>
    {
        public AttendeeRepository(PostConferenceDatabaseContext context) : base(context)
        {
        }
    }
}
