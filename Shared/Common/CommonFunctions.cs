using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Reflection;

namespace Shared.Common
{
    public class CommonFunctions
    {
        public static string GetDescription(Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }

        public static List<string> SaveFile(List<IFormFile> files, string subDirectory, string imgPrefix = "")
        {
            List<string> tempFileAddress = new();
            subDirectory ??= string.Empty;
            var target = SiteKeys.SitePhysicalPath + "\\wwwroot" + subDirectory;

            Directory.CreateDirectory(target);

            files.ForEach(file =>
            {
                if (file.Length <= 0) return;

                var nFilename = string.Format(imgPrefix + "{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
                var filePath = Path.Combine(target, nFilename);
                tempFileAddress.Add(nFilename);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
            });
            return tempFileAddress;
        }


        public static string GetRelativeFilePath(string? fileName, string folderPath, string defaultImage)
        {
            return string.Format("{0}{1}{2}", SiteKeys.SiteUrl, folderPath, fileName ?? defaultImage);
        }
    }
}
