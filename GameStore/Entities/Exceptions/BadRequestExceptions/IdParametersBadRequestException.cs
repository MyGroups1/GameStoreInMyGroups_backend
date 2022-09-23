using Entities.Exceptions.Abstract;

namespace Entities.Exceptions.BadRequestExceptions
{
    public sealed class IdParametersBadRequestException : BadRequestException
    {
        public IdParametersBadRequestException()
            : base("Parameter ids is null")
        {
        }
    }
}