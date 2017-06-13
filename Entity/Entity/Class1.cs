using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysqlOp;
using MySql.Data.MySqlClient;

namespace Entity
{
    public class User
    {
        public int id;
        public string name;
        public string password;
        public int manageAreaId;
        public static User CreateUser(string name,string password,int manageAreaId)
        {
            MysqlManager.Instance().DefautConnect();
            MySqlDataReader reader = MysqlManager.Instance().ExecuteReader("SELECT count(*) FROM muser;");
            int num = 0;
            while (reader.Read())
            {
                num = (int)reader[0];
            }
            User user = new User();
            user.id = num + 1;
            user.name = name;
            user.password = password;
            user.manageAreaId = manageAreaId;
            return user;
        }
        public void UpdateMysqlValue()
        {
            string cmd = "UPDATE muser SET userId=" + id.ToString() + ",userName='" + name + "',password='" + password + "',manageAraeId=" + manageAreaId.ToString() + "; WHERE userId=" + id.ToString() + ";";
            MySqlConnection connection = MysqlManager.Instance().DefautConnect();
            MysqlManager.Instance().ExecuteNonQuery(cmd);
        }

        public static User GetFromMySql(int id)
        {
            MysqlManager.Instance().DefautConnect();
            MySqlDataReader reader = MysqlManager.Instance().ExecuteReader("SELECT * FROM muser WHERE userId = " + id.ToString() + ";");
            User user = new User();
            while (reader.Read())
            {
                user.id = (int)reader[0];
                user.name = (string)reader[1];
                user.password = (string)reader[2];
                user.manageAreaId = (int)reader[3];
            }
            return user;
        }

        public static User GetFromMySql(string name)
        {
            MysqlManager.Instance().DefautConnect();
            MySqlDataReader reader = MysqlManager.Instance().ExecuteReader("SELECT * FROM muser WHERE userName = " + name + ";");
            User user = new User();
            while (reader.Read())
            {
                user.id = (int)reader[0];
                user.name = (string)reader[1];
                user.password = (string)reader[2];
                user.manageAreaId = (int)reader[3];
            }
            return user;
        }
    }

    public class Comment
    {
        public int id;
        public int userId;
        public int areaId;
        public string text;

        public void UpdateMysqlValue()
        {
            text.Replace("\'", "\\\'");
            string cmd = "UPDATE mcomment SET id=" + id.ToString() + ",userId=" + userId + ",areaId=" + areaId + ",text='" + text + "'; WHERE id=" + id.ToString() + ";";
            MySqlConnection connection = MysqlManager.Instance().Connect("localhost", "root", "root", "starcomment");
            MysqlManager.Instance().ExecuteNonQuery(cmd);
        }

        public static Comment GetFromMySql(int id)
        {
            MysqlManager.Instance().DefautConnect();
            MySqlDataReader reader = MysqlManager.Instance().ExecuteReader("SELECT * FROM mcomment WHERE id = " + id.ToString() + ";");
            Comment comment = new Comment();
            while (reader.Read())
            {
                comment.id = (int)reader[0];
                comment.userId = (int)reader[1];
                comment.areaId = (int)reader[2];
                comment.text = (string)reader[3];
            }
            return comment;
        }
    }
}
