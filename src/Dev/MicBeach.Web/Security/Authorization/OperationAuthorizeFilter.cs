using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Web.Security.Authorization
{
    public class OperationAuthorizeFilter : AuthorizeFilter
    {
        /// <summary>
        /// authorize verify func
        /// </summary>
        public static Func<AuthorizationFilterContext, Task<AuthorizeVerifyResult>> AuthorizeVerifyFuncAsync
        {
            get; set;
        }

        private static bool HasAllowAnonymous(IList<IFilterMetadata> filters)
        {
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var task = base.OnAuthorizationAsync(context);
            if (context.Result != null && (context.Result is ChallengeResult || context.Result is ForbidResult))
            {
                return;
            }
            if (HasAllowAnonymous(context.Filters))//允许匿名访问
            {
                return;
            }
            bool isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                context.Result = new ChallengeResult();
                return;
            }
            if (AuthorizeVerifyFuncAsync != null)
            {
                var verifyResult = await AuthorizeVerifyFuncAsync(context).ConfigureAwait(false) ?? AuthorizeVerifyResult.ChallengeResult();
                switch (verifyResult.VerifyValue)
                {
                    case AuthorizeVerifyValue.Challenge:
                        context.Result = new ChallengeResult();
                        break;
                    case AuthorizeVerifyValue.Forbid:
                    default:
                        context.Result = new ForbidResult();
                        break;
                    case AuthorizeVerifyValue.Success:
                        break;
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
            await Task.CompletedTask;
        }
    }
}
