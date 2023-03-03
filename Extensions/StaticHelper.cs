using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebTools.Extensions
{
    public class StaticHelper
    {
        #region Định dạng lại chuỗi dữ liệu
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }
        #endregion

        #region Khử dấu cho string        
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #endregion

        #region Encode and Decode a base64 string
        //base64 = EncodeBase64(text, Encoding.ASCII);
        public static string EncodeFileToBase64(string filePath)
        {
            if (filePath == null) return null;
            var bytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64ToFilePDF(string fileBase64, string fileName)
        {
            if (fileBase64 == null) return null;
            string randomID = Guid.NewGuid().ToString("N");
            byte[] tempBytes = Convert.FromBase64String(fileBase64);
            string filePath = @$"D:\VanBan\{randomID}_{fileName}.pdf";
            File.WriteAllBytes(filePath, tempBytes);
            if (File.Exists(filePath)) { return filePath; }
            else { return String.Empty; }
        }
        #endregion
    }
}
