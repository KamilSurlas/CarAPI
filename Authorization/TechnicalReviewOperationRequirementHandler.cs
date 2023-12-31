using CarAPI.Entities;
using CarAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarAPI.Authorization
{
    public class TechnicalReviewOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, TechnicalReview>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, TechnicalReview technicalReview)
        {
            var userId = context.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (requirement.Operation == ResourceOperationType.Read)
            {
                context.Succeed(requirement);
            }
            if (requirement.Operation == ResourceOperationType.Create)
            {
                if (context.User.IsInRole("Mechanic"))
                {
                    context.Succeed(requirement);
                }
            }
  
                if (technicalReview.AddedByUserId == int.Parse(userId))
                {
                    context.Succeed(requirement);
                }
            
            return Task.CompletedTask;
        }
    }
}
