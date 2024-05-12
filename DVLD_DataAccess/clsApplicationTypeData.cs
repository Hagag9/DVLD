using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
            ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicationTypeTitle = reader["ApplicationTypeTitle"].ToString();
                    ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);

                    isFound = true;
                }
                else isFound= false;
                reader.Close();
            }
            catch (Exception ex) 
            {
                isFound= false;
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                try
                {
                    connection.Close();
                }
                catch { }
                  
            }
            return isFound;
        }
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = "SELECT * FROM ApplicationTypes";
            SqlCommand command = new SqlCommand (query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) 
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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;
            SqlConnection connection = new SqlConnection (clsDataAccsessSetting.connectionString);              
            string query = @"ISERT INTO ApplicationTypes(ApplicationTypeTitle,ApplicationFees)
                            Values(@Title, @Fees)
                            SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null && int.TryParse(result.ToString(),out int insertedID)) 
                {
                    ApplicationTypeID = insertedID;
                }
            }
            catch (Exception ex) 
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }
            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Fees", Fees);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
;           }
            catch(Exception ex)
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }


    }
}
