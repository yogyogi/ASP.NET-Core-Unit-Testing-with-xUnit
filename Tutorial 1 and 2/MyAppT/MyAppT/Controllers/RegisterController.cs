using Microsoft.AspNetCore.Mvc;
using MyAppT.Models;

namespace MyAppT.Controllers
{
    public class RegisterController : Controller
    {
        private IRegisterRepository context;
        public RegisterController(IRegisterRepository appDbContext)
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
                await context.CreateAsync(register);
                return RedirectToAction("Read");
            }
            else
                return View();
        }

        public async Task<IActionResult> Read()
        {
            var rl = await context.ListAsync();
            return View(rl);
        }

        public async Task<IActionResult> Update(int id)
        {
            Register r = await context.GetByIdAsync(id);
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Register register)
        {
            if (ModelState.IsValid)
            {
                await context.UpdateAsync(register);
                ViewBag.Result = "Success";
            }
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await context.DeleteAsync(id);
            return RedirectToAction("Read");
        }
    }
}
