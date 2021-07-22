using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAppT.Controllers
{
    public class PartyController : Controller
    {
        public IActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entry(string name, string membership)
        {
            if (string.IsNullOrEmpty(name))
                ModelState.AddModelError("name", "Please enter your name");
            if (string.IsNullOrEmpty(membership))
                ModelState.AddModelError("membership", "Please enter your membership");

            if (ModelState.IsValid)
            {
                string nm = name + "," + membership;
                return View((object)nm);
            }
            else
                return BadRequest(ModelState);
        }
    }
}
