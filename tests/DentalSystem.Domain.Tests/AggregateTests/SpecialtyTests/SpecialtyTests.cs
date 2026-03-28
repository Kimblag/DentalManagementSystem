using DentalSystem.Domain.Aggregates.Specialty;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Enums.Specialty;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Domain.Tests.AggregateTests.SpecialtyTests
{
    public sealed class SpecialtyTests
    {
        /* Variables para usar por defecto y evitar repetir lo mismo siempre.
         * OJO: KISS 
         */
        // VOs
        private readonly Name _defaultSpecialtyName = new("Orthodontics");
        private readonly Name _defaultTreatmentName = new("General Cleaning");
        private readonly Money _defaultTreatmentCost = new(10000, "ARS");
        private readonly TreatmentCode _defaultTreatmentCode = new("01.01");

        // fábrica de estados

        // Crear Specialty en estado DRAFT
        private Specialty CreateDraftSpecialty()
        {
            return new Specialty(_defaultSpecialtyName, "Test Description");
        }

        // Crear Spacialty con 1 tratamiento
        private Specialty CreateSpecialtyWithOneTreatment()
        {
            Specialty specialty = new(_defaultSpecialtyName, "Test description");
            specialty.AddTreatment(_defaultTreatmentCode, _defaultTreatmentName, _defaultTreatmentCost);

            return specialty;
        }

        // Crear Specialty con N cantidad de tratamientos
        private Specialty CreateSpecialtyWithMultipleTreatments()
        {
            Specialty specialty = new(_defaultSpecialtyName, "Test description");

            for (int i = 0; i < 3; i++)
            {
                specialty.AddTreatment(
                    new TreatmentCode($"01.0{i}"),
                    new Name($"Treatment {i}"),
                    _defaultTreatmentCost);
            }

            return specialty;
        }

        // Crear Specialty en estado ARCHIVED
        private Specialty CreateArchivedSpecialty()
        {
            Specialty specialty = new(_defaultSpecialtyName, "Test description");

            for (int i = 0; i < 3; i++)
            {
                specialty.AddTreatment(
                    new TreatmentCode($"01.0{i}"),
                    new Name($"Treatment {i}"),
                    _defaultTreatmentCost);
            }

            specialty.Archive();
            return specialty;
        }

        // Crear Specialty con n tratamientos (uno con estado ARCHIVED)
        private Specialty CreateSpecialtyWithOneArchivedTreatment()
        {
            Specialty specialty = new(_defaultSpecialtyName, "Test description");

            for (int i = 0; i < 3; i++)
            {
                specialty.AddTreatment(
                    new TreatmentCode($"01.0{i}"),
                    new Name($"Treatment {i}"),
                    _defaultTreatmentCost);
            }

            specialty.ArchiveTreatment(new TreatmentCode("01.01"));
            return specialty;
        }


        /* Happy path */
        [Fact]
        public void Create_ValidData_ShouldSucceedWithInitialDraftState()
        {
            // Arrange
            string description = "Specialty description";
            // Act
            Specialty specialty = new(_defaultSpecialtyName, description);

            // Assert
            Assert.NotEqual(Guid.Empty, specialty.Id);
            Assert.Equal(_defaultSpecialtyName, specialty.Name);
            Assert.Equal(description, specialty.Description);
            Assert.Equal(description, specialty.Description);
            Assert.Equal(SpecialtyStatus.DRAFT, specialty.Status);
        }

        [Fact]
        public void Rename_ValidName_ShouldChangeName()
        {
            //Arrange
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            Name newName = new("Orthodontics Sp.");
            //Act
            specialty.Rename(newName);

            //Assert
            Assert.Equal(newName, specialty.Name);
        }

        [Fact]
        public void Archive_ActiveSpecialty_ShouldChangeTreatmentAndSpecialtyStatus()
        {
            // Arrange
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            SpecialtyStatus statusSnapshot = specialty.Status;

            // Act
            specialty.Archive();

            // Assert
            Assert.Equal(SpecialtyStatus.ACTIVE, statusSnapshot);// antes 
            Assert.Equal(SpecialtyStatus.ARCHIVED, specialty.Status); // después
            Assert.All(specialty.Treatments,
                t => Assert.Equal(TreatmentStatus.ARCHIVED, t.Status));
        }

        [Fact]
        public void Archive_DraftSpecialty_ShouldChangeSpecialtyStatus()
        {
            // Arrange
            Specialty specialty = CreateDraftSpecialty();
            SpecialtyStatus statusSnapshot = specialty.Status;

            // Act
            specialty.Archive();

            // Assert
            Assert.Equal(SpecialtyStatus.DRAFT, statusSnapshot);
            Assert.Equal(SpecialtyStatus.ARCHIVED, specialty.Status);
        }


        [Fact]
        public void Archive_ArchivedSpecialty_ShouldNotChange()
        {
            // Arrange
            Specialty specialty = CreateArchivedSpecialty();
            SpecialtyStatus statusSnapshot = specialty.Status;

            // Act
            specialty.Archive();

            // Assert
            Assert.Equal(SpecialtyStatus.ARCHIVED, statusSnapshot);
            Assert.Equal(SpecialtyStatus.ARCHIVED, specialty.Status);
        }


        [Fact]
        public void Active_ArchivedSpecialty_ShouldChangeStatus()
        {
            // Arrange
            Specialty specialty = CreateArchivedSpecialty();
            SpecialtyStatus statusSnapshot = specialty.Status;

            // Act
            specialty.Activate();

            // Assert
            Assert.Equal(SpecialtyStatus.ARCHIVED, statusSnapshot);
            Assert.Equal(SpecialtyStatus.ACTIVE, specialty.Status);
        }

        [Fact]
        public void Active_ActiveSpecialty_ShouldNotChangeStatus()
        {
            // Arrange
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            SpecialtyStatus statusSnapshot = specialty.Status;

            // Act
            specialty.Activate();

            // Assert
            Assert.Equal(SpecialtyStatus.ACTIVE, statusSnapshot);
            Assert.Equal(SpecialtyStatus.ACTIVE, specialty.Status);
        }

        /* tests de gestión de tratamientos */
        [Fact]
        public void AddTreatment_DraftSpecialty_ShouldActiveSpecialtyAndAddTreatment()
        {
            // Arrange
            Specialty specialty = CreateDraftSpecialty();
            SpecialtyStatus statusSnapshot = specialty.Status;
            TreatmentCode treatmentCode = new("01.01");

            // Act
            specialty.AddTreatment(treatmentCode, _defaultTreatmentName, _defaultTreatmentCost);

            // Assert
            Assert.Equal(SpecialtyStatus.DRAFT, statusSnapshot);
            Assert.Equal(SpecialtyStatus.ACTIVE, specialty.Status);
            Assert.Contains(specialty.Treatments, t => t.Code == treatmentCode);
        }


        [Fact]
        public void AddTreatment_ActiveSpecialty_ShouldAddNewTreatment()
        {
            // Arrange
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            TreatmentCode treatmentCode = new("01.03");
            Name newTreatmentName = new("Brackets");

            // Act
            specialty.AddTreatment(treatmentCode, newTreatmentName, _defaultTreatmentCost);

            // Assert
            Assert.Equal(2, specialty.Treatments.Count);
            Assert.Contains(specialty.Treatments, t => t.Code == treatmentCode);
        }

        [Fact]
        public void RenameTreatment_ActiveTreatment_ShouldChangeName()
        {
            // Arrange
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            Name oldName = specialty.Treatments.First().Name;
            Name newTreatmentName = new("Brackets");

            // Act
            specialty.RenameTreatment(_defaultTreatmentCode, newTreatmentName);

            // Assert
            Assert.Equal(_defaultTreatmentName, oldName); // old name
            Assert.Contains(specialty.Treatments, t => t.Name == newTreatmentName); // new name
        }


        [Fact]
        public void UpdateTreatmentBaseCost_ActiveTreatment_ShouldChangeCost()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            Money newCost = new(20000, "ARS");

            specialty.UpdateTreatmentBaseCost(_defaultTreatmentCode, newCost);

            Assert.Contains(specialty.Treatments, t => t.BaseCost == newCost);
        }

        [Fact]
        public void ArchiveTreatment_WithMultipleActiveTreatments_ShouldArchiveSpecificTreatment()
        {
            Specialty specialty = CreateSpecialtyWithMultipleTreatments();
            TreatmentCode targetCode = new("01.01");

            specialty.ArchiveTreatment(targetCode);

            Assert.Contains(specialty.Treatments, t => t.Code == targetCode && t.Status == TreatmentStatus.ARCHIVED);
        }

        [Fact]
        public void ArchiveTreatment_AlreadyArchived_ShouldNotChangeStatus()
        {
            Specialty specialty = CreateSpecialtyWithOneArchivedTreatment();
            TreatmentCode targetCode = new("01.01");

            specialty.ArchiveTreatment(targetCode);

            Assert.Contains(specialty.Treatments, t => t.Code == targetCode && t.Status == TreatmentStatus.ARCHIVED);
        }

        [Fact]
        public void ActivateTreatment_ArchivedTreatment_ShouldActivate()
        {
            Specialty specialty = CreateSpecialtyWithOneArchivedTreatment();
            TreatmentCode targetCode = new("01.01");

            specialty.ActivateTreatment(targetCode);

            Assert.Contains(specialty.Treatments, t => t.Code == targetCode && t.Status == TreatmentStatus.ACTIVE);
        }

        [Fact]
        public void ActivateTreatment_AlreadyActive_ShouldNotChangeStatus()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();

            specialty.ActivateTreatment(_defaultTreatmentCode);

            Assert.Contains(specialty.Treatments, t => t.Code == _defaultTreatmentCode && t.Status == TreatmentStatus.ACTIVE);
        }


        /* Error path */
        [Fact]
        public void Rename_ArchivedSpecialty_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateArchivedSpecialty();
            Name newName = new("Endodontics");

            Assert.Throws<DomainRuleException>(() => specialty.Rename(newName));
        }

        [Fact]
        public void AddTreatment_ArchivedSpecialty_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateArchivedSpecialty();
            TreatmentCode newCode = new("99.99");

            Assert.Throws<DomainRuleException>(() => specialty.AddTreatment(newCode, _defaultTreatmentName, _defaultTreatmentCost));
        }

        [Fact]
        public void AddTreatment_DuplicateCode_ShouldThrowDomainConflictException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();

            Assert.Throws<DomainConflictException>(() => specialty.AddTreatment(_defaultTreatmentCode, _defaultTreatmentName, _defaultTreatmentCost));
        }

        [Fact]
        public void RenameTreatment_ArchivedSpecialty_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateArchivedSpecialty();
            Name newName = new("New Name");

            Assert.Throws<DomainRuleException>(() => specialty.RenameTreatment(new TreatmentCode("01.00"), newName));
        }

        [Fact]
        public void RenameTreatment_TreatmentNotFound_ShouldThrowDomainNotFoundException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            TreatmentCode notFoundCode = new("99.99");
            Name newName = new("New Name");

            Assert.Throws<DomainNotFoundException>(() => specialty.RenameTreatment(notFoundCode, newName));
        }

        [Fact]
        public void UpdateTreatmentBaseCost_ArchivedSpecialty_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateArchivedSpecialty();
            Money newCost = new(20000, "ARS");

            Assert.Throws<DomainRuleException>(() => specialty.UpdateTreatmentBaseCost(new TreatmentCode("01.00"), newCost));
        }

        [Fact]
        public void UpdateTreatmentBaseCost_TreatmentNotFound_ShouldThrowDomainNotFoundException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            TreatmentCode notFoundCode = new("99.99");
            Money newCost = new(20000, "ARS");

            Assert.Throws<DomainNotFoundException>(() => specialty.UpdateTreatmentBaseCost(notFoundCode, newCost));
        }

        [Fact]
        public void ArchiveTreatment_LastActiveTreatment_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();

            Assert.Throws<DomainRuleException>(() => specialty.ArchiveTreatment(_defaultTreatmentCode));
        }

        [Fact]
        public void ArchiveTreatment_TreatmentNotFound_ShouldThrowDomainNotFoundException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            TreatmentCode notFoundCode = new("99.99");

            Assert.Throws<DomainNotFoundException>(() => specialty.ArchiveTreatment(notFoundCode));
        }

        [Fact]
        public void ActivateTreatment_ArchivedSpecialty_ShouldThrowDomainRuleException()
        {
            Specialty specialty = CreateArchivedSpecialty();

            Assert.Throws<DomainRuleException>(() => specialty.ActivateTreatment(new TreatmentCode("01.00")));
        }

        [Fact]
        public void ActivateTreatment_TreatmentNotFound_ShouldThrowDomainNotFoundException()
        {
            Specialty specialty = CreateSpecialtyWithOneTreatment();
            TreatmentCode notFoundCode = new("99.99");

            Assert.Throws<DomainNotFoundException>(() => specialty.ActivateTreatment(notFoundCode));
        }

    }
}
