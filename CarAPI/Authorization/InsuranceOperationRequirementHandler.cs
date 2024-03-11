using CarAPI.Entities;
using CarAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarAPI.Authorization
{
    public class InsuranceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Insurance>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Insurance insurance)
        {
            var userId = context.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (requirement.Operation == ResourceOperationType.Read)
            {
                context.Succeed(requirement);
            }
            if (requirement.Operation == ResourceOperationType.Create)
            {
                if (context.User.IsInRole("Insurer"))
                {
                    context.Succeed(requirement);
                }
            }
            if (insurance.Car.CreatedByUserId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
