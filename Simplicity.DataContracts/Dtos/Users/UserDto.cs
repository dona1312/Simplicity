﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Simplicity.DataContracts.Dtos.Users
{
    public class UserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Role Role { get; set; }

        public string PicturePath { get; set; }

        public DateTime? Birthday { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
