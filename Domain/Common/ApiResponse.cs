using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public int Status {  get; set; }
        public string ErrorMessage { get; set; }
        public ApiResponse()
        {
            Success = true;
        }
    }
}
