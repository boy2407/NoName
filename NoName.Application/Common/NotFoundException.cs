using System;

namespace NoName.Application.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) not found.")
        {
        }
    }
}
