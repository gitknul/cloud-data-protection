using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class SearchController
    {
        private string[] names = new[]
        {
            "Olivier", "Knul", "Menneke", "Olly", "Gerard", "Mark", "Kevin", "Wilson", "Freshheads", "Oli4", "Oli5",
            "Gerardus", "Markus", "Marko", "Piet", "Peter", "Wout", "Wouter", "Wim", "Henk"
        };

        [HttpGet("")]
        public ActionResult Search(string text)
        {
            text = text?.Trim()?.ToLower();

            if (text == null)
            {
                return new OkObjectResult(new SearchOutput
                {
                    Names = names.Randomize().ToList()
                });
            }

            return new OkObjectResult(new SearchOutput
            {
                Names = names
                    .Where(n => n.Contains(text, StringComparison.OrdinalIgnoreCase))
                    .Randomize()
                    .ToList()
            });
        }
    }

    public static class Shuffler
    {
        private static Random rnd = new Random();

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(_ => rnd.Next());
        }
    }

    public class SearchOutput
    {
        public IEnumerable<string> Names { get; set; }
    }
}