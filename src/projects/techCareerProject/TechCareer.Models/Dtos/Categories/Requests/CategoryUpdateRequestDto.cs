﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Models.Dtos.Categories.Requests
{
    public sealed class CategoryUpdateRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
