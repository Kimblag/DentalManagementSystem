using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.LifecycleStatusTests
{
    public class LifecycleStatus_ReactivateShould
    {
        [Fact]
        public void Reactivate_WhenInactive_ShouldBecomeActive()
        {
            var status = new LifecycleStatus();
            status.Deactivate();

            status.Reactivate();

            Assert.True(status.IsActive);
            Assert.False(status.IsInactive);
        }

        [Fact]
        public void Reactivate_WhenAlreadyActive_ShouldThrowException()
        {
            var status = new LifecycleStatus();

            Assert.Throws<InvalidStatusTransitionException>(() => status.Reactivate());
        }
    }
}
