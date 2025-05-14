using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using ConvenienceStoreManager.UI;

namespace ConvenienceStoreManager
{
    static class Program
    {
        private static string connectionStrings = "Data Source=STEVE-jonh;Initial Catalog=ConvenienceStoreDB;Integrated Security=True;Encrypt=False";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kiểm tra kết nối đến cơ sở dữ liệu trước khi chạy ứng dụng
            if (InitializeDatabase())
            {
                Application.Run(new frmMain());
            }
            else
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Ứng dụng sẽ đóng.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        // Kiểm tra kết nối đến cơ sở dữ liệu
        private static bool InitializeDatabase()
        {
            try
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["ConvenienceStoreDB"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}