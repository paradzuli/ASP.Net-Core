﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoreProject1.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }

    }
}
