using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Dapper;
using MigrationTest;

namespace MigrationTest.XUnit
{
  [Collection("DbTests")]
  public class DbRefreshPerTest : DbTest
  {
    public const string TEST_DB = "Data Source=(LocalDb)\\MSSQLLocalDB;Database=MigrationTestUnitTest";

    public DbRefreshPerTest() : base()
    {
    }

    [Fact]
    public void CanCreateALog()
    {
      using (var con = new SqlConnection(TEST_DB))
      {
        con.Open();
        var msg = "created a record";
        con.Execute("INSERT INTO Log (Text) VALUES (@Text)", new { Text = msg });


        var logs = con.Query<Log>("SELECT * FROM Log").AsList();

        Assert.Single(logs);
        Assert.Equal(msg, logs[0].Text);
      }
    }

    [Fact]
    public void StartWithNoLogs()
    {
      using (var con = new SqlConnection(TEST_DB))
      {
        con.Open();
        var logs = con.Query<Log>("SELECT * FROM Log").AsList();

        Assert.Empty(logs);
      }
    }
  }
}
