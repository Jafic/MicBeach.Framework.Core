using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Site.Cms.Config
{
    public class MyAuthenticationHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        static AuthenticationTicket NowTicket = null;
        public AuthenticationScheme Scheme { get; private set; }

        public HttpContext Context { get; private set; }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookieValue = Context.Request.Cookies["login_cookie"];
            if (string.IsNullOrWhiteSpace(cookieValue))
            {
                return AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(NowTicket);
        }

        public async Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/login");
            await Task.CompletedTask;
        }

        public async Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            await Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            Context.Response.Cookies.Append("login_cookie", ticket.ToString());
            NowTicket = ticket;
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete("login_cookie");
            return Task.CompletedTask;
        }
    }
}
