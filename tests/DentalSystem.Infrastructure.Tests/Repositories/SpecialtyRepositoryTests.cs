using DentalSystem.Domain.Entities;
using DentalSystem.Infrastructure.Persistence;
using DentalSystem.Infrastructure.Persistence.Repositories.Specialties;
using DentalSystem.Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.Infrastructure.Tests.Repositories
{
    public sealed class SpecialtyRepositoryTests
    {
        // Add Specialty with treatments
        [Fact]
        public async Task Add_WhenSpecialtyIsNew_ShouldPersistSpecialtyWithTreatments()
        {
            // arrange
            var connection = DbContextHelper.CreateInMemoryConnection();

            using var context = DbContextHelper.CreateDbContext(
                connection,
                ensureCreated: true
            );

            var repository = new SpecialtyRepository(context);
            var unitOfWork = new UnitOfWork(context);

            var specialty = new Specialty("General Dentistry", [
                  ("Cleaning", 50m, "Teeth cleaning"),
                  ("Check-up", 30m, "Routine dental check")
                ],
                "Basic dental treatments");

            // Act
            repository.Add(specialty);
            await unitOfWork.CommitAsync();

            // Assert

            // create a new db context to avoid tracking illusions
            using var assertContext = DbContextHelper.CreateDbContext(connection);

            var persistedSpecialty = await assertContext.Set<Specialty>()
                .Include(s => s.Treatments)
                .SingleAsync();

            Assert.Equal("General Dentistry", persistedSpecialty.Name.Value);
            Assert.Equal("Basic dental treatments", persistedSpecialty.Description!.Value);
            Assert.True(persistedSpecialty.Status.IsActive);

            Assert.Equal(2, persistedSpecialty.Treatments.Count);

            var cleaning = persistedSpecialty.Treatments
                .Single(t => t.Name.Value == "Cleaning");

            Assert.Equal(50m, cleaning.BaseCost);
            Assert.True(cleaning.Status.IsActive);

            var checkup = persistedSpecialty.Treatments
                .Single(t => t.Name.Value == "Check-up");

            Assert.Equal(30m, checkup.BaseCost);
            Assert.True(checkup.Status.IsActive);

            connection.Dispose();
        }


        [Fact]
        public async Task GetById_WhenSpecialtyExists_ShouldReturnSpecialtyWithTreatments()
        {
            // Arrange
            var connection = DbContextHelper.CreateInMemoryConnection();
            using var context = DbContextHelper.CreateDbContext(connection, true);
            var repository = new SpecialtyRepository(context);
            var unitOfWork = new UnitOfWork(context);

            var specialty = new Specialty("General Dentistry", [
                  ("Cleaning", 50m, "Teeth cleaning"),
                  ("Check-up", 30m, "Routine dental check")
                ],
                "Basic dental treatments");

            repository.Add(specialty);
            await unitOfWork.CommitAsync();

            // Act
            using var assertContext = DbContextHelper.CreateDbContext(connection);
            var assertRepository = new SpecialtyRepository(assertContext);
            Specialty? storedSpecialty = await assertRepository.GetByIdAsync(specialty.SpecialtyId);

            // Assert
            Assert.NotNull(storedSpecialty);
            Assert.Equal("General Dentistry", storedSpecialty.Name.Value);
            Assert.Equal("Basic dental treatments", storedSpecialty.Description!.Value);

            Assert.Equal(2, storedSpecialty.Treatments.Count);
            var cleaning = storedSpecialty.Treatments
                .Single(t => t.Name.Value == "Cleaning");

            Assert.Equal(50m, cleaning.BaseCost);
            Assert.True(cleaning.Status.IsActive);

            var checkup = storedSpecialty.Treatments
                .Single(t => t.Name.Value == "Check-up");

            Assert.Equal(30m, checkup.BaseCost);

            connection.Dispose();
        }


        [Fact]
        public async Task Cascade_WhenDeactivateAnExistingSpecialty_ShouldDeactivateAssociatedTreatments()
        {
            // Arrange
            var connection = DbContextHelper.CreateInMemoryConnection();
            using var context = DbContextHelper.CreateDbContext(connection, true);
            var repository = new SpecialtyRepository(context);
            var unitOfWork = new UnitOfWork(context);

            var specialty = new Specialty("General Dentistry", [
                  ("Cleaning", 50m, "Teeth cleaning"),
                  ("Check-up", 30m, "Routine dental check")
                ],
                "Basic dental treatments");

            repository.Add(specialty);
            await unitOfWork.CommitAsync();

            // Act
            using var actContext = DbContextHelper.CreateDbContext(connection);
            var actRepository = new SpecialtyRepository(actContext);
            var actUnitOfWork = new UnitOfWork(actContext);
            
            Specialty storedSpecialty =
                await actRepository.GetByIdAsync(specialty.SpecialtyId)
                ?? throw new Exception("Specialty not found");

            storedSpecialty.Deactivate();
            await actUnitOfWork.CommitAsync();

            // Assert
            using var assertContext = DbContextHelper.CreateDbContext(connection);

            var persistedSpecialty = await assertContext.Set<Specialty>()
                .Include(s => s.Treatments)
                .SingleAsync();
            Assert.True(persistedSpecialty.Status.IsInactive);

            var cleaning = persistedSpecialty.Treatments
                .Single(t => t.Name.Value == "Cleaning");
            Assert.True(cleaning.Status.IsInactive);

            var checkup = persistedSpecialty.Treatments
                .Single(t => t.Name.Value == "Check-up");
            Assert.True(checkup.Status.IsInactive);

            connection.Dispose();
        }

    }
}
