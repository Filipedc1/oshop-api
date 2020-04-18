using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ShopApi.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string ProfileImageUrl   { get; set; }
        public string FirstName         { get; set; }
        public string LastName          { get; set; }
        public bool IsActive            { get; set; }

        public DateTime MemberSince     { get; set; }
        //public Address Address          { get; set; }

        public IEnumerable<IdentityUserClaim<string>> Claims { get; set; }
        //public ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}
