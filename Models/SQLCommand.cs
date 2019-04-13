using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
//const string ConnectionString = "Data Source=118.89.54.249:1521/xe;user id=system;password=oracle;";
//数据库连接字符串已写于配置文件Web.config中，此处仅作展示用
namespace SimpleSQLCommand
{
    //检查表中某列是否存在某数据，存在则返回true
    class Check_If_Exist
    {

        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;
        public bool TYPE_STRING(string TableName, string ColumnName, string Value)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            string sql = "select " + ColumnName + " from " + TableName + " where " + ColumnName + " = :Value";
            OracleParameter[] prm = new OracleParameter[1];

            OracleCommand cmd = new OracleCommand(sql, conn);

            prm[0] = cmd.Parameters.Add("Value", OracleDbType.Varchar2, Value, ParameterDirection.Input);

            if (cmd.ExecuteScalar() != null)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }
    }
    class Create_New_User
    {
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;
        public void Create(string user_name, string user_password, int authority)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand("", conn);
            conn.Open();
            cmd.CommandText = "select max(USER_NUMBER) from USER_IDENTITY";
            string u = cmd.ExecuteScalar().ToString();
            int uid = Convert.ToInt32(u) + 1;
            conn.Close();
            Insert_Into insert = new Insert_Into();
            insert.TABLE_USER_IDENTITY(uid, user_name, user_password, authority);
            insert.TABLE_USER_INFORMATION(uid);
            return;
        }
    }

    class Get_Information
    {
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

        public EBookDB.Models.UserInformation USER_INFORMATION_BYID(int u_id)
        {

            EBookDB.Models.UserInformation user_info = new EBookDB.Models.UserInformation();
            user_info.Number = u_id;
            user_info.NickName = "";
            user_info.Sex = "";
            user_info.Location = "";
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand("", conn);
            conn.Open();
            string sql_nickname = "select USER_NICK_NAME from USER_INFORMATION where USER_NUMBER = :u_id";
            string sql_sex = "select USER_SEX from USER_INFORMATION where USER_NUMBER = :u_id";
            string sql_location = "select USER_LOCATION from USER_INFORMATION where USER_NUMBER = :u_id";
            OracleParameter[] prm = new OracleParameter[1];
            prm[0] = cmd.Parameters.Add("u_id", OracleDbType.Decimal, u_id, ParameterDirection.Input);
            cmd.CommandText = sql_nickname;
            if (cmd.ExecuteScalar() != null)
            {
                user_info.NickName = cmd.ExecuteScalar().ToString();
            }
            cmd.CommandText = sql_sex;
            if (cmd.ExecuteScalar() != null)
            {
                user_info.Sex = cmd.ExecuteScalar().ToString();
            }
            cmd.CommandText = sql_location;
            if (cmd.ExecuteScalar() != null)
            {
                user_info.Location = cmd.ExecuteScalar().ToString();
            }

            return user_info;
        }
    }
    class Update
    {
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;
        public void TABLE_USER_INFORMATION(int user_number, string nick_name, string sex, string location)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand("", conn);
            OracleParameter[] prm = new OracleParameter[4];
            cmd.CommandText = "update USER_INFORMATION set USER_NICK_NAME = :user_nickname ,USER_SEX = :user_sex ,USER_LOCATION = :user_location where USER_NUMBER = "
                + user_number.ToString();       
            //用预备语句传入用户ID号int值会报错ORA-01722，直接连接字符串没有问题，原因未知
            //用户ID号由系统生成不由用户手动输入，因此不存在注入式攻击风险
            //prm[3] = cmd.Parameters.Add("user_number", OracleDbType.Decimal, user_number, ParameterDirection.Input);
            prm[0] = cmd.Parameters.Add("user_nickname", OracleDbType.Varchar2, nick_name, ParameterDirection.Input);
            prm[1] = cmd.Parameters.Add("user_sex", OracleDbType.Varchar2, sex, ParameterDirection.Input);
            prm[2] = cmd.Parameters.Add("user_location", OracleDbType.Varchar2, location, ParameterDirection.Input);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    class Insert_Into
    {
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;
        public void TABLE_USER_IDENTITY(int user_number, string user_name, string user_password, int authority)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand("", conn);
            OracleParameter[] prm = new OracleParameter[4];
            cmd.CommandText = "insert into USER_IDENTITY values(:u_id,:user_name,:user_password,:authority)";
            prm[0] = cmd.Parameters.Add("u_id", OracleDbType.Decimal, user_number, ParameterDirection.Input);
            prm[1] = cmd.Parameters.Add("user_name", OracleDbType.Varchar2, user_name, ParameterDirection.Input);
            prm[2] = cmd.Parameters.Add("user_password", OracleDbType.Varchar2, user_password, ParameterDirection.Input);
            prm[3] = cmd.Parameters.Add("authority", OracleDbType.Decimal, authority, ParameterDirection.Input);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void TABLE_USER_INFORMATION(int user_number)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            conn.Open();
            OracleCommand cmd = new OracleCommand("", conn);
            OracleParameter[] prm = new OracleParameter[4];
            cmd.CommandText = "insert into USER_INFORMATION values(:u_id,:user_nick_name,:user_sex,:user_location)";
            prm[0] = cmd.Parameters.Add("u_id", OracleDbType.Decimal, user_number, ParameterDirection.Input);
            prm[1] = cmd.Parameters.Add("user_nick_name", OracleDbType.Varchar2, GenerateRadomString(8), ParameterDirection.Input);
            prm[2] = cmd.Parameters.Add("user_sex", OracleDbType.Varchar2, "none", ParameterDirection.Input);
            prm[3] = cmd.Parameters.Add("user_location", OracleDbType.Decimal, null, ParameterDirection.Input);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private string GenerateRadomString(int size)
        {
            int rep = 0;
            string str = "新用户";
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < size; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
    }

    class Login_
    {
        readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;
        public int ByIdentity(string user_name, string user_password)
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand("", conn);
            cmd.CommandText = "select user_number from USER_IDENTITY where user_name = :user_name and user_password = :user_password";
            OracleParameter[] prm = new OracleParameter[2];
            prm[0] = cmd.Parameters.Add("user_name", OracleDbType.Varchar2, user_name, ParameterDirection.Input);
            prm[1] = cmd.Parameters.Add("user_password", OracleDbType.Varchar2, user_password, ParameterDirection.Input);

            conn.Open();
            if (cmd.ExecuteScalar() != null)
            {
                string u = cmd.ExecuteScalar().ToString();
                int u_id = Convert.ToInt32(u);
                return u_id;
            }
            return -1;


        }
    }
}