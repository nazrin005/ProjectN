namespace ProjectN.Utilities
{
    public static class FileUploadExtension
    {
        public static string SaveImage(this IFormFile ImageFile, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            string fullpath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }
            return fileName;
        }
        public static void DeleteFile(this string? fileName, IWebHostEnvironment env, string folder)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            string path = Path.Combine(env.WebRootPath, folder, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
