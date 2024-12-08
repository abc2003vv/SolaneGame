using System;

[Serializable]
public class CurrencyAndAssets
{
    public string UserID { get; set; }          // ID người chơi
    public long Gold { get; set; }              // Lượng vàng
    public long Gems { get; set; }              // Lượng ngọc cao cấp
    public long SpecialTokens { get; set; }     // Các loại token đặc biệt
}
