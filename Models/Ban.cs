namespace Xyrenth.Models;

public class Ban
{
	public Ban(string nick, string steamID, string reason, string hardwareID, string ip, string duration, string date, string price) {
		Nick = nick;
		SteamID = steamID;
		Reason = reason;
        HardwareID = hardwareID;
        IP = ip;
        Duration = duration;
		Date = date;
		PriceText = $"{price}₽";
	}
    public string Nick { get; set; }
	public string SteamID { get; set; }
    public string Reason { get; set; }
    public string HardwareID { get; set; }
    public string IP { get; set; }
    public string Date { get; set; }
    public string Duration { get; set; }
    public string PriceText { get; set; }
}