using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Constants
{
    public static class ValidationRegex
    {
        public const string PropertyRegex = @"^[^<>'""\[\]*^(){}!~#%&]+$";
        public const string DescriptionAndMessageRegex = @"^[^<>]+$";
    }
}
