using System;

public class Users
{
    public string UserID;
    public string Username;
    public string Password;
    public string PasswordHash;
    public string RegistrationDate;
    public string Status;
    public string CharacterName; // Thêm trường tên nhân vật

    public Users(string userID, string username, string password, string passwordHash, string registrationDate, string status, string characterName)
    {
        UserID = userID;
        Username = username;
        Password = password;
        PasswordHash = passwordHash;
        RegistrationDate = registrationDate;
        Status = status;
        CharacterName = characterName; // Khởi tạo giá trị cho tên nhân vật
    }
}
