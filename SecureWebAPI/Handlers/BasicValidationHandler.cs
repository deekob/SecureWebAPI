﻿using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SecureWebAPI.Handlers
{
    public class BasicValidationHandler : DelegatingHandler
    {
   
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {


        HttpResponseMessage errorResponse = null;
        if (request.RequestUri.AbsolutePath.Contains("swagger"))
        {
            return base.SendAsync(request, cancellationToken);
        }

        var credentials = ParseAuthorizationHeader(request);

        if (credentials != null)
        {
            var identity = new BasicAuthenticationIdentity(credentials.Name, credentials.Password);
            var principal = new GenericPrincipal(identity, null);

            //   Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
                HttpContext.Current.User = principal;
        }
        else
        {
            errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "cant find basic auth token");
        }

        return errorResponse != null ?
            Task.FromResult(errorResponse) :
            base.SendAsync(request, cancellationToken);
    }
   
    /// <summary>
    /// Parses the Authorization header and creates user credentials
    /// </summary>
    /// <param name="actionContext"></param>
    protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
    {
        string authHeader = null;
        var auth = request.Headers.Authorization;
        if (auth != null && auth.Scheme == "Basic")
            authHeader = auth.Parameter;

        if (string.IsNullOrEmpty(authHeader))
            return null;

        authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

        var tokens = authHeader.Split(':');
        if (tokens.Length < 2)
            return null;

        return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
    }



    
    }

    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password) : base(name, "Basic")
        {
            this.Password = password;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }
    }
}