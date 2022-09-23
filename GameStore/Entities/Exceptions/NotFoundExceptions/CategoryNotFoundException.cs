using System;
using Entities.Exceptions.Abstract;

namespace Entities.Exceptions.NotFoundExceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(Guid categoryId)
            : base($"The category with id: {categoryId} doesn't exist in the database.")
        {
        }
    }
}