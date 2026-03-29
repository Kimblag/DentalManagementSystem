using DentalSystem.Domain.Enums.Specialty;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Domain.Aggregates.Specialty
{
    public class Specialty
    {
        public Guid Id { get; private set; }
        public Name Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public SpecialtyStatus Status { get; private set; }

        // El uso de listas rompen el encapsulamiento porque cualquiera puede modificarlas,
        // opto por el uso de dos campos: uno privado para tener la data real, uno público para exponer la data de forma segura
        private readonly List<Treatment> _treatments = new();
        public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();


        // constructor vacío EF Core
        protected Specialty()
        {

        }

        // constructor público para creación
        public Specialty(Name name, string? description = null)
        {
            ValidateDescription(description);

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Status = SpecialtyStatus.DRAFT; // inicia en draft hasta que se le agregue un tratamiento y se active
        }

        /* BEHAVIORS */

        // renombrar especialidad
        public void Rename(Name newName)
        {
            EnsureActive();

            // como ya viene validado por el VO, simplemente asigno.
            Name = newName;
        }

        // Actualizar descripción
        public void UpdateDescription(string? newDescription = null)
        {
            EnsureActive();
            ValidateDescription(newDescription);

            // puede ser un valor nulo o una cadena, no hace falta validar.
            Description = newDescription;
        }

        // Reactivar especialidad
        public void Activate()
        {
            if (Status == SpecialtyStatus.ACTIVE) return;

            // Activar sus tratamientos asociados
            _treatments.ForEach(t => t.Activate());

            // Activar specialty
            Status = SpecialtyStatus.ACTIVE;
        }

        // Archivar especialidad
        public void Archive()
        {
            if (Status == SpecialtyStatus.ARCHIVED) return;

            // Archivar sus tratamientos asociados
            _treatments.ForEach(t => t.Archive());

            // Archivar Specialty
            Status = SpecialtyStatus.ARCHIVED;
        }


        /* TREATMENT BEHAVIORS */

        //Agregar tratamiento
        public void AddTreatment(TreatmentCode treatmentCode, Name treatmentName,
            Money baseCost, string? description = null)
        {
            EnsureActive();

            // buscar si ya tenemos el código en el listado
            if (_treatments.Any(t => t.Code == treatmentCode))
                throw new DomainConflictException($"The treatment with code {treatmentCode.Value} already exist.");

            // agregar el tratamiento a la lista.
            // La clase Treatment sólo puede instanciarse desde Specialty
            _treatments.Add(new Treatment(treatmentCode, treatmentName, baseCost, description));

            // si se acaba de crear la especialidad (DRAFT) se debe activar al agregar el tratamiento
            if (Status == SpecialtyStatus.DRAFT) Activate();
        }


        //Renombrar tratamiento
        public void RenameTreatment(TreatmentCode treatmentCode, Name newName)
        {
            EnsureActive();

            // buscar treatment
            Treatment? treatmentToUpdate = _treatments.Find(t => t.Code == treatmentCode)
                ?? throw new DomainNotFoundException($"The treatment with code {treatmentCode.Value} not found.");

            // validar si el tratamiento está archivado: NO debe dejar modificar
            if (treatmentToUpdate.Status == TreatmentStatus.ARCHIVED)
                throw new DomainRuleException("Cannot modify an archived treatment.");

            // Renombrar
            treatmentToUpdate.Rename(newName);
        }

        // Actualizar base cost
        public void UpdateTreatmentBaseCost(TreatmentCode treatmentCode, Money newBaseCost)
        {
            EnsureActive();

            // buscar treatment
            Treatment? treatmentToUpdate = _treatments.Find(t => t.Code == treatmentCode)
               ?? throw new DomainNotFoundException($"The treatment with code {treatmentCode.Value} not found.");

            // validar si el tratamiento está archivado: NO debe dejar modificar
            if (treatmentToUpdate.Status == TreatmentStatus.ARCHIVED)
                throw new DomainRuleException("Cannot modify an archived treatment.");

            treatmentToUpdate.UpdateBaseCost(newBaseCost);
        }

        //Cambiar status: Reactivate
        public void ActivateTreatment(TreatmentCode treatmentCode)
        {
            EnsureActive();

            // buscar treatment
            Treatment? treatmentToUpdate = _treatments.Find(t => t.Code == treatmentCode)
               ?? throw new DomainNotFoundException($"The treatment with code {treatmentCode.Value} not found.");
            treatmentToUpdate.Activate();
        }

        //Cambiar status: Archive
        public void ArchiveTreatment(TreatmentCode treatmentCode)
        {
            EnsureActive();

            // buscar treatment
            Treatment? treatmentToUpdate = _treatments.Find(t => t.Code == treatmentCode)
               ?? throw new DomainNotFoundException($"The treatment with code {treatmentCode.Value} not found.");

            // si ya está archivado, no pasa nada. IDEMPOTENCIA!!
            if (treatmentToUpdate.Status == TreatmentStatus.ARCHIVED) return;

            // validar si el tratamiento a inactivar es el último activo de la lista:
            // Especialidad sin tratamiento no puede existir
            if (_treatments.Count(t => t.Status == TreatmentStatus.ACTIVE) == 1)
                throw new DomainRuleException("Cannot archive the last active treatmend of the specialty.");

            treatmentToUpdate.Archive();
        }

        //Actualizar descripción
        public void UpdateTreatmentDescription(TreatmentCode treatmentCode, string? newDescription = null)
        {
            EnsureActive();

            // buscar treatment
            Treatment? treatmentToUpdate = _treatments.Find(t => t.Code == treatmentCode)
               ?? throw new DomainNotFoundException($"The treatment with code {treatmentCode.Value} not found.");

            // validar si el tratamiento está archivado: NO debe dejar modificar
            if (treatmentToUpdate.Status == TreatmentStatus.ARCHIVED)
                throw new DomainRuleException("Cannot modify an archived treatment.");
            ValidateDescription(newDescription);
            treatmentToUpdate.UpdateDescription(newDescription);
        }

        /* Helpers */
        private void EnsureActive()
        {
            // Validar si está archivada: no debe permitir modificar
            if (Status == SpecialtyStatus.ARCHIVED)
                throw new DomainRuleException("Cannot modify an archived specialty.");
        }

        private void ValidateDescription(string? description = null)
        {
            if (description is not null && description.Length > 250)
                throw new DomainValidationException("The description must have a maximum of 250 characters.");

        }
    }
}
