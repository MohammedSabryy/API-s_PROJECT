﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record ProductResultDTO
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal price { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
    }
}
