﻿using Microsoft.Build.Framework;

namespace TestApiJWT.Models
{
    public class AddRoleModel
    {
        [Required]
        public string UserId {  get; set; }
        [Required]
        public string Role { get; set; }

    }
}
