using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandelResponses
{
    public class CustomException : Response
    {
        public CustomException(int StatusCode, string? message = null,string? details = null) : base(StatusCode, message)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
