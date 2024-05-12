using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID,ref string CountryName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString );
            string query = @"SELECT * FROM Countries WHERE CountryID= @countryId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@countryId", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    isFound = true;
                    CountryName = (string)reader["CountryName"];
                }

            }
            catch (Exception ex) 
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
                isFound = false; 
            }
            finally { connection.Close(); }
            return isFound;
        }
        public static bool GetCountryInfoByName(ref int ID,string CountryName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = @"SELECT * FROM Countries WHERE CountryName= @countryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@countryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ID = (int)reader["CountryID"];
                }

            }
            catch (Exception ex)
            { 
                isFound = false;
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally { connection.Close(); }
            return isFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable(); 
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = "select * from Countries order by CountryName";
            SqlCommand command =new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    dt.Load(reader);
                   
                }
                reader.Close();
            }
            catch (Exception ex) 
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally { connection.Close(); } 
            return dt;
        }
    }
}
