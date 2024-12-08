using System;

[Serializable]
public class UserSettings
{
    public string UserID { get; set; }          // ID người chơi
    public int SoundVolume { get; set; }        // Mức âm lượng âm thanh (0-100)
    public int MusicVolume { get; set; }        // Mức âm lượng nhạc nền (0-100)
    public string ControlScheme { get; set; }   // Thiết lập điều khiển (ví dụ: Default, Custom)
    public string Language { get; set; }        // Ngôn ngữ được chọn (ví dụ: English, Vietnamese)
}
