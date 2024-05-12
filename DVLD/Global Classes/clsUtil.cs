using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    internal class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid newGuid = Guid.NewGuid();
            return newGuid.ToString();
        }
        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if(Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Error Creating Folder" + ex.Message);
                    return false;
                }   
            }
            return true;
        }

        static public string ReplaceFileNameWithGuid(string sourceFile)
        {
            string FileName = sourceFile;
            FileInfo fi = new FileInfo(FileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            string DestinationFolder = @"c:\DVLD-People-Images\";
            if(!CreateFolderIfDoesNotExist(DestinationFolder)) 
                return false;
            string DestinationFile=DestinationFolder + ReplaceFileNameWithGuid(sourceFile);
            try
            {
                File.Copy(sourceFile, DestinationFile, true);
            }
            catch (IOException iox) 
            { 
            MessageBox.Show(iox.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            sourceFile=DestinationFile; 
            return true;
        }
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            { 
                byte[] HashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(HashBytes).Replace("-","").ToLower();
            }
        }
     
        
    }
}
