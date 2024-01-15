namespace Business.Utilities.Extentions
{
    public static class FileValidator
    {
        public static bool IsValid(this IFormFile file, string format = "image/")
        {
            if (file.ContentType.Contains(format)) return true;
            return false;
        }
        public static bool LimitSize(this IFormFile file, int limiSize = 10)
        {
            if (file.Length <= limiSize * 1024 * 1024) return true;
            return false;
        }

        public static string GetGuidName(string fullName)
        {
            int score = fullName.LastIndexOf("_");
            if (score > 0)
            {
                return fullName.Substring(0, score);
            }
            return fullName;
        }
        public static string GetFileFormat(string fullName)
        {
            int score = fullName.LastIndexOf(".");
            if (score > 0)
            {
                return fullName.Substring(score);
            }
            return fullName;
        }

        public static async Task<string> CrateFileAsync(this IFormFile file, string root, params string[] folders)
        {
            string originalName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string guidName = GetGuidName(originalName);
            string format = GetFileFormat(originalName);
            string finalName = guidName + format;

            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, finalName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return finalName;
        }

        public static async void DeleteAsync(this string finalName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, finalName);

            if (File.Exists(path)) File.Delete(finalName);
        }

    }
}
