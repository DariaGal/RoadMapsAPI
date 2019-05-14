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

        public static ServiceErrorResponse UserTreeNotFound(string treeId, string userId)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = $"User({userId}) doesn't have tree with id: \"{treeId}\" in collection",
                    Target = "tree"
                }
            };
            return error;
        }
        
        public static ServiceErrorResponse NodeNotFound(string nodeId, string treeId)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = $"Tree({treeId}) doesn't have node with id: \"{nodeId}\"",
                    Target = "tree"
                }
            };
            return error;
        }

        public static ServiceErrorResponse UserCannotEditTree(string treeId, string userId)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Forbidden,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Forbidden,
                    Message = $"User({userId}) can't edit tree with id \"{treeId}\" because not author",
                    Target = "tree"
                }
            };
            return error;
        }
    }
}
