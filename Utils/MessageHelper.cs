using System;
using System.Windows.Forms;

namespace ConvenienceStoreManager.Utils
{
    public static class MessageHelper
    {
        // Hiển thị thông báo thành công
        public static void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hiển thị thông báo lỗi
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Hiển thị thông báo cảnh báo
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Hiển thị thông báo thông tin
        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Hiển thị hộp thoại xác nhận
        public static DialogResult ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        // Hiển thị thông báo thêm mới thành công
        public static void ShowAddSuccess(string objectName)
        {
            ShowSuccess($"Thêm {objectName} thành công!");
        }

        // Hiển thị thông báo cập nhật thành công
        public static void ShowUpdateSuccess(string objectName)
        {
            ShowSuccess($"Cập nhật {objectName} thành công!");
        }

        // Hiển thị thông báo xóa thành công
        public static void ShowDeleteSuccess(string objectName)
        {
            ShowSuccess($"Xóa {objectName} thành công!");
        }

        // Hiển thị hộp thoại xác nhận xóa
        public static DialogResult ShowDeleteConfirmation(string objectName)
        {
            return ShowConfirmation($"Bạn có chắc muốn xóa {objectName} này không?");
        }

        // Hiển thị hộp thoại xác nhận hủy thay đổi
        public static DialogResult ShowCancelConfirmation()
        {
            return ShowConfirmation("Bạn có chắc muốn hủy thay đổi không?");
        }

        // Hiển thị thông báo lỗi khi truy cập cơ sở dữ liệu
        public static void ShowDatabaseError(Exception ex)
        {
            ShowError($"Lỗi cơ sở dữ liệu: {ex.Message}");
            // Có thể ghi log lỗi chi tiết ở đây
            Console.WriteLine("Database Error Details: " + ex.ToString());
        }
    }
}