﻿namespace ShopApi.DTOs
{
    public class AuthenticatedUserDto
    {
        public string Username  { get; set; }
        public bool IsAdmin     { get; set; }
        public string Token     { get; set; }
    }
}
