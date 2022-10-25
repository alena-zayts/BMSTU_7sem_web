using BL.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BL.Exceptions.UserExceptions
{
    public class UserRegistrationException : Exception
    {
        public User? User { get; }
        public UserRegistrationException() : base() { }
        public UserRegistrationException(string? message) : base(message) { }
        public UserRegistrationException(string? message, Exception? innerException) : base(message, innerException) { }

        public UserRegistrationException(string? message, User? user) : base(message)
        {
            this.User = user;
        }
    }
}
