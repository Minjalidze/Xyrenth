namespace Xyrenth.Models;

public static class FileInfoExtensions
{
    private static readonly IDictionary<string, string> MimeTypes = new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.ms-excel"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"},
        {".ogg", "application/ogg"}
    };

    public static string GetMimeType(this FileInfo fileInfo)
    {
        return MimeTypes.TryGetValue(fileInfo.Extension.ToLowerInvariant(), out var mimeType) ? mimeType : "application/octet-stream";
    }
}