using DentalSystem.Domain.Exceptions.Rules.Specialties;

namespace DentalSystem.Domain.ValueObjects
{
    /// <summary>
    /// Represents the immutable operational state of a domain entity.
    /// </summary>
    /// <remarks>
    /// Instances are created through static factory methods. State transitions return 
    /// a new instance, ensuring the value object remains immutable.
    /// </remarks>
    public sealed class LifecycleStatus : IEquatable<LifecycleStatus>
    {
        public bool IsActive { get; }
        public bool IsInactive => !IsActive;

        private LifecycleStatus(bool isActive)
        {
            IsActive = isActive;
        }


        /// <summary>
        /// Creates a <see cref="LifecycleStatus"/> instance in an active state.
        /// </summary>
        /// <returns>A new active status instance.</returns>
        public static LifecycleStatus Active()
            => new(true);


        /// <summary>
        /// Creates a <see cref="LifecycleStatus"/> instance in an inactive state.
        /// </summary>
        /// <returns>A new inactive status instance.</returns>
        public static LifecycleStatus Inactive()
            => new(false);


        /// <summary>
        /// Validates and returns a new inactive state from the current instance.
        /// </summary>
        /// <returns>A new <see cref="LifecycleStatus"/> in an inactive state.</returns>
        /// <exception cref="InvalidStatusTransitionException">Thrown when the status is already inactive.</exception>
        public LifecycleStatus Deactivate()
        {
            if (!IsActive)
                throw new InvalidStatusTransitionException("The entity is already inactive.");

            return Inactive();
        }


        /// <summary>
        /// Validates and returns a new active state from the current instance.
        /// </summary>
        /// <returns>A new <see cref="LifecycleStatus"/> in an active state.</returns>
        /// <exception cref="InvalidStatusTransitionException">Thrown when the status is already active.</exception>
        public LifecycleStatus Reactivate()
        {
            if (IsActive)
                throw new InvalidStatusTransitionException("The entity is already active.");

            return Active();
        }


        /// <summary>
        /// Determines whether the specified object is equal to the current state.
        /// </summary>
        /// <param name="other">The status to compare.</param>
        /// <returns>True if the state values match; otherwise, false.</returns>
        public bool Equals(LifecycleStatus? other)
            => other is not null && IsActive == other.IsActive;


        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the object is a <see cref="LifecycleStatus"/> with the same state.</returns>
        public override bool Equals(object? obj)
            => Equals(obj as LifecycleStatus);


        /// <summary>
        /// Returns the hash code for this status instance.
        /// </summary>
        /// <returns>A hash code based on the active/inactive value.</returns>
        public override int GetHashCode()
            => IsActive.GetHashCode();
    }
}
