using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace UserService.Services.CloudService
    {
    public class CloudinaryService
        {
        private Cloudinary _cloudinary;

        public CloudinaryService()
            {
            // Cấu hình Cloudinary bằng thông tin của bạn
            var account = new Account(
                "djvanrbcm", // Cloud name của bạn
                "274921459829559",     // API key của bạn
                "ol8cRcon9JVy9LNd9fOHjYzow1I"   // API secret của bạn
            );
            _cloudinary=new Cloudinary(account);
            }

        public async Task<string>? UploadImageFromBase64(string? base64Image)
            {
            try
                {
                // 1. Nếu không có ảnh, trả về null (không lỗi)
                if (string.IsNullOrWhiteSpace(base64Image))
                    {
                    return null;
                    }
                // 2. Nếu base64 có dạng "data:image/png;base64,...."
                int commaIndex = base64Image.IndexOf(",");
                if (commaIndex!=-1)
                    {
                    base64Image= base64Image.Substring(commaIndex+1);
                    }
                // 3. Kiểm tra xem base64 có đúng định dạng không
                if (!IsBase64String(base64Image))
                    {
                    throw new FormatException("Base64 image format is invalid.");
                    }
                // 4. Chuyển base64 thành mảng byte
                byte[] imageBytes =  Convert.FromBase64String(base64Image);
                // 5. Upload ảnh lên Cloudinary
                using MemoryStream? stream = new MemoryStream(imageBytes);
                var uploadParams = new ImageUploadParams
                    {
                    File=new FileDescription("image_file", stream)
                    };

                var uploadResult =  _cloudinary.Upload(uploadParams);
                return  uploadResult?.SecureUrl?.ToString();
                }
            catch (Exception ex)
                {
                Console.WriteLine("Lỗi khi upload ảnh lên Cloudinary: "+ex.Message);
                return null; // Trả về null nếu có lỗi thay vì throw Exception
                }
            }

        /// <summary>
        /// Kiểm tra chuỗi có phải là Base64 hợp lệ không
        /// </summary>
        private bool IsBase64String(string base64)
            {
            if (string.IsNullOrWhiteSpace(base64)) return false;
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
            }
        }
    }
