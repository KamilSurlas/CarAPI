using CarAPI.Entities;
using CarAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Car>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Car car)
        {
            if(requirement.Operation == ResourceOperationType.Read || requirement.Operation == ResourceOperationType.Create)
            {
                context.Succeed(requirement);
            }
            var userId = context.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(car.CreatedByUserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
