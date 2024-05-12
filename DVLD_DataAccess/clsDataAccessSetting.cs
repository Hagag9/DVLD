using System;
using System.Configuration;


namespace DVLD_DataAccess
{
    
    static class clsDataAccsessSetting
    {
        //public static string connectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;";
        public static string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            // source name for Event log
        public static string SourceName = "DVLD App";
    }
}
