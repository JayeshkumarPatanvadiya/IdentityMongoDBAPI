using IdentityMongoDBAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMongoDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationsUsers> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private SignInManager<ApplicationsUsers> _signInManager;


        public UserController(UserManager<ApplicationsUsers> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationsUsers> signInManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {

                var userExists = await _userManager.FindByNameAsync(model.Name);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                ApplicationsUsers user = new ApplicationsUsers()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Name
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            
            return Ok();

        }


        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] UserRole role)
        {

            var roleExists = await _roleManager.FindByNameAsync(role.RoleName);
            if (roleExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = role.RoleName
            });
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Ok();

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]

        public async Task<IActionResult> Login([FromBody] [Required][EmailAddress] string email,[Required] string password,string requiredurl)
        {
            ApplicationsUsers appUser = await _userManager.FindByEmailAsync(email);
            if(appUser != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, password,false,false);
                if(result.Succeeded)
                {
                    return Ok();
                }
            }
            return NotFound();
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok();
        //}


    }
}
