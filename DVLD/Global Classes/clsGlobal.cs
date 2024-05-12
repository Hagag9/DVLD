using DVLD_Buisness;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    internal class clsGlobal
    {
        public static clsUser CurrentUser;
        public static string SourceName = "DVLD App";
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            //using Registry
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string valueName = "data";
            string vlaueData = Username + "#//#" + Password;
            try
            {
                using(RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                {
                    using (RegistryKey key = baseKey.OpenSubKey(keyPath, true))
                    {
                        if (Username=="" && key != null) 
                        { 
                            key.DeleteValue(valueName);
                            return true; 
                        }
                        Registry.SetValue(keyPath, valueName, vlaueData, RegistryValueKind.String);
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                EventLog.WriteEntry(clsGlobal.SourceName,ex.Message,EventLogEntryType.Error);    
                return false;
            }
            //=============================================
            //try
            //{
            //    string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            //    string filepath = currentDirectory + "\\data.txt";
            //    if (Username == "" && File.Exists(filepath))
            //    {
            //        File.Delete(filepath);
            //        return true;
            //    }
            //    string dataToSave = Username + "#//#" + Password;
            //    using (StreamWriter writer = new StreamWriter(filepath))
            //    {
            //        writer.WriteLine(dataToSave);
            //        return true;
            //    }
            //}
            //catch (Exception ex) 
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}
        }
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string valueName = "data";
            try
            {
                string value = Registry.GetValue(keyPath, valueName,null) as string ;
                if (value != "")
                {
                    string[] result = value.Split(new string[] { "#//#" }, StringSplitOptions.None);
                    Username = result[0];
                    Password = result[1];
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                EventLog.WriteEntry(clsGlobal.SourceName, ex.Message, EventLogEntryType.Error);
                return false;
            }

            //try
            //{
            //    string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            //    string filepath = currentDirectory + "\\data.txt";
            //    if (File.Exists(filepath))
            //    {
            //        using (StreamReader reader = new StreamReader(filepath))
            //        {
            //            string line;
            //            while((line=reader.ReadLine())!=null) 
            //            {
            //                Console.WriteLine(line);
            //                string[] result =line.Split(new string[] { "#//#" }, StringSplitOptions.None);
            //                Username = result[0];   
            //                Password = result[1];
            //            }
            //            return true;

            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}
        }

    } 
}