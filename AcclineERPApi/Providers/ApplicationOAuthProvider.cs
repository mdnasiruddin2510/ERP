
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using AcclineERPApi.Filters;
using AcclineERPApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebMatrix.WebData;


namespace AcclineERPApi.Providers
{
   
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }
            _publicClientId = publicClientId;
        }

        //[InitializeSimpleMembership]
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //** Replace below user authentication code as per your Entity Framework Model ***        
            //if (!WebSecurity.Login(context.UserName, context.Password, true))
            //{
            //    context.SetError("invalid_grant",
            //    "the user name or password is incorrect.");
            //    return;
            //}

            ASPLEntities db = new ASPLEntities();
            SecUserInfo user = db.SecUserInfoes.Where(u => u.UserName == context.UserName && u.Password == context.Password).SingleOrDefault();

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            

            ClaimsIdentity oAuthIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);
            ClaimsIdentity cookiesIdentity =
            new ClaimsIdentity(context.Options.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(context.UserName);
            AuthenticationTicket ticket =
            new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            string username=string.Empty;
            foreach (KeyValuePair<string,
            string> property in context.Properties.Dictionary)
            {
                if (property.Key.Equals("userName"))
                {
                    username = property.Value;
                }
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            //context.AdditionalResponseParameters.Add("minAmount", currentUser.MinAmount);
            //context.AdditionalResponseParameters.Add("maxAmount", currentUser.MaxAmount);

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication
        (OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri
        (OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }
            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string>
            data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}