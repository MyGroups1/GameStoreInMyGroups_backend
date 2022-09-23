using System;
using Entities.Exceptions.Abstract;

namespace Entities.Exceptions.NotFoundExceptions
{
    public sealed class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(Guid postId)
            : base($"The post with id: {postId} doesn't exist in the database.")
        {
        }
    }
}