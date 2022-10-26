using System;
using System.Collections.Generic;
using System.Linq;
using EnterpriseApp.API.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Responses
{
    public class ApiResponse
    {
        public StatusCode Code { get; }

        public string Status { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(StatusCode statusCode, string message = null, string status = "success")
        {
            Code = statusCode;
            Message = message;
            Status = status;
        }
    }

    public class ApiOkResponse : ApiResponse
    {
        public object Data { get; }

        public ApiOkResponse(object data) : base(StatusCode.SUCCESS)
        {
            Data = data;
        }
    }

    public class ApiError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "validationInfo")]
        public IEnumerable<string> ValidationInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "code")]
        public StatusCode Code { get; set; }
    }

    public class CommonErrorModel
    {
        [JsonProperty("error")]
        public ApiError Error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public CommonErrorModel(StatusCode statusCode, string message = null, IEnumerable<string> errors = null)
        {
            Error = new ApiError()
            {
                Title = "SLFP Detha",
                Message = message,
                ValidationInfo = errors,
                Code = statusCode
            };

            Status = "FAIL";
        }
    }

    public class ApiBadRequestResponse : CommonErrorModel
    {
        public ApiBadRequestResponse(ModelStateDictionary modelState) : base(StatusCode.ERROR_InvalidParameters, "Field Validation Error")
        {
            if (modelState.IsValid)
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));

            Error.ValidationInfo = from e in modelState.Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
                                   select string.Format("{0} - {1}", e.Key, string.Join(",", e.Value.Errors.Select(x => x.Exception != null ? "Field Parsing Error Occurred" : x.ErrorMessage).Distinct()));
        }
    }

    public class ModelStateNode
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}
