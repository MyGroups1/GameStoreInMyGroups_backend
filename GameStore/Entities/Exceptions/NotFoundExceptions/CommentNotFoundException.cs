using System;
using Entities.Exceptions.Abstract;

namespace Entities.Exceptions.NotFoundExceptions
{
    public sealed class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(Guid categoryId)
            : base($"The category with id: {categoryId} doesn't exist in the database.")
        {
        }
    }
}