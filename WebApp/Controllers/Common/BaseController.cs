using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Controllers.Common
{
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected readonly UserManager<IdentityUser> _userManager;
        public BaseController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
