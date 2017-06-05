using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Data;

namespace MysqlOp
{
    public class MysqlManager
    {
        private static MysqlManager instance;
        private MySqlConnection connection;
       
        public static MysqlManager Instance()
        {
            if(instance == null)
            {
                instance = new MysqlManager();
                instance.connection = null;
            }
            return instance;
        }

        public MySqlConnection DefautConnect()
        {
            if (this.connection != null)
            {
                return this.connection;
            }
            string connection = "server=localhost;user id=root;password=root;database=starcomment;";
            try
            {
                this.connection = new MySqlConnection(connection);
                return this.connection;
            }
            catch (Exception e)
            {
                Console.WriteLine("建立数据库连接失败");
                Console.WriteLine(e);
                return null;
            }
        }
        public MySqlConnection Connect(string server,string user,string password,string database)
        {
            if(this.connection != null)
            {
                return this.connection;
            }
            string connection = "server="+server+";user id="+user+";password="+password+";database="+ database +";";
            try
            {
                this.connection = new MySqlConnection(connection);
                return this.connection;
            }
            catch(Exception e)
            {
                Console.WriteLine("建立数据库连接失败");
                Console.WriteLine(e);
                return null;
            }
        }
        

        public int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            if (connection == null)
            {
                Console.WriteLine("请先建立连接");
                return 0;
            }

            
            if (cmdParms.Length == 0)
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    connection.Close();
                    throw e;
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
            }
        }
        public object ExecuteScalar(string SQLString, params MySqlParameter[] cmdParms)
        {
            if (connection == null)
            {
                Console.WriteLine("请先建立连接");
                return null;
            }
            if (cmdParms.Length == 0)
            {

                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Equals(obj, null)) || (Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (MySqlException e)
                {
                    connection.Close();
                    throw e;
                }
            }
            else
            {

                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
            }
        }

        public MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            
            if (cmdParms.Length == 0)
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    MySqlDataReader myReader = cmd.ExecuteReader();
                    return myReader;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return myReader;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
            }
        }
        public void Disconnect()
        {
            try
            {
                connection.Close();
                connection = null;
            }
            catch
            {
                Console.WriteLine("关闭数据库连接错误");
            }
        }
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType; 
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

    }
}
