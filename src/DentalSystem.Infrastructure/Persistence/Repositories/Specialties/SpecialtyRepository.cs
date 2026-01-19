using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace DentalSystem.Infrastructure.Persistence.Repositories.Specialties
{
    public class SpecialtyRepository(DentalSystemDbContext context) : ISpecialtyRepository
    {
        private readonly DentalSystemDbContext _context = context;

        public void Add(Specialty specialty)
        {
            _context.Set<Specialty>().Add(specialty);
        }

        public async Task<Specialty?> GetByIdAsync(Guid specialtyId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Specialty>()
                .Include(s => s.Treatments) // indicates to include its treatments (backing field, not public navigation)
                .FirstOrDefaultAsync(
                    s => s.SpecialtyId == specialtyId, 
                    cancellationToken);
        }

    }
}
