using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        /*private AppDbContext context;
        public RegisterController(AppDbContext appDbContext)
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

                return RedirectToAction("Read");
            }
            else
                return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var pc = context.Register.Where(a => a.Id == id).FirstOrDefault();
            context.Remove(pc);
            await context.SaveChangesAsync();

            return RedirectToAction("Read");
        }*/
    }
}
