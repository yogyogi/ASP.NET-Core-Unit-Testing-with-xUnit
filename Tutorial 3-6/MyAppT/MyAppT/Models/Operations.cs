using Microsoft.EntityFrameworkCore;

namespace MyAppT.Models
{
    public interface IRegisterRepository
    {
        Task<Register> GetByIdAsync(int id);
        Task<List<Register>> ListAsync();
        Task CreateAsync(Register register);
        Task UpdateAsync(Register register);
        Task DeleteAsync(int id);
    }

    public class RegisterRepository : IRegisterRepository
    {
        private readonly AppDbContext context;

        public RegisterRepository(AppDbContext dbContext)
        {
            context = dbContext;
        }

        public Task<Register> GetByIdAsync(int id)
        {
            return context.Register.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<List<Register>> ListAsync()
        {
            return context.Register.ToListAsync();
        }

        public Task CreateAsync(Register register)
        {
            context.Register.Add(register);
            return context.SaveChangesAsync();
        }

        public Task UpdateAsync(Register register)
        {
            context.Entry(register).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var r = await GetByIdAsync(id);
            context.Remove(r);
            await context.SaveChangesAsync();
        }
    }
}
