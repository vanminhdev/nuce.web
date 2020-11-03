using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace nuce.web.manager.Pages
{
    public class NotFoundModel : PageModel
    {
        public void OnGet(string message)
        {
            if(!string.IsNullOrWhiteSpace(message))
            {
                ViewData["Message"] = message;
            }
        }
    }
}
