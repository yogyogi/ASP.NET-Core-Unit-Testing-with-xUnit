using Microsoft.AspNetCore.Mvc;
using MyAppT.Models;

namespace MyAppT.Controllers
{
    public class RegistrationController : Controller
    {
        private AppDbContext context;
        public RegistrationController(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Register register)
        {
            if (ModelState.IsValid)
            {
                context.Add(register);
                await context.SaveChangesAsync();

                return RedirectToAction("Read");
            }
            else
                return View();
        }

        public IActionResult Read()
        {
            var register = context.Register.ToList();
            return View(register);
        }

        public IActionResult Update(int id)
        {
            var pc = context.Register.Where(a => a.Id == id).FirstOrDefault();
            return View(pc);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Register register)
        {
            if (ModelState.IsValid)
            {
                context.Update(register);
                await context.SaveChangesAsync();

                ViewBag.Result = "Success";
            }
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var pc = context.Register.Where(a => a.Id == id).FirstOrDefault();
            context.Remove(pc);
            await context.SaveChangesAsync();

            return RedirectToAction("Read");
        }
    }
}
