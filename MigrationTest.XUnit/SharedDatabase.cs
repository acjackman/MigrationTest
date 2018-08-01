using Xunit;

namespace MigrationTest.XUnit
{
  [Collection("DbTests")]
  public class SharedDatabase : IClassFixture<DatabaseFixture>
  {
    [Fact]
    public void SmokeTest()
    {
      Assert.True(true);
    }
  }
}
