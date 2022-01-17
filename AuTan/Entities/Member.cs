namespace AuTan.Entities;

public class Member
{
    public long Id { get; set; }
    public long Money { get; set; }
    public string Wallpaper { get; set; }
    public long MinuteLastActive { get; set; } // minutes from epoch
}