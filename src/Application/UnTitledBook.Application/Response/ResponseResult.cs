using UnTitledBook.Application.Interfaces;
using UntitledBook.Domain.Enums;
using Serilog;

namespace UnTitledBook.Application.Response
{
    public class ResponseResult : IResponseResult
    {
        private string _message = string.Empty;
        private bool _isError = false;
        private ResponseStatus _responseStatus;
        private Dictionary<string, object> _fields = new();

        public string Message
        {
            get => _message;
            set => _message = value;
        }

        public bool IsError
        {
            get => _isError;
            set => _isError = value;
        }

        public ResponseStatus ResponseStatus
        {
            get => _responseStatus;
            set => _responseStatus = value;
        }

        public IDictionary<string, object> Fields
        {
            get => _fields;
            set => _fields = (Dictionary<string, object>)value;
        }

        public IResponseResult SetErrorResponse(string mes = "error", ResponseStatus responseStatus = ResponseStatus.Error, string logMessage = "")
        {
            IsError = true;
            Message = mes;
            ResponseStatus = responseStatus;
            return this;
        }

        public IResponseResult SetSuccessResponse(string mes = "success", ResponseStatus responseStatus = ResponseStatus.Success)
        {
            IsError = false;
            Message = mes;
            ResponseStatus = responseStatus;
            return this;
        }

        public IResponseResult SetErrorValidationResponse(List<object> errors, ResponseStatus responseStatus = ResponseStatus.Error)
        {
            foreach (var item in errors)
            {
                var propertyNameProperty = item.GetType().GetProperty("PropertyName");
                var errorMessageProperty = item.GetType().GetProperty("ErrorMessage");

                if (propertyNameProperty != null && errorMessageProperty != null)
                {
                    var propertyName = propertyNameProperty.GetValue(item)?.ToString();
                    var errorMessage = errorMessageProperty.GetValue(item)?.ToString();

                    Fields.Add(propertyName, errorMessage);
                }
            }

            IsError = true;
            Message = "Hata";
            ResponseStatus = responseStatus;
            return this;
        }

        public void SetExceptionResponse(Exception ex)
        {
            IsError = true;
            Message = "Bilinmeyen bir Hata!"; 
            ResponseStatus = ResponseStatus.Exception;
            Log.Fatal(ex.Message);
        }
    }
}
