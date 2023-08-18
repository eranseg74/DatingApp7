using Microsoft.AspNetCore.Mvc.Filters;
using API.Extensions;
using API.Interfaces;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // The ActionExecutingContext will be used for actions we want to perform before the api action is executed
        // The ActionExecutionDelegate will be used for actions we want to perform after the api action is executed
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return; // Checking first if the user is authenticated
            var userId = resultContext.HttpContext.User.GetUserId(); // Getting the username from the api call's result
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>(); // Using the User Repository
            var user = await repo.GetUserByIdAsync(userId); // Finding the user using the UserRepository
            user.LastActive = DateTime.UtcNow; // Setting the LastActive parameter for the specific user
            await repo.SaveAllAsync(); // Updating the database
        }
    }
}