using ContactMe.Client.Helpers;
using ContactMe.Models;
using System.Text.RegularExpressions;

namespace ContactMe.Helpers
{
    public static class UploadHelper
    {
        public static readonly string DefaultProfilePicture = ImageHelper.DefaultProfilePicture;
        public static readonly string DefaultContactImage = ImageHelper.DefaultContactImage;
        public static readonly int MaxFileSize = ImageHelper.MaxFileSize;

        public static async Task<ImageUpload> GetImageUploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);
            byte[] data = ms.ToArray();

            if (ms.Length > MaxFileSize)
            {
                throw new IOException("Images must be less than 5MB!");
            }

            ImageUpload upload = new ImageUpload()
            {
                Id = Guid.NewGuid(),
                Data = data,
                Type = file.ContentType
            };

            return upload;
        }

        public static ImageUpload GetImageUpload(string dataUrl)
        {
            GroupCollection matchGroups = Regex.Match(dataUrl, @"data:(?<type>.+?);base64,(?<data>.+)").Groups;

            if (matchGroups.ContainsKey("type") && matchGroups.ContainsKey("data"))
            {
                string contentType = matchGroups["type"].Value;
                byte[] data = Convert.FromBase64String(matchGroups["data"].Value);

                if (data.Length <= MaxFileSize)
                {
                    ImageUpload upload = new ImageUpload()
                    {
                        Id = Guid.NewGuid(),
                        Data = data,
                        Type = contentType
                    };

                    return upload;
                }
            }

            throw new IOException("Data URL was either invalid or too large");
        }
    }
}
