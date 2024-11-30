

using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using Core.CrossCuttingConcerns.Rules;
using TechCareer.Models.Entities;
using TechCareer.Service.Constants;

namespace TechCareer.Service.Rules;

public class InstructorBusinessRules : BaseBusinessRules
{
    public void IsInstructorNameUnique (bool anyInstructor)
    {
        if (anyInstructor)
        {
            throw new BusinessException(InstructorMessages.InstructorNameAlreadyExistsMessage);
        }
    }

    public void IsInstructorExists(Instructor? instructor)
    {
        if (instructor is null)
        {
            throw new NotFoundException(InstructorMessages.InstructorNotFoundMessage);
        }
    }
}
