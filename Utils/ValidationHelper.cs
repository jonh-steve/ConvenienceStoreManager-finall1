using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ConvenienceStoreManager.Utils
{
    public static class ValidationHelper
    {
        // Kiểm tra giá trị không được trống
        public static bool IsNotEmpty(Control control, string controlName)
        {
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                MessageHelper.ShowWarning($"{controlName} không được để trống!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra giá trị là số nguyên hợp lệ
        public static bool IsValidInteger(Control control, string controlName, out int value)
        {
            value = 0;
            if (!int.TryParse(control.Text, out value))
            {
                MessageHelper.ShowWarning($"{controlName} phải là số nguyên hợp lệ!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra giá trị là số thực hợp lệ
        public static bool IsValidDecimal(Control control, string controlName, out decimal value)
        {
            value = 0;
            if (!decimal.TryParse(control.Text, out value))
            {
                MessageHelper.ShowWarning($"{controlName} phải là số thực hợp lệ!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra giá trị là số dương
        public static bool IsPositive(decimal value, string controlName, Control control)
        {
            if (value <= 0)
            {
                MessageHelper.ShowWarning($"{controlName} phải là số dương!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra giá trị không âm
        public static bool IsNotNegative(decimal value, string controlName, Control control)
        {
            if (value < 0)
            {
                MessageHelper.ShowWarning($"{controlName} không được là số âm!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra độ dài chuỗi không vượt quá giới hạn
        public static bool MaxLength(Control control, string controlName, int maxLength)
        {
            if (control.Text.Length > maxLength)
            {
                MessageHelper.ShowWarning($"{controlName} không được vượt quá {maxLength} ký tự!");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra chuỗi theo mẫu regex
        public static bool MatchesPattern(Control control, string controlName, string pattern, string errorMessage)
        {
            if (!Regex.IsMatch(control.Text, pattern))
            {
                MessageHelper.ShowWarning($"{controlName} {errorMessage}");
                control.Focus();
                return false;
            }
            return true;
        }

        // Kiểm tra DataGridView có dữ liệu không
        public static bool HasRows(DataGridView dataGridView, string gridName)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageHelper.ShowWarning($"{gridName} không có dữ liệu!");
                return false;
            }
            return true;
        }

        // Kiểm tra combo box đã chọn item chưa
        public static bool IsSelected(ComboBox comboBox, string comboName)
        {
            if (comboBox.SelectedIndex == -1)
            {
                MessageHelper.ShowWarning($"Vui lòng chọn {comboName}!");
                comboBox.Focus();
                return false;
            }
            return true;
        }
    }
}