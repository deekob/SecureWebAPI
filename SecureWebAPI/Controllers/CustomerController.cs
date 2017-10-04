using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace SecureWebAPI.Controllers
{
    [RoutePrefix("api/v1/customer")]
    public class CustomerController : ApiController
    {
        private const string CustomerIdClaimName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        protected string CustomerId
        {
            get
            {
                ClaimsPrincipal claims = User as ClaimsPrincipal;

                if (claims == null)
                {
                    Debug.WriteLine(
                        $"Could not get User as a {typeof(ClaimsPrincipal).FullName}, got {User.GetType().FullName} instead.");
                    return null;
                }

                Claim id =
                    claims.Claims.FirstOrDefault(
                        x => string.Equals(x.Type, CustomerIdClaimName, StringComparison.OrdinalIgnoreCase));

                if (id != null)
                {
                    return (id.Value);
                }

                Debug.WriteLine($"Could not retrieve required claim from the ClaimPrincipal to identify the customer.");
                return null;
            }
        }


     
        [Authorize]
        [HttpGet]
        [Route("current/bids")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> GetBids()
        {
            var customerId = CustomerId;

            return Ok($"no bids for customer {customerId}");
        }

        [Authorize]
        [HttpGet]
        [Route("current")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> GetCustomer()
        {
            var customerId = CustomerId;

            return Ok($"no bids for customer {customerId}");
        }

    }
}
