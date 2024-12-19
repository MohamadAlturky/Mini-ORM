using System.Data;
using Microsoft.Data.SqlClient;


namespace Data.Services;

public static class DbConnectionFactory
{
    public static string ConnectionString { get; set; } = "Server=DESKTOP-OO326C9\\SQLEXPRESS01;Database=Test;User id=sa; Password=A@123456789; MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Trusted_Connection=false";
    public static IDbConnection GetSqlServerConnection
    { 
        get
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}