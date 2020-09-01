using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    public static class StringResource
    {
        public static string Error = "Some Error Occured";
        public static string CustomError = "CustomError";
        public static string SuccessLogin = "Logged in successfully";
        public static string Invalid = "Invalid {0}";
        public static string InvalidPassword = "Invalid username or password";
        public static string AccountBlocked = "Your account has been block. Please contact admin for assistance.";
        public static string AccountApproved = "Your account has not been approved by the admin.";
        public static string InvalidUser = "User doesn't exist";
        public static string AlreadyTaken = "The {0} is already taken";
        public static string DoesNotExist = "The record doesn't exist";
        public static string ModelValidationError = "Model validation Error";
        public static string NullException = "Null Exception";
        public static string ReachedMaxAttempts = "Reached max number of attempts";
        public static string CannotRemoveAllItems = "Cannot remove all items from the order.";
        
    }

    public static class RegexResource
    {
        public static string Email = "^[A-Z0-9._%-]+@[A-Z0-9.-]+?\\.[a-zA-Z]{2,3}$";

    }

}
