using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

                return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        // Telling the controller that the username will come as an input and the roles will come from the query in the url
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
            // If roles is empty issue a bad request. Any user should have at least one role
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");
            // Transferring the roles string into an array
            var selectedRoles = roles.Split(",").ToArray();
            // Getting the user properties
            var user = await _userManager.FindByNameAsync(username);
            // If the user is not found, returning the appropriate response
            if (user == null) return NotFound();
            // Getting the current roles of the user
            var userRoles = await _userManager.GetRolesAsync(user);
            // Adding the new roles to the user's roles list
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            // Checking that the action was a success
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            // Removing any roles that the user had, that are not specified in the selected roles
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            // Checking that the second action was a success
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");
            // Returning the user's new updated roles
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderate can see this");
        }
    }
}