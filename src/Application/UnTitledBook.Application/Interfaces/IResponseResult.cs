using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledBook.Domain.Enums;

namespace UnTitledBook.Application.Interfaces
{
    public interface IResponseResult
    {
        string Message { get; set; }
        bool IsError { get; set; }
        ResponseStatus ResponseStatus { get; set; }
        IDictionary<string, object> Fields { get; set; }
        IResponseResult SetErrorResponse(string mes = "error", ResponseStatus responseStatus = ResponseStatus.Error, string logMessage = "");
        IResponseResult SetSuccessResponse(string mes = "success", ResponseStatus responseStatus = ResponseStatus.Success);
        IResponseResult SetErrorValidationResponse(List<object> errors, ResponseStatus responseStatus = ResponseStatus.EmptyParameter);
        void SetExceptionResponse(Exception ex);
    }
}
