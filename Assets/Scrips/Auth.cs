using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Data.SqlClient;
using System;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    // Trường cho Register
    public InputField registerUsernameField;  // Ô nhập tên tài khoản đăng ký
    public InputField registerPasswordField;  // Ô nhập mật khẩu đăng ký
    public InputField confirmPasswordField;   // Ô nhập lại mật khẩu (dành cho đăng ký)
    public InputField emailField;             // Ô nhập email (dành cho đăng ký)

    // Trường cho Login
    public InputField loginUsernameField;     // Ô nhập tên tài khoản đăng nhập
    public InputField loginPasswordField;     // Ô nhập mật khẩu đăng nhập

    public Text feedbackText;                 // Hiển thị thông báo cho người dùng

    private string connectionString = "Server=DESKTOP-DPT7713\\SQLEXPRESS; Database=SolanaGame; User Id=sa; Password=123456;";

    // Hàm đăng ký tài khoản
    public void Register()
    {
        string username = registerUsernameField.text;
        string password = registerPasswordField.text;
        string confirmPassword = confirmPasswordField.text;
        string email = emailField.text;

        // Kiểm tra các trường dữ liệu không được bỏ trống
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
        {
            feedbackText.text = "Vui lòng điền đầy đủ thông tin!";
            return;
        }

        // Kiểm tra mật khẩu và mật khẩu xác nhận
        if (password != confirmPassword)
        {
            feedbackText.text = "Mật khẩu và xác nhận mật khẩu không khớp!";
            return;
        }

        try
        {
            // Kết nối đến cơ sở dữ liệu và kiểm tra xem tài khoản đã tồn tại chưa
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Account WHERE Username = @username";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", username);

                    int userExists = (int)checkCommand.ExecuteScalar();
                    if (userExists > 0)
                    {
                        feedbackText.text = "Tài khoản đã tồn tại!";
                        Debug.Log("Tài khoản đã tồn tại!");
                        return;
                    }
                }

                // Nếu tài khoản chưa tồn tại, tiến hành đăng ký
                string query = "INSERT INTO Account (Username, Password, Email) VALUES (@username, @password, @email)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password); // Mật khẩu chưa mã hóa, có thể mã hóa sau nếu cần
                    command.Parameters.AddWithValue("@email", email);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        feedbackText.text = "Đăng ký thành công!";
                        Debug.Log("Đăng ký thành công!");
                    }
                    else
                    {
                        feedbackText.text = "Đăng ký thất bại.";
                        Debug.Log("Đăng ký thất bại.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            feedbackText.text = "Lỗi khi đăng ký: " + e.Message;
            Debug.LogError("Lỗi khi đăng ký: " + e.Message);
        }
    }

    // Hàm đăng nhập
    public void Login()
    {
        string username = loginUsernameField.text;
        string password = loginPasswordField.text;

        // Kiểm tra các trường dữ liệu không được bỏ trống
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Vui lòng điền tên đăng nhập và mật khẩu!";
            return;
        }

        try
        {
            // Kết nối đến cơ sở dữ liệu và kiểm tra thông tin đăng nhập
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Password FROM Account WHERE Username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    string storedPassword = command.ExecuteScalar() as string;

                    if (storedPassword != null)
                    {
                        if (storedPassword == password)
                        {
                            feedbackText.text = "Đăng nhập thành công!";
                            Debug.Log("Đăng nhập thành công!");
                            UpdateLastLogin(username, connection);
                        }
                        else
                        {
                            feedbackText.text = "Mật khẩu không chính xác!";
                            Debug.Log("Mật khẩu không chính xác.");
                        }
                    }
                    else
                    {
                        feedbackText.text = "Tài khoản không tồn tại!";
                        Debug.Log("Tài khoản không tồn tại.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            feedbackText.text = "Lỗi khi đăng nhập: " + e.Message;
            Debug.LogError("Lỗi khi đăng nhập: " + e.Message);
        }
    }

    // Cập nhật thời gian đăng nhập lần cuối
    private void UpdateLastLogin(string username, SqlConnection connection)
    {
        string query = "UPDATE Account SET LastLogin = GETDATE() WHERE Username = @username";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@username", username);
            command.ExecuteNonQuery();
        }
    }
}
