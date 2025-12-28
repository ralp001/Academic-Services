using MediatR;

namespace Academic.Application.Features.Subjects.Queries
{
    /// <summary>
    /// Query to retrieve all subjects assigned to a specific academic class.
    /// </summary>
    public class GetSubjectsByClassIdQuery : IRequest<List<SubjectDto>>
    {
        public Guid ClassId { get; set; }

        public GetSubjectsByClassIdQuery(Guid classId)
        {
            ClassId = classId;
        }
    }
}