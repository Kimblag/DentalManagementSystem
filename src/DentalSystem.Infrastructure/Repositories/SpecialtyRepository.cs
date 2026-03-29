using DentalSystem.Application.Interfaces.Repositories;
using DentalSystem.Domain.Aggregates.Specialty;
using DentalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.Infrastructure.Repositories
{
    public sealed class SpecialtyRepository : ISpecialtyRepository
    {

        private readonly DentalSystemDbContext _context;

        // inyectar el contexto
        public SpecialtyRepository(DentalSystemDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Specialty specialty)
        {
            _context.Specialties.Add(specialty);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            return _context.Specialties.AnyAsync(s => s.Name.Value == name);
        }

        public async Task<bool> ExistsTreatmentCodeAsync(string code)
        {
            return await _context.Specialties.SelectMany(s => s.Treatments)
                .AnyAsync(t => t.Code.Value == code);
        }

        public async Task<Specialty?> GetByIdAsync(Guid id)
        {
            // se utiliza "include" para incluir sus tratamientos.
            return await _context.Specialties
                .Include(s => s.Treatments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task UpdateAsync(Specialty specialty)
        {
            _context.Specialties.Update(specialty);
            return Task.CompletedTask;
        }
    }
}
