﻿using System;
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
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AnalysisController> _logger;
        private readonly IConsentService _consentService;

        public AccountController(ILogger<AnalysisController> logger, IConsentService consentService)
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
                    Message = request.Error
                };    
            }
            
            // Take the callback to finalise link to PSU account
            // Store the token somewhere.
            
            await _consentService.CallbackAsync(request.Code, request.State);

            // return URI of the created resource.
            // return response.Headers.Location;
            return new DTO.Response.AuthorisationCallbackResponse()
            {
                Message = ""
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