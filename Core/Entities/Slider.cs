﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Slider:AuditEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
    }
}
