using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using Xyrenth.Models;

namespace Xyrenth.Controllers
{
    public class ClientController : Controller
    {
        private const string FilesDirectory = "/rustlegacy";
        private const string ProjectsDirectory = "/projects";

        internal static List<Hash> BHashes;
        internal static List<Hash> LHashes;

        internal static string ManifestHash;
        internal static string UpdaterHash;

        [HttpGet("/clientbundles")]
        public async Task<IActionResult> Bundles()
        {
            if (BHashes == null || BHashes.Count == 0)
            {
                return NotFound();
            }
            return Content(JsonConvert.SerializeObject(BHashes, Formatting.Indented));
        }
        [HttpGet("/clientlibraries")]
        public async Task<IActionResult> Libraries()
        {
            if (LHashes == null || LHashes.Count == 0)
            {
                return NotFound();
            }
            return Content(JsonConvert.SerializeObject(LHashes, Formatting.Indented));
        }

        [HttpGet("/client")]
        public IActionResult DownloadClient()
        {
            var filePath = $"{FilesDirectory}/XyrenthClient.exe";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }

        [HttpGet("/vcredist")]
        public IActionResult DownloadVC()
        {
            var filePath = $"{FilesDirectory}/vcredist.zip";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }

        [HttpGet("/netframework")]
        public IActionResult DownloadNetFramework()
        {
            var filePath = $"{FilesDirectory}/dotNetFx35.exe";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }

        [HttpGet("/clientdownload")]
        public IActionResult Download(string file)
        {
            if (LHashes.Find(f => f.Name == file) == null)
            {
                return Content("отсоси пинуса шкила");
            }
            var filePath = $"{FilesDirectory}{file}";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }

        [HttpGet("/clientmanifest")]
        public IActionResult GetManifest(string action)
        {
            if (action == "hash") return Content(ManifestHash);
            if (action == "load")
            {
                var filePath = $"{FilesDirectory}/bundles/manifest.txt";
                var fileInfo = new FileInfo(filePath);

                var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
                Response.Headers.Append("Content-Disposition", cd.ToString());

                return PhysicalFile(filePath, fileInfo.GetMimeType());
            }
            return Content("отсоси пинуса шкила");
        }

        [HttpGet("/clientupdater")]
        public IActionResult GetUpdater(string action)
        {
            var filePath = $"{FilesDirectory}/XyrenthUpdater.exe";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }


        [HttpGet("/projectfile")]
        public IActionResult ProjectFile(string project, string file)
        {
            if (string.IsNullOrEmpty(project) || string.IsNullOrEmpty(file))
            {
                return NotFound();
            }
            if (file.Contains("../") || file.Contains(@"..\") || project.Contains("../") || project.Contains(@"..\"))
            {
                return Content("отсоси пинуса шкила");
            }
            var filePath = $"{ProjectsDirectory}/{project}/{file}";
            var fileInfo = new FileInfo(filePath);

            var cd = new ContentDisposition { FileName = fileInfo.Name, Inline = true };
            Response.Headers.Append("Content-Disposition", cd.ToString());

            return PhysicalFile(filePath, fileInfo.GetMimeType());
        }
    }
}
