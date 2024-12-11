using System.Text;

namespace EcomemerceASP_NET.Helpers
{
    public class MyUtil
    {
        //public static string UploadHinh(IFormFile Hinh, string folder)
        //{
        //    try
        //    {
        //        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName);
        //        using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
        //        {
        //            Hinh.CopyTo(myfile);
        //        }
        //        return Hinh.FileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //}
        public static string UploadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                // Xây dựng đường dẫn thư mục
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Tạo tên file duy nhất để tránh trùng
                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(Hinh.FileName);
                var fullPath = Path.Combine(folderPath, uniqueFileName);

                // Lưu file
                using (var myfile = new FileStream(fullPath, FileMode.Create))
                {
                    Hinh.CopyTo(myfile);
                }

                // Trả về đường dẫn tương đối
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi upload file: {ex.Message}");
                return string.Empty;
            }
        }


        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$$#";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[new Random().Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
