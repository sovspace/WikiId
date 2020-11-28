using Application.Features.AccessRequstFeatures.Commands;
using Application.Features.AccessRequstFeatures.Queries;
using Application.Features.ArticleFeatures.Queries;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Threading.Tasks;
using WebApp.Controllers.Common;
using WebApp.Hubs;

namespace WebApp.Controllers
{
    public class AccessController : BaseController
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public AccessController(UserManager<IdentityUser> userManager,
            IHubContext<NotificationHub> hubContext) : base(userManager)
        {
            _hubContext = hubContext;
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Create(int articleId, AccessType accessType)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var mediatorResult = await Mediator.Send(new CreateAccessRequestCommand
            {
                AccessType = accessType,
                IdentityUserId = identityUser.Id,
                ArticleId = articleId,
                UserRoles = await _userManager.GetRolesAsync(identityUser)
            });
            HttpStatusCode statusCode = mediatorResult.IsSuccessful ? HttpStatusCode.OK : HttpStatusCode.MethodNotAllowed;
            return new JsonResult(mediatorResult.Message)
            {
                StatusCode = (int)statusCode,
            };
        }


        [Authorize]
        [HttpPost]
        public async Task<JsonResult> ApproveAccessRequest(int accessRequestId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            AccessRequest accessRequest = await Mediator.Send(new GetAccessRequestByIdCommand
            {
                AccessRequestId = accessRequestId,
                IdentityUserId = currentUser.Id
            });

            if (accessRequest != null)
            {
                var addedUser = await _userManager.FindByIdAsync(accessRequest.Profile.IdentityUserId);
                string accessRole;
                if (accessRequest.AccessType == AccessType.Edit)
                {
                    accessRole = accessRequest.Article.EditRoleString;
                }
                else
                {
                    accessRole = accessRequest.Article.ViewRoleString;
                }
                var result = await _userManager.AddToRoleAsync(addedUser, accessRole);

                if (result.Succeeded)
                {
                    var mediatorResult = await Mediator.Send(new DeleteAccessRequestCommand { AccessRequstId = accessRequestId, IdentityUserId = currentUser.Id });
                    if (mediatorResult.IsSuccessful)
                    {
                        await _hubContext.Clients.User(addedUser.Id).SendAsync("Notify", $"User {currentUser.UserName} added you give you {accessRole}");
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(addedUser, accessRole);
                    }
                    HttpStatusCode statusCode = mediatorResult.IsSuccessful ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    return new JsonResult(new { mediatorResult.Message }) { StatusCode = (int)statusCode };
                }
                else
                {
                    return new JsonResult(new { result.Errors }) { StatusCode = (int)HttpStatusCode.BadRequest };
                }
            }
            else
            {
                return new JsonResult(new { message = "Not found" }) { StatusCode = (int)HttpStatusCode.NotFound };
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> DisapproveAccessRequest(int accessRequestId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var mediatorResult = await Mediator.Send(new DeleteAccessRequestCommand
            {
                AccessRequstId = accessRequestId,
                IdentityUserId = currentUser.Id
            });
            if (mediatorResult.IsSuccessful)
            {
                await _hubContext.Clients.User(currentUser.Id).SendAsync("Notify", $"User {currentUser.UserName} dissaprove you request");
            }
            HttpStatusCode statusCode = mediatorResult.IsSuccessful ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return new JsonResult(new { mediatorResult.Message }) { StatusCode = (int)statusCode };
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> RemoveUserFromRole(int articleId, string userId, string accessType)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            Article article = await Mediator.Send(new GetArticleByIdQuery { Id = articleId, UserRoles = currentUserRoles });
            if (article != null)
            {
                if (article.Creator.IdentityUserId == userId)
                {
                    return new JsonResult(new { Message = "Can't delete creator" }) { StatusCode = (int)HttpStatusCode.BadRequest };
                }
                if (article.Creator.IdentityUserId == currentUser.Id)
                {
                    var removedUser = await _userManager.FindByIdAsync(userId);
                    string accessRole;
                    if (accessType == AccessType.Edit.ToString())
                    {
                        accessRole = article.EditRoleString;
                    }
                    else
                    {
                        accessRole = article.ViewRoleString;
                    }
                    var result = await _userManager.RemoveFromRoleAsync(removedUser, accessRole);

                    if (result.Succeeded)
                    {
                        await _hubContext.Clients.User(removedUser.Id).SendAsync("Notify", $"User {currentUser.UserName} remove you from role {accessRole}");
                        return new JsonResult(new { message = "Ok" }) { StatusCode = (int)HttpStatusCode.OK };
                    }
                    else
                    {
                        return new JsonResult(new { message = "Can't remove from role" }) { StatusCode = (int)HttpStatusCode.BadRequest };
                    }
                }
                else
                {
                    return new JsonResult(new { message = "Can't do this" });
                }
            }
            else
            {
                return new JsonResult(new { message = "Not found" }) { StatusCode = (int)HttpStatusCode.NotFound };
            }

        }
    }
}
