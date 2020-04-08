using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAccount.DTO.Request;
using SimpleAccount.DTO.Response;
using SimpleAccount.Services;

namespace SimpleAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsentController : ControllerBase
    {
        private readonly IConsentService _consentService;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(ILogger<ConsentController> logger, IConsentService consentService,
            IAccountService accountService, ITrueLayerDataApi trueLayerDataApi)
        {
            _logger = logger;
            _consentService = consentService;
        }

        [HttpPost("[action]", Name = "Account_Auth_Callback")]
        public async Task<AuthorisationCallbackResponse> Callback([FromForm] AuthorisationCallbackRequest request)
        {
            if (!string.IsNullOrEmpty(request.Error))
                return new AuthorisationCallbackResponse
                {
                    Message = $"ErrorMessage - {request.Error}"
                };

            var consent = await _consentService.CallbackAsync(request.Code, request.State);

            return new AuthorisationCallbackResponse
            {
                Message = $"Success - Consent to {consent.ConnectorId} granted."
            };
        }

        [HttpGet("[action]", Name = "Account_Auth_Link")]
        public AuthorisationLinkResponse Authorise(string userId = null)
        {
            // This use of userId as an argument, and being used for state is questionable and requires further
            // review but it serves it's purpose to avoid having to hard code users.
            if (string.IsNullOrEmpty(userId))
                return new AuthorisationLinkResponse
                {
                    Error = "user not provided"
                };

            return new AuthorisationLinkResponse
            {
                AuthorisationUrl = _consentService.AuthorisationUrl(userId)
            };
        }
    }
}