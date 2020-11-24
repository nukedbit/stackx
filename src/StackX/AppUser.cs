using System;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;

namespace StackX.ServiceInterface
{
    public class AppUser : UserAuth
    {
        [Default("")]
        public string ProfileUrl { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}