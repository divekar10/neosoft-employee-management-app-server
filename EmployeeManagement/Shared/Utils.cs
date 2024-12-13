namespace EmployeeManagement.Shared
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

        public static int DateDifferenceInYears(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate == null)
            {
                throw new ArgumentNullException(nameof(firstDate));
            }

            if (secondDate == null)
            {
                throw new ArgumentNullException(nameof(secondDate));
            }

            var age = secondDate.Year - firstDate.Year;

            if (DateTime.Now.Month < firstDate.Month ||
                (DateTime.Now.Month == firstDate.Month && DateTime.Now.Day < firstDate.Day))
            {
                age--;
            }

            return age;
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
            return fileName;
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

        public static void DeleteFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);
                var file = new FileInfo(filePath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
        }

        public static int GetRandomNumber()
        {
            var random = new Random();

            return random.Next(99999);
        }
    }
}
