using System;

namespace MyLibraryAPI.Helpers
{
    public class ErrorsUtil
    {
        public static object GetErrorMessage(in Exception e)
        {
            string Message = "Something went wrong!";
            string DetailedError = e.Message;
            Exception inner = e.InnerException;
            while (inner != null)
            {
                DetailedError += inner.Message;
                inner = inner.InnerException;
            }
            return new
            {
                Message,
                DetailedError
            };
        }
    }
}