using DentalSystem.Domain.Exceptions.Rules.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.LifecycleStatusTests
{
    public class LifecycleStatus_ReactivateShould
    {
        [Fact]
        public void Reactivate_WhenInactive_ShouldBecomeActive()
        {
            var status = LifecycleStatus.Active();
            status = status.Deactivate();

            status = status.Reactivate();

            Assert.True(status.IsActive);
            Assert.False(status.IsInactive);
        }

        [Fact]
        public void Reactivate_WhenAlreadyActive_ShouldThrowException()
        {
            var status = LifecycleStatus.Active();

            Assert.Throws<InvalidStatusTransitionException>(() => status.Reactivate());
        }
    }
}
