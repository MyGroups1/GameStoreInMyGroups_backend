using System;
using Entities.Exceptions.Abstract;

namespace Entities.Exceptions.NotFoundExceptions
{
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid userId)
            : base($"The user with id: {userId} doesn't exist in the database.")
        {
        }
    }
}