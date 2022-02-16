using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudDataProtection.App.Shared.Rest
{
    public class SearchOutput
    {
        public IEnumerable<string> Names { get; set; }

        public SearchOutput()
        {
        }
    }
}
