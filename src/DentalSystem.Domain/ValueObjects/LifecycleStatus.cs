using DentalSystem.Domain.Exceptions.Specialties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidStatusTransitionException("The entity is already inactive.");

            IsActive = false;
        }

        public void Reactivate()
        {
            if (IsActive)
                throw new InvalidStatusTransitionException("The entity is already active.");

            IsActive = true;
        }

        // Equity by value
        public override bool Equals(object? obj)
        {
            return obj is LifecycleStatus other && IsActive == other.IsActive;
        }

        public override int GetHashCode() => IsActive.GetHashCode();
    }
}
