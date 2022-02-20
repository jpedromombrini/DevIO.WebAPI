using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DevIO.API.Dtos;


namespace DevIO.API.Controllers
{    
    [Route("api/conta")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(INotificador notificador) : base(notificador)
        {
            
        }
        public async Task<ActionResult> Registrar(RegisterUserDto registerUserDto)
        {
            return CustomResponse(registerUserDto);
        }
    }
}