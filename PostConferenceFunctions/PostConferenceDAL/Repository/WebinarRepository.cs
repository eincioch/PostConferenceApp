using System;
using PostConferenceDAL.PostConferenceDbContext;

namespace PostConferenceDAL.Repository
{
    public class WebinarRepository : GenericRepository<Webinar, PostConferenceDatabaseContext>
    {
        public WebinarRepository(PostConferenceDatabaseContext context) : base(context)
        {
        }
    }
}
