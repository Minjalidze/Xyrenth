namespace Xyrenth.Models
{
    public interface IClipboardService
    {
        Task CopyToClipboard(string text);
    }
}
