using System.Web.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace EBookDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            string constr = "Data Source=118.89.54.249:1521/xe;user id=system;password=oracle;";
            OracleConnection con = new OracleConnection(constr);
            con.Open();

            OracleParameter[] prm = new OracleParameter[4];

            // Create OracleParameter objects through OracleParameterCollection
            OracleCommand cmd = con.CreateCommand();

            int U_ID = 200001;
            string username = "usnm";
            string password = "pswd";
            int AUTHORITY = 5;

            prm[0] = cmd.Parameters.Add(":u_id", OracleDbType.Decimal,
                U_ID, ParameterDirection.Input);
            prm[1] = cmd.Parameters.Add(":user_name", OracleDbType.Varchar2,
                username, ParameterDirection.Input);
            prm[2] = cmd.Parameters.Add(":user_password", OracleDbType.Varchar2,
                password, ParameterDirection.Input);
            prm[3] = cmd.Parameters.Add(":authority", OracleDbType.Decimal, 
                AUTHORITY, ParameterDirection.Input);
            cmd.CommandText =
                "insert into users_ values(:u_id, :user_name, :user_password, :authority)";
            cmd.ExecuteNonQuery();
            con.Close();

            return View();
        }
    }
}