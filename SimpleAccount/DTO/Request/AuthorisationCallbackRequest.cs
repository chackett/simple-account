using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;

namespace SimpleAccount.DTO.Request
{
    public class AuthorisationCallbackRequest
    {
        [BindProperty(Name = "code")]
        public string Code { get; set; }
        [BindProperty(Name = "state")]
        public string State { get; set; }
        [BindProperty(Name = "scope")]
        public string Scope { get; set; }
        [BindProperty(Name = "error")]
        public string Error { get; set; }
        
    }
}