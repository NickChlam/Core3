using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Api.helpers
{
    public class updateUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // await next - when the action is completed 
            var resultsContext = await next();
            // get user id using result context to gain access to HttpContext 
            var userId= int.Parse(resultsContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // get repo using get services 
            var repo = resultsContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            // the the current user 
            var user = await repo.GetUser(userId);
            // ser the users last active to now 
            user.LastActive = DateTime.Now;

            await repo.SaveAll();

        }
    }
}