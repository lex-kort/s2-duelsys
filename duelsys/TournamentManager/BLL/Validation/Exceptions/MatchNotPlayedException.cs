﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation.Exceptions
{
    public class MatchNotPlayedException : Exception
    {
        public MatchNotPlayedException(string message)
            : base(message)
        { }
    }
}
