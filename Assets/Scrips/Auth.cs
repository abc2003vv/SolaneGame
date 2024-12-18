using UnityEngine;
using System;
using Firebase.Database;
using Google.MiniJSON;
using UnityEngine.UI;
using System.Security.Cryptography;
using Unity.VisualScripting;
using System.Text;
using UnityEngine.SceneManagement;
using Firebase.Auth;
[Serializable]
public class dataTosave
{

}
public class Auth : MonoBehaviour
{
    // gọi đối tượng
    // input Unity
    public GameObject PanelMessage;
    public InputField username;
    public InputField password;
    public InputField userRes;
    public InputField passwordRes;
    public InputField agianpasswordRes;
    public InputField Email;
    private DatabaseReference databaseRef;
    private FirebaseAuth auth;
    private bool checkRes = false;
    private void Awake()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //Login
    public void Login()
    {
        string usernameInput = username.text;
        string passwordInput = password.text;

        auth.Si(usernameInput, passwordInput).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Login encountered an error: " + task.Exception);
                return;
            }

            // Đăng nhập thành công
            FirebaseUser user = task.Result;
            Debug.Log("User logged in successfully: " + user.Email);

            // Chuyển đến Scene mới (ví dụ: "Lobby")
            LoadSceneByName("Lobby");
        });
    }



    //Register
    public void Register()
    {
        string usernameInput = userRes.text;
        string passwordInput = passwordRes.text;
        string confirmpasswrod = agianpasswordRes.text;
        string email = Email.text;

        // check 
        if (string.IsNullOrEmpty(usernameInput) || string.IsNullOrEmpty(passwordInput) || string.IsNullOrEmpty(email))
        {
            print("not null Res");
        }
        if (passwordInput != confirmpasswrod)
        {
            print("not equal password Res");
        }
        LoadSceneByName("Lobby");
        string userID = Guid.NewGuid().ToString();
        string passwordHash = HashPassword(passwordInput);
        string registrantionDate = DateTime.Now.ToString("yyyy-MM-yy HH:mm:ss");
        Users newuser = new Users(userID, usernameInput, passwordInput, passwordHash, registrantionDate, "active");
        // save data in Firebase
        CheckIfEmailExists(email, exists =>
        {
            if (exists)
            {
                Debug.LogError("Email already exists. Please use a different email.");
            }
            else
            {
                SaveDataUser(newuser);

            }
        });
    }
    private void SaveDataUser(Users users)
    {
        string json = JsonUtility.ToJson(users);

        databaseRef.Child("users").Child(users.UserID).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Step 2: Data saved successfully!");
                checkRes = true;
            }
            else
            {
                Debug.LogError("Step 2: Failed to save data: " + task.Exception);
            }
        });
    }
    private string HashPassword(string password)
    {
        using (SHA256 sHA256 = SHA256.Create())
        {
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
    //
    private void CheckIfEmailExists(string email, Action<bool> callback)
    {
        databaseRef.Child("users").OrderByChild("email").EqualTo(email).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                callback(snapshot.Exists); // True nếu email đã tồn tại
            }
            else
            {
                Debug.LogError("Failed to check email: " + task.Exception);
                callback(false);
            }
        });
    }

}