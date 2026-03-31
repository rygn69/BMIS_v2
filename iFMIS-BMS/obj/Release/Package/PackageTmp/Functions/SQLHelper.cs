using System.Data;
using System.Data.SqlClient;

public static class SQLHelper
{
    /// <summary>
    /// Returns datatable from a query
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <param name="TableIndex">Int datatype - Use to specify what table to be fetched</param>
    /// <returns>DataTable</returns>
    public static DataTable GetDataTable(this string Query, string ConnectionString, int TableIndex)
    {
        DataSet ds = new DataSet();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(ds);

            return ds.Tables[TableIndex];
        }
    }

    /// <summary>
    /// Returns datatable with index 0 from a query
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>DataTable</returns>
    public static DataTable GetDataTable(this string Query, string ConnectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(ds);

            return ds.Tables[0];
        }
    }

    /// <summary>
    /// Returns dataset from a query
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>DataSet</returns>
    public static DataSet GetDataSet(this string Query, string ConnectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(ds);

            return ds;
        }
    }

    /// <summary>
    /// Returns an object
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>Object</returns>
    public static object GetValue(this string Query, string ConnectionString)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand com = new SqlCommand(Query, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                return reader.GetValue(0);
            }

            return null;
        }
    }

    /// <summary>
    /// Returns an object
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>Object</returns>
    public static object GetValue(this string Query, string ConnectionString, int ColumnIndex)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand com = new SqlCommand(Query, con);

            return com.ExecuteScalar();
        }
    }

    /// <summary>
    /// Returns true if the query contains one or more rows otherwise false
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>bool</returns>
    public static bool HasRows(this string Query, string ConnectionString)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand com = new SqlCommand(Query, con);
            SqlDataReader reader = com.ExecuteReader();

            return reader.HasRows;
        }
    }

    /// <summary>
    /// Returns top one row from a query
    /// </summary>
    /// <param name="Query">Use for query</param>
    /// <param name="Connection">SqlConnection - Use to open a connection to database</param>
    /// <returns>DataRow</returns>
    public static DataRow TopOne(this string Query, string ConnectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(Query, con);
            da.Fill(ds);

            return ds.Tables[0].Rows[0];
        }
    }
}