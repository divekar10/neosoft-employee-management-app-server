﻿namespace EmployeeManagement.Shared
{
    public static class Utils
    {
        public static bool Is18EmployeeYearsOld(DateTime? dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                throw new ArgumentNullException();
            }

            var age = DateTime.Now.Year - dateOfBirth?.Year;

            if (DateTime.Now.Month < dateOfBirth?.Month ||
                (DateTime.Now.Month == dateOfBirth?.Month && DateTime.Now.Day < dateOfBirth?.Day))
            {
                age--;
            }

            return age > 18;
        }

        public static async Task<string> SaveFileAsync(IFormFile file)
        {
            var folderLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
            if (!Directory.Exists(folderLocation))
            {
                Directory.CreateDirectory(folderLocation);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderLocation, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("wwwroot\\uploads", fileName);
        }

        //public static async Task<string> SaveFileAsync(IFormFile file)
        //{
        //    var folderLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
        //    if (!Directory.Exists(folderLocation))
        //    {
        //        Directory.CreateDirectory(folderLocation);
        //    }

        //    var filePath = Path.Combine(folderLocation, Guid.NewGuid() + Path.GetExtension(file.FileName));
        //    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    return filePath;
        //}

        public static async Task<string> FileToBase64(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (File.Exists(path))
            {
                byte[] bytes = await File.ReadAllBytesAsync(path);
                return Convert.ToBase64String(bytes);
            }

            return string.Empty;
        }

        public static int GetRandomNumber()
        {
            var random = new Random();

            return random.Next(99999);
        }
    }
}
