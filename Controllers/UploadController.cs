using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Context;
namespace recipe_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UploadController : ControllerBase
    {
        
[HttpPost("{folderName}")]
public async Task<IActionResult> UploadFile(IFormFile file, string folderName)
{
    if (file == null || file.Length == 0)
    {
        return BadRequest("No file uploaded or file is empty.");
    }

    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    var filePath = Path.Combine(uploadsFolder, file.FileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    return Ok(new { file.FileName, file.Length, FilePath = filePath });
}


} }