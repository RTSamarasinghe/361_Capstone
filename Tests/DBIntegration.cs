using Accessors;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DB_integration;
[TestClass]
public class DBIntegration
{
    private ICustomerAccessor _customerAccessor;
    [TestInitialize]
    public void Setup()
    {
        private const string ConnectionString = @"Server=localhost\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;";
        _customerAccessor = new CustomerAccessor(ConnectionString);
    }
    [TestMethod]
    public void DatabaseConnection_ShouldOpenSuccessfully()
    {
        using var conn = new SqlConnection(@"Server=MSI\SQLEXPRESS;Database=ProjectDB;Trusted_Connection=True;TrustServerCertificate=True;");

        conn.Open();

        Assert.AreEqual(System.Data.ConnectionState.Open, conn.State);
    }
}
