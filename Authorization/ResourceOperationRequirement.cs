using CarAPI.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CarAPI.Authorization
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {     
        public ResourceOperationType Operation { get;}

        public ResourceOperationRequirement(ResourceOperationType operation)
        {
            Operation = operation;
        }
    }
}
