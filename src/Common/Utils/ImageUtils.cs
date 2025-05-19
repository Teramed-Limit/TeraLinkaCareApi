using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TeraLinkaCareApi.Common.Utils;

public static class ImageUtils
{
    /// <summary>
    /// 將 base64（可含 data:image/png;base64, 前綴）字串轉換為 JPG 並儲存。
    /// </summary>
    /// <param name="base64String">Base64 字串（可含或不含前綴）</param>
    /// <param name="outputFilePath">儲存 JPG 的完整路徑，例如 "output.jpg"</param>
    public static string ConvertBase64PngToJpg(string base64String, string outputFilePath)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return null;

        // 移除 data URI 前綴
        var base64Data = base64String.Contains(",")
            ? base64String.Substring(base64String.IndexOf(",") + 1)
            : base64String;

        byte[] imageBytes = Convert.FromBase64String(base64Data);

        using var ms = new MemoryStream(imageBytes);
        using var img = Image.FromStream(ms);

        img.Save(outputFilePath + ".jpg", ImageFormat.Jpeg);

        return outputFilePath + ".jpg";
    }
}