using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
       
        public static bool GetTestTypeInfoByID(int TestTypeID,ref string TestTypeTitle,
            ref string TestTypeDescription,ref float TestFees)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = "select * from TestTypes where TestTypeID = @TestTypeID";
            SqlCommand command=new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        TestTypeTitle = reader["TestTypeTitle"].ToString();
                        TestTypeDescription = reader["TestTypeDescription"].ToString();
                        TestFees = Convert.ToSingle(reader["TestTypeFees"]);
                        IsFound= true;
                    }
                    else
                        IsFound = false;

                }  
            }
            catch (Exception ex)
            {
                IsFound= false;
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
            return IsFound;

        }
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = "select * from TestTypes order by TestTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                try { connection.Close(); } catch { }
            }
            return dt;
        }
        public static int AddNewTestType(string Title,string Description,float Fees)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = @"INSERT INTO TestTypes (TestTypeTitle,TestTypeDescription,TestTypeFees) 
                         Values(@TestTypeTitle,@TestTypeDescription,@TestTypeFees)
                            SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@TestTypeTitle", Title);
            command.Parameters.AddWithValue("@TestTypeDescription", Description);
            command.Parameters.AddWithValue("@TestTypeFees", Fees);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedId))
                {
                    TestID = insertedId;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                try { connection.Close(); } catch { }
            }
            return TestID;
        }
        public static bool UpdateTestType(int ID,string Title,string Description,float Fees)
        {
            int AffectedRows = 0;
            SqlConnection connection = new SqlConnection(clsDataAccsessSetting.connectionString);
            string query = @"Update TestTypes
                            Set TestTypeTitle = @TestTypeTitle,
                            TestTypeDescription = @TestTypeDescription,
                            TestTypeFees = @TestTypeFees 
                            Where TestTypeID = @TestTypeID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeTitle", Title);
            command.Parameters.AddWithValue("@TestTypeDescription", Description);
            command.Parameters.AddWithValue("@TestTypeFees", Fees);
            command.Parameters.AddWithValue("@TestTypeID", ID);
            try
            {
                connection.Open();
                AffectedRows = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(clsDataAccsessSetting.SourceName, ex.Message, EventLogEntryType.Error);
                return false;
            }
            finally { 
                try { connection.Close(); } catch { } 
            }

            return AffectedRows> 0;
        }
    }
}
