using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using MysqlOp;
using MySql.Data.MySqlClient;

namespace EntityManager
{
    public class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance()
        {
            if (instance == null)
            {
                instance = new UserManager();
                return instance;
            }
            else
            {
                return instance;
            }
        }
        private Dictionary<int, User> userDict = new Dictionary<int, User>();

        public User GetUserById(int id)
        {
            if(userDict.ContainsKey(id))
            {
                return userDict[id];
            }
            else
            {
                User user = User.GetFromMySql(id);
                if(userDict.Count < 5000)
                {
                    userDict[id] = user;
                }
                return user;
            }
        }
    }
}
