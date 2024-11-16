using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System;
using UnityEditor.MemoryProfiler;
using UnityEngine.Windows;
using UnityEditor.Search;
using System.Text.RegularExpressions;

public class Auth : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField email;
    public InputField usernameRes;
    public InputField passwordRes;
    public InputField confirmpassword;
    public GameObject PanelLogin;
    public GameObject PanelRes;
    public GameObject PanelMessage;
    public Text text;
    private string connectionString = "Server=DESKTOP-DPT7713\\SQLEXPRESS; Database=GAMESOLANA; User Id=sa; Password=123456;";
    public void btnOK()
    {
        PanelMessage.SetActive(false);
    }
    public void btnReclick()
    {
        PanelLogin.SetActive(false);
        PanelRes.SetActive(true);
    }
    public void OK()
    {
        PanelRes.SetActive(false);
        PanelLogin.SetActive(true);
    }
    // Start is called before the first frame update
    public void Registerbtn()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                Debug.Log("connect success!");
            }
        }
        catch (Exception ex) {
            Debug.Log("lỗi" + ex);
        }
    }
    public void Loginbtn()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Kết nối thành công đến SQL Server!");

                // Câu truy vấn SQL để lấy mật khẩu từ cơ sở dữ liệu
                string query = "SELECT password FROM [UserProfile] WHERE username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số vào câu truy vấn
                    command.Parameters.AddWithValue("@username", username.text);

                    // Thực hiện truy vấn và lấy kết quả
                    string storedPassword = command.ExecuteScalar() as string;

                    if (storedPassword != null)
                    {
                        // So sánh mật khẩu người dùng nhập vào với mật khẩu trong cơ sở dữ liệu
                        if (password.text == storedPassword)
                        {
                            Debug.Log("Đăng nhập thành công!");
                            // Thực hiện các hành động sau khi đăng nhập thành công
                        }
                        else
                        {
                            text.text = "Mât khẩu sai!";
                            PanelMessage.SetActive(true);
                        }
                    }
                    else
                    {
                        text.text = "tài khoản không tồn tại";
                        PanelMessage.SetActive(true);
                    }
                }
            }
        }
        catch (Exception e)
        {
            text.text = "Lỗi khi kết nối" + e.Message;
            PanelMessage.SetActive(true);
        }
    }
    public void Register()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Connect success!");

                string query = "INSERT INTO [UserProfile] (username, PasswordHash, email, registrationDate, status) VALUES (@username, @password, @email, @registrationDate, @status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username.text);
                    command.Parameters.AddWithValue("@password", password.text); 
                    command.Parameters.AddWithValue("@email", email.text);
                    command.Parameters.AddWithValue("@registrationDate", DateTime.Now); 
                    command.Parameters.AddWithValue("@status", "active"); 

                    // Thực hiện truy vấn INSERT
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Debug.Log("Đăng ký thành công!");
                        text.text = "Đăng ký thành công!";
                    }
                    else
                    {
                        text.text = "Đăng ký thất bại!";
                    }
                    PanelMessage.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
            text.text = "Đã xảy ra lỗi. Vui lòng thử lại!";
            PanelMessage.SetActive(true);
        }
    }

}




