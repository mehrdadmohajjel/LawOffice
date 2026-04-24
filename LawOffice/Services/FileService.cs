namespace LawOffice.Services
{
    public interface IFileService
    {
        Task<(string storedName, string relativePath)> SaveAsync(
            IFormFile file, string folder);
        void Delete(string relativePath);
        bool Exists(string relativePath);
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions =
            [".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".xlsx", ".xls"];
        private const long MaxFileSize = 20 * 1024 * 1024; // 20MB

        public FileService(IWebHostEnvironment env) => _env = env;

        public async Task<(string storedName, string relativePath)> SaveAsync(
            IFormFile file, string folder)
        {
            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("حجم فایل بیش از حد مجاز است (۲۰ مگابایت)");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException("نوع فایل مجاز نیست");

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadPath);

            var storedName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadPath, storedName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return (storedName, $"/uploads/{folder}/{storedName}");
        }

        public void Delete(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool Exists(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath,
                relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            return File.Exists(fullPath);
        }
    }    }