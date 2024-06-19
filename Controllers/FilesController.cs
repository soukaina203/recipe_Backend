using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Services;

namespace Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // private IWebHostEnvironment _hostingEnvironment;
        private readonly AppSettings _appSettings;
        private readonly string _webRootPath;
        public FilesController(IWebHostEnvironment hostingEnvironment, IOptions<AppSettings> appSettings)
        {
            // _hostingEnvironment = hostingEnvironment;
            _appSettings = appSettings.Value;
            // if(_appSettings.deployMode=="DOCKER")
            //     _webRootPath = "C:\\Users\\Administrator\\Documents\\APP\\GAM\\PIC";
            // else
            _webRootPath = "wwwroot"; // _appSettings.UrlUpload == "" ? hostingEnvironment.WebRootPath : _appSettings.UrlUpload;

        }

        [HttpPost("{folder}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromRoute] string folder)
        {
            var file = Request.Form.Files[0];

            string path = Path.Combine(_webRootPath, folder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (file != null)
            {
                try
                {
                    string fullPath = Path.Combine(path, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                catch (System.Exception ex)
                {
                    return Ok(ex.Message);
                }

            }
            return Ok(new { message = file.FileName });
        }

        [HttpPost("{folder}"), DisableRequestSizeLimit]
        public async Task<ActionResult<string>> UploadFiles([FromRoute] string folder)
        {
            IFormFileCollection files = Request.Form.Files;

            string path = Path.Combine(_webRootPath, folder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (files.Count > 0)
            {
                try
                {
                    foreach (var file in files)
                    {
                        string fullPath = Path.Combine(path, file.FileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    return Ok(ex.Message);
                }

            }
            return Ok(new { message = files.Count , code = 1});
        }

        [HttpPost("{folder}")]
        public IActionResult DeleteFiles(string folder, string[] filenames)
        {
            if (filenames.Length == 0)
            {
                return Ok(false);
            }
            try
            {
                foreach (var filename in filenames)
                {
                    var fileInfo = new FileInfo($"{_webRootPath}\\{folder}\\{filename}");
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }

            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }

            return Ok(true);
        }

        [HttpPost("{folder}/{filename}")]
        public IActionResult DeleteFile(string folder, string filename)
        {
            if (filename == null)
            {
                return Ok(false);
            }

            var fileInfo = new FileInfo($"{_webRootPath}\\{folder}\\{filename.Replace("_", "/")}");

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                return Ok(true);
            }

            return Ok(false);
        }

        [HttpGet("{path}")]
        public IActionResult download( string path)
        {
            string _path = "wwwroot/" + path.Replace('_', '/');

            if (System.IO.File.Exists(_path))
            {
                return File(System.IO.File.OpenRead(_path), "application/octet-stream", Path.GetFileName(_path));
            }

            return Ok(new { message = "file not found" });
        }
    }
}
