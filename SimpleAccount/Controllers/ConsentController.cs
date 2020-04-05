using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleAccount.DTO.Response;
using SimpleAccount.Services;

namespace SimpleAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsentController : ControllerBase
    {
        private readonly ILogger<ConsentController> _logger;
        private readonly IConsentService _consentService;

        public ConsentController(ILogger<ConsentController> logger, IConsentService consentService,
            IAccountService accountService, ITrueLayerDataApi trueLayerDataApi)
        {
            _logger = logger;
            _consentService = consentService;
        }

        [HttpPost("[action]", Name = "Account_Auth_Callback")]
        public async Task<DTO.Response.AuthorisationCallbackResponse> Callback([FromForm] DTO.Request.AuthorisationCallbackRequest request)
        {
            if (!string.IsNullOrEmpty(request.Error))
            {
                return new DTO.Response.AuthorisationCallbackResponse()
                {
                    Message = $"Error - {request.Error}"
                };    
            }
            
            await _consentService.CallbackAsync(request.Code, request.State);
            
            return new DTO.Response.AuthorisationCallbackResponse()
            {
                Message = "Success - Consent to accounts granted."
            };
        }

        [HttpGet("[action]", Name = "Account_Auth_Link")]
        public DTO.Response.AuthorisationLinkResponse Authorise(string userId = null)
        {
            // This use of userId as an argument, and being used for state is questionable and requires further
            // review but it serves it's purpose to avoid having to hard code users.
            if (string.IsNullOrEmpty(userId))
            {
                return new DTO.Response.AuthorisationLinkResponse
                {
                    Error = "user not provided"
                };
            }

            return new DTO.Response.AuthorisationLinkResponse
            {
                AuthorisationUrl = _consentService.AuthorisationUrl(userId)
            };
        }
    }
}