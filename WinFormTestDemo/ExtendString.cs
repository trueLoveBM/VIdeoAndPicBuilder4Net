using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormTestDemo
{
    public static class ExtendString
    {
        /// <summary>
        /// 获取本路径下以时间格式yyMMddHHmmssfff为名字的唯一文件名Added by renyingjie
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetUniqueFileName(this string filePath, string fileExtension)
        {
            string temp = filePath + $@"/{DateTime.Now.ToString("yyMMddHHmmssfff")}{fileExtension}";
            while (File.Exists(temp))
            {
                temp = filePath + $@"/{DateTime.Now.ToString("yyMMddHHmmssfff")}{fileExtension}";
            }
            return temp;
        }
    }
}
