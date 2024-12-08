using System;

[Serializable]
public class Achievement
{
    public string AchievementID { get; set; }   // ID thành tích
    public string UserID { get; set; }          // ID người chơi
    public string AchievementName { get; set; } // Tên thành tích
    public string Description { get; set; }     // Mô tả cách đạt thành tích
    public DateTime DateAchieved { get; set; }  // Ngày đạt thành tích
    public string Reward { get; set; }          // Phần thưởng nhận được (tiền, vật phẩm)
}
