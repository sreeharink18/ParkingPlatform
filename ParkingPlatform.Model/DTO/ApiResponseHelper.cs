using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlatform.Model.DTO
{
    public class ApiResponseHelper
    {
        public static ApiResponse SuccessResponse(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse
            {
                IsSuccess = true,
                Result = result,
                StatusCode = statusCode
            };
        }
        public static ApiResponse ErrorResponse(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}
