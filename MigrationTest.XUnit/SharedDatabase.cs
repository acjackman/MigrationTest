using Xunit;

namespace MigrationTest.XUnit
{
  public class SharedDatabase : IClassFixture<DatabaseFixture>
  {
    [Fact]
    public void SmokeTest()
    {
      Assert.True(true);
    }
  }
}
