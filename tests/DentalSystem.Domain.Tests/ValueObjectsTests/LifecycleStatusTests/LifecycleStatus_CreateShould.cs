using DentalSystem.Domain.ValueObjects;


namespace DentalSystem.Domain.Tests.ValueObjectsTests.LifecycleStatusTests
{
    public class LifecycleStatus_CreateShould
    {
        [Fact]
        public void Create_WhenCalled_ShouldBeActiveByDefault()
        {
            // Act
            var status = LifecycleStatus.Active();

            // Assert
            Assert.True(status.IsActive);
            Assert.False(status.IsInactive);
        }
    }
}
