using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;


namespace DentalSystem.Domain.Tests.ValueObjectsTests.LifecycleStatusTests
{
    public class LifecycleStatus_DeactivateShould
    {
        [Fact]
        public void Deactivate_WhenActive_ShouldBecomeInactive()
        {
            var status = new LifecycleStatus(); // is active by default

            status.Deactivate();

            Assert.True(status.IsInactive);
            Assert.False(status.IsActive);
        }

        [Fact]
        public void Deactivate_WhenAlreadyInactive_ShouldThrowException()
        {
            var status = new LifecycleStatus();
            status.Deactivate();

            Assert.Throws<InvalidStatusTransitionException>(() => status.Deactivate());
        }
    }
}
