using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace automacao
{
    class conexao
    {
        public SqlConnection abre_cn()
        {
            try
            {
                string sconn = "";

                //producao
                sconn = @"Data Source=LEONAM\SQLEXPRESS01;Initial Catalog=AluraAutomacao;Persist Security Info=True;User ID=sa;Password=12345678";

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = sconn;
                cn.Open();

                return cn;
            }
            catch (Exception err)
            {
                throw;
                return null;
            }
        }


    }
}
