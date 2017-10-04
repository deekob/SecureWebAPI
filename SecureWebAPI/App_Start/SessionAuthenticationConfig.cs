using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using SecureWebAPI;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SessionAuthenticationConfig), "PreAppStart")]

namespace SecureWebAPI
{
    public static class SessionAuthenticationConfig
    {
        public static void PreAppStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(System.IdentityModel.Services.SessionAuthenticationModule));
        }
    }
}