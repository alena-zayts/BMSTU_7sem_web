using BL.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BL.Exceptions.UserExceptions
{
    public class UserDuplicateException : Exception
    {
        public User? User { get; }
        public UserDuplicateException() : base() { }
        public UserDuplicateException(string? message) : base(message) { }
        public UserDuplicateException(string? message, Exception? innerException) : base(message, innerException) { }

        public UserDuplicateException(string? message, User? user) : base(message)
        {
            this.User = user;
        }
    }
}
