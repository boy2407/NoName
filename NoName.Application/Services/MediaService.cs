using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NoName.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace NoName.Application.Services
{
    public class MediaService : IMediaService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public MediaService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            var datePath = Path.Combine(DateTime.Now.ToString("yyyy"),
                                         DateTime.Now.ToString("MM"),
                                         DateTime.Now.ToString("dd"));

            var fullFolderPath = Path.Combine(_userContentFolder, folderName, datePath);

            if (!Directory.Exists(fullFolderPath))
                Directory.CreateDirectory(fullFolderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullFilePath = Path.Combine(fullFolderPath, fileName);

            using var outputStream = new FileStream(fullFilePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(outputStream);

         
            return $"/{USER_CONTENT_FOLDER_NAME}/{folderName}/{datePath.Replace("\\", "/")}/{fileName}";
        }
        
        public async Task DeleteFileAsync(string filePath)
        {

            var absolutePath = Path.Combine(_userContentFolder, filePath.TrimStart('/'));
            if (File.Exists(absolutePath))
            {
                await Task.Run(() => File.Delete(absolutePath));
            }
        }
    }
}
