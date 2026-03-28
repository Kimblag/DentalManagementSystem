using DentalSystem.Domain.Enums.Specialty;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Domain.Aggregates.Specialty
{
    public class Treatment
    {
        public TreatmentCode Code { get; private set; } = null!;
        public Name Name { get; private set; } = null!;
        public Money BaseCost { get; private set; } = null!;
        public TreatmentStatus Status { get; private set; } // ACTIVE - ARCHIVED

        public string? Description { get; private set; }


        // ctor vacío EF
        protected Treatment() { }


        // ctor para creación
        internal Treatment(TreatmentCode code, Name name, Money baseCost, string? description = null)
        {
            Code = code;
            Name = name;
            BaseCost = baseCost;
            Status = TreatmentStatus.ACTIVE; // active by default

            if (description is not null) Description = description;
        }

        /* BEHAVIOR */

        // renombrar tratamiento
        internal void Rename(Name newName)
        {
            Name = newName;
        }

        // actualizar costo base
        internal void UpdateBaseCost(Money newBaseCost)
        {
            BaseCost = newBaseCost;
        }

        // Cambiar status: Reactivate
        internal void Activate()
        {
            // si el estado actual es activo, no hacer nada
            if (Status == TreatmentStatus.ACTIVE) return;
            Status = TreatmentStatus.ACTIVE;
        }

        // Cambiar status: Deactivate
        internal void Archive()
        {
            if (Status == TreatmentStatus.ARCHIVED) return;
            Status = TreatmentStatus.ARCHIVED;
        }

        // editar descripción, puede establecerse a null o agregar una descripción
        internal void UpdateDescription(string? newDescription = null)
        {
            Description = newDescription;
        }

    }
}
