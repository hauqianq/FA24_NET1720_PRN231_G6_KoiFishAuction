﻿using System.ComponentModel.DataAnnotations;

namespace KoiFishAuction.Common.RequestModels.User
{
    public class LoginUserRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
