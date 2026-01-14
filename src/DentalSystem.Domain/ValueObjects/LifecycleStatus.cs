using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed class LifecycleStatus
    {
        public bool IsActive { get; private set; }
        public bool IsInactive => !IsActive;

        public LifecycleStatus()
        {
            // default active
            IsActive = true;
        }


        internal void Deactivate()
        {
            if (!IsActive)
                throw new InvalidStatusTransitionException("The entity is already inactive.");

            IsActive = false;
        }

        internal void Reactivate()
        {
            if (IsActive)
                throw new InvalidStatusTransitionException("The entity is already active.");

            IsActive = true;
        }

        // Equity by value
        public override bool Equals(object? obj)
        {
            return obj is LifecycleStatus other
                && IsActive == other.IsActive
                && IsInactive == other.IsInactive;
        }

        public override int GetHashCode() => HashCode.Combine(IsActive, IsInactive);
    }
}
