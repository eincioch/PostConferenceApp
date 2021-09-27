using System;
using PostConferenceDAL.PostConferenceDbContext;

namespace PostConferenceDAL.Repository
{
    public class WebinarFeedbackRepository : GenericRepository<WebinarFeedback, PostConferenceDatabaseContext>
    {
        public WebinarFeedbackRepository(PostConferenceDatabaseContext context) : base(context)
        {
        }
    }
}
