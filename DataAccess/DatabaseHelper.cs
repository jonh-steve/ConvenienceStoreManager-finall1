using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace ConvenienceStoreManager.DataAccess
{
    /// <summary>
    /// Lớp hỗ trợ kết nối và thao tác với cơ sở dữ liệu SQL Server
    /// </summary>
    public class DatabaseHelper
    {
        // Chuỗi kết nối đến SQL Server
        private static string connectionString = ConfigurationManager.ConnectionStrings["ConvenienceStoreDB"].ConnectionString;

        /// <summary>
        /// Lấy đối tượng kết nối đến cơ sở dữ liệu
        /// </summary>
        /// <returns>Đối tượng SqlConnection đã được khởi tạo</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Thực thi câu lệnh SQL không trả về dữ liệu (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="query">Câu lệnh SQL</param>
        /// <param name="parameters">Mảng tham số</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int affectedRows = 0;

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        affectedRows = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi câu lệnh: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return affectedRows;
        }

        /// <summary>
        /// Thực thi câu lệnh SQL và trả về dữ liệu dạng DataTable
        /// </summary>
        /// <param name="query">Câu lệnh SQL</param>
        /// <param name="parameters">Mảng tham số</param>
        /// <returns>DataTable chứa kết quả truy vấn</returns>
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable result = new DataTable();

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            connection.Open();
                            adapter.Fill(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Thực thi câu lệnh SQL và trả về giá trị đơn
        /// </summary>
        /// <param name="query">Câu lệnh SQL</param>
        /// <param name="parameters">Mảng tham số</param>
        /// <returns>Giá trị đầu tiên của dòng đầu tiên trong kết quả</returns>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;

            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();
                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Thực thi câu lệnh SQL với giao tác (transaction)
        /// </summary>
        /// <param name="queries">Danh sách câu lệnh SQL</param>
        /// <param name="parametersList">Danh sách mảng tham số tương ứng với từng câu lệnh</param>
        /// <returns>True nếu thành công, False nếu thất bại</returns>
        public static bool ExecuteTransaction(string[] queries, SqlParameter[][] parametersList)
        {
            bool success = false;

            if (queries.Length != parametersList.Length)
            {
                throw new ArgumentException("Số lượng câu lệnh và mảng tham số không khớp nhau");
            }

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    for (int i = 0; i < queries.Length; i++)
                    {
                        using (SqlCommand command = new SqlCommand(queries[i], connection, transaction))
                        {
                            if (parametersList[i] != null)
                            {
                                command.Parameters.AddRange(parametersList[i]);
                            }
                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    success = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi thực thi giao tác: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return success;
        }
    }
}