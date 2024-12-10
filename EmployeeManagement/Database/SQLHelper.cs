using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace EmployeeManagement.Database;

public class SQLHelper
{
    private SQLHelper()
    {
    }
    public static string ConnectionString = string.Empty;
    private static readonly Lazy<SQLHelper> lazy = new Lazy<SQLHelper>(() => new SQLHelper());
    public static SQLHelper Instance
    {
        get
        {
            return lazy.Value;
        }
    }

    public IDbConnection Connection
    {
        get { return new SqlConnection(ConnectionString); }
    }
    public async Task ExecuteAsync(string storedProcedure, object parameters = null, int commandTimeout = 0)
    {
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                await Connection.QueryAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
            else
            {
                await Connection.QueryAsync(storedProcedure,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }
    }
    public async Task<T> ExecuteAsync<T>(string storedProcedure, string columnName, object parameters = null)
    {
        var ds = await ExecuteProcedureAsync(storedProcedure, parameters);
        if (ds is not null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0].Field<T>(columnName);
        }
        return default(T);
    }
    public async Task<IEnumerable<T>> ExecuteAsync<T>(string storedProcedure, object parameters = null, int commandTimeout = 0)
    {
        IDataReader results;
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                results = await Connection.ExecuteReaderAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
            else
            {
                results = await Connection.ExecuteReaderAsync(storedProcedure,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }

            var ds = ConvertDataReaderToDataSet(results);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables[0]);
            IEnumerable<T> list = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<T>>(JSONString));
            return list;
        }
    }
    public async Task<DataSet> ExecuteProcedureAsync(string storedProcedure, object parameters = null, int commandTimeout = 0)
    {
        IDataReader dataReader = null;
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                dataReader = await Connection.ExecuteReaderAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
            else
            {
                dataReader = await Connection.ExecuteReaderAsync(storedProcedure,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }

            return ConvertDataReaderToDataSet(dataReader);
        }
    }

    /// <summary>
    /// Used when their is output parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="storedProcedure"></param>
    /// <param name="columnName"></param>
    /// <param name="parameters"></param>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public async Task<T> ExecuteProcedureAsync<T>(string storedProcedure, string columnName, object parameters = null, int commandTimeout = 0)
    {
        IDataReader dataReader = null;
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                dataReader = await Connection.ExecuteReaderAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
            else
            {
                dataReader = await Connection.ExecuteReaderAsync(storedProcedure,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }

            var value = parameters as DynamicParameters;
            return value.Get<T>(columnName);
        }
    }
    public async Task<T> ExecuteFunctionAsync<T>(string text, string columnName, object parameters = null, int commandTimeout = 0)
    {
        IDataReader dataReader = null;
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                dataReader = await Connection.ExecuteReaderAsync(text, parameters,
                    commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
            else
            {
                dataReader = await Connection.ExecuteReaderAsync(text,
                    commandType: CommandType.Text, commandTimeout: commandTimeout);
            }

            var ds = ConvertDataReaderToDataSet(dataReader);

            if (ds is not null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0].Field<T>(columnName);
            }
            return default(T);
        }
    }

    //public async Task<PagedResponse<T>> ExecuteProcedureAsync<T>(string storedProcedure, object parameters = null, string columnName = "TotalRecords", int commandTimeout = 0)
    //{
    //    var response = new PagedResponse<T>();
    //    using (IDbConnection dbConnection = Connection)
    //    {
    //        IEnumerable<T> data;
    //        if (parameters != null)
    //        {
    //            data = await Connection.QueryAsync<T>(storedProcedure, parameters,
    //                commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
    //        }
    //        else
    //        {
    //            data = await Connection.QueryAsync<T>(storedProcedure,
    //                commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
    //        }

    //        var value = parameters as DynamicParameters;
    //        var totalRecords = value.Get<int>(columnName);
    //        response.Data = data;
    //        response.TotalRecords = totalRecords;
    //        return response;
    //    }
    //}

    public async Task<int> ExecuteProcedureReturnsRowsAffectedAsync(string storedProcedure, object parameters = null, int commandTimeout = 0)
    {
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                return await Connection.ExecuteAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
            else
            {
                return await Connection.ExecuteAsync(storedProcedure,
                    commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }
    }

    public async Task ExecuteQueryAsync(string sqlText, object parameters = null, int commandTimeout = 0)
    {
        using (IDbConnection dbConnection = Connection)
        {
            if (parameters != null)
            {
                await Connection.ExecuteAsync(sqlText, parameters,
                    commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
            else
            {
                await Connection.ExecuteAsync(sqlText,
                    commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
        }
    }
    private DataSet ConvertDataReaderToDataSet(IDataReader data)
    {
        DataSet ds = new DataSet();
        int i = 0;
        while (!data.IsClosed)
        {
            ds.Tables.Add("Table" + (i + 1));
            ds.EnforceConstraints = false;
            ds.Tables[i].Load(data);
            i++;
        }
        return ds;
    }
}
