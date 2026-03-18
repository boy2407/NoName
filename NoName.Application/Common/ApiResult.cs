using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
        public List<string> errors { get; set; }
        public static ApiResult<T> Success(T resultObj, string message = "Success")
        {
            return new ApiResult<T> { IsSuccessed = true, ResultObj = resultObj, Message = message };
        }

        public static ApiResult<T> Failure(string message, List<string> errors = null)
        {
            return new ApiResult<T> { IsSuccessed = false, Message = message,  errors = null };
        }
    }
}
