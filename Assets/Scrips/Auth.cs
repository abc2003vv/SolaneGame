using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System;
using UnityEditor.MemoryProfiler;
using UnityEngine.Windows;

public class Auth : MonoBehaviour
{
    public InputField username;
    public InputField password;
    private string connectionString = "Server=DESKTOP-DPT7713\\SQLEXPRESS; Database=SolanaGame; User Id=sa; Password=123456;";
    // Start is called before the first frame update
    
    private void Loginbtn() 
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Kết nối thành công đến SQL Server!");

                // Câu truy vấn SQL để lấy mật khẩu từ cơ sở dữ liệu
                string query = "SELECT Password FROM Account WHERE Username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số vào câu truy vấn
                    command.Parameters.AddWithValue("@username", username);

                    // Thực hiện truy vấn và lấy kết quả
                    string storedPassword = command.ExecuteScalar() as string;

                    if (storedPassword != null)
                    {
                        // So sánh mật khẩu người dùng nhập vào với mật khẩu trong cơ sở dữ liệu
                        if (password.text==storedPassword)
                        {
                            Debug.Log("Đăng nhập thành công!");
                            // Thực hiện các hành động sau khi đăng nhập thành công
                        }
                        else
                        {
                            Debug.Log("Mật khẩu sai!");
                        }
                    }
                    else
                    {
                        Debug.Log("Tài khoản không tồn tại!");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi kết nối hoặc truy vấn cơ sở dữ liệu: " + e.Message);
        }
    }
}

