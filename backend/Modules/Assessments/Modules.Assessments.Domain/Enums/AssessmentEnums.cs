namespace Modules.Assessments.Domain.Enums;

public enum AssessmentType
{
    DefenseQuiz = 0,
    CodingSandbox = 1
}

public enum AssessmentStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Abandoned = 3,
    Failed = 4
}
