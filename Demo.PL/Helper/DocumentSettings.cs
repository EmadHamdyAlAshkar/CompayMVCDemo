namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);

            // 2. Get file name and make it unique
            var fileName = $"{Guid.NewGuid}-{Path.GetFileName(file.FileName)}";

            // 3. Get file path
            var filepath = Path.Combine(folderPath, fileName);

            //
            using var filStream = new FileStream(filepath, FileMode.Create);

            file.CopyTo(filStream);

            return fileName;
        }
    }
}
