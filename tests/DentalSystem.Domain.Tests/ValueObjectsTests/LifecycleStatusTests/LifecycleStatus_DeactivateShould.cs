using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;


namespace DentalSystem.Domain.Tests.ValueObjectsTests.LifecycleStatusTests
{
    public class LifecycleStatus_DeactivateShould
    {
        [Fact]
        public void Deactivate_WhenActive_ShouldBecomeInactive()
        {
            var status = LifecycleStatus.Active(); // is active by default

            status = status.Deactivate();

            Assert.True(status.IsInactive);
            Assert.False(status.IsActive);
        }

        [Fact]
        public void Deactivate_WhenAlreadyInactive_ShouldThrowException()
        {
            var status = LifecycleStatus.Active();
            status = status.Deactivate();

            Assert.Throws<InvalidStatusTransitionException>(() => status.Deactivate());
        }
    }
}
