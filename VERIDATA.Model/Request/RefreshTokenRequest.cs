﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VERIDATA.Model.Request
{
    public class RefreshTokenRequest
    {
        //public int UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
