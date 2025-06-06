﻿using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Models.Identity
{
    public class RegistrationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
