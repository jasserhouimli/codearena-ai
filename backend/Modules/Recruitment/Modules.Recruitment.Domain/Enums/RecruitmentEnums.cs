namespace Modules.Recruitment.Domain.Enums;

public enum JobStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2,
    Closed = 3
}

public enum ApplicationStage
{
    Applied = 0,
    RepoAudited = 1,
    DefenseQuizCompleted = 2,
    SandboxPassed = 3,
    Shortlisted = 4,
    Rejected = 5,
    Hired = 6
}
