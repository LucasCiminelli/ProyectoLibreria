using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment _enviroment;

        public FileService(IWebHostEnvironment environment)
        {
            _enviroment = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = this._enviroment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");

                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var extension = Path.GetExtension(imageFile.FileName);

                var allowedExtensions = new String[] { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(extension))
                {
                    string allowedExtensionsString = String.Join(",", allowedExtensions);
                    string message = $"Extensión del archivo inválida. Extensiones válidas: {allowedExtensionsString}";
                    return new Tuple<int, string>(0, message);
                }

                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + extension;

                var fileWithPath = Path.Combine(path, newFileName);


                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();

                return new Tuple<int, string>(1, newFileName);


            }
            catch (Exception)
            {

                return new Tuple<int, string>(0, "Error guardando la imagen");
            }
        }
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _enviroment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }

                return false;

            }
            catch (Exception)
            {

                return false;
            }
        }


    }
}