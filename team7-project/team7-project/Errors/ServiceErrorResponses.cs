using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Models.Errors;

namespace team7_project.Errors
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty.",
                    Target = target
                }
            };

            return error;
        }

        public static ServiceErrorResponse UserNameAlreadyExists(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = $"User \"{userName}\" already exists.",
                    Target = userName
                }
            };

            return error;
        }

        public static ServiceErrorResponse NotEnoughUserData()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"Username and / or password are not entered.",
                    Target = "user"
                }
            };

            return error;
        }

        public static ServiceErrorResponse UserNotFound()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.ValidationError,
                    Message = $"There is no user with this login or password.",
                    Target = "user"
                }
            };
            return error;
        }

        public static ServiceErrorResponse TreeNotFound(string id)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = $"There is no tree with id: \"{id}\"",
                    Target = "tree"
                }
            };
            return error;
        }
    }
}
