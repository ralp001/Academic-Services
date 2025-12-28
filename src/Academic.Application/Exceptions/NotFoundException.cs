using System;

namespace Academic.Application.Exceptions
{
    /// <summary>
    /// Custom exception to be thrown when a requested entity cannot be found.
    /// This will be mapped to an HTTP 404 response by the middleware.
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}