namespace Demo.PL.Utility
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\Files" + @$"\{folderName}";

            var folderParh=Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);
            
            var fileName = $"{Guid.NewGuid()}-{file?.FileName}";

            var filePath =Path.Combine(folderParh, fileName);

            using (var fileStream = new FileStream(filePath, FileMode
                .Create)) 
            {
                file.CopyTo(fileStream);
            }
            return fileName;

        }
        public static void DeleteFile(string fileName, string folderName) 
        {
            var folderParh = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName, fileName);

        }
    }
}
