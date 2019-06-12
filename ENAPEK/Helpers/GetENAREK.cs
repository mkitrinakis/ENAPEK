﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data; 
using System.Data.SqlClient;
using System.Data.Sql; 

namespace ENAPEK.Helpers
{

   public struct StructENAPEK
    {
       
        public void IsENAREKError(string msg)
        {
            success = false;
            error = msg;
            ENAREK = ""; 
        }
        public bool success;
        public string error ; // in case of errors 
        public string ENAREK; 
        public bool IsNew ;  
    }

    public class GetENAREK
    {

        

        private string errorMessage = ""; 

        // SQL  
        public StructENAPEK byAMKAOnlyQuery(string AMKA)
        {
            StructENAPEK response = new StructENAPEK(); 
            string rs = getFromAMKATable(AMKA);
            Log.write("rs = " + rs);
            if (!errorMessage.Equals("")) { response.IsENAREKError(errorMessage); return response; }
            response.ENAREK = rs;
            response.error = "";
            response.IsNew = true;
            response.success = true;
            return response; 
        }

        public StructENAPEK byAMKAQueryUpdate(string AMKA)
        {
            StructENAPEK response = new StructENAPEK();
            string rs = getFromAMKATable(AMKA);
            if (!errorMessage.Equals("")) { response.IsENAREKError(errorMessage); return response; }
            if (!rs.Trim().Equals(""))
            {
                response.success = true;
                response.error = ""; 
                response.ENAREK = rs;
                response.IsNew = true;
                return response; 
            }
            else
            {
                rs = insertToAMKATable(AMKA);
                if (!errorMessage.Equals("")) { response.IsENAREKError(errorMessage); return response; }
                response.success = true;
                response.error = "";
                response.ENAREK = rs;
                response.IsNew = false;
                return response;
            }
        }
        


        //public string byAllOnlyQuery(string AMKA, string SurName, string FirstName, string FatherName, string MotherName, DateTime BirthDate)
        //{
        //    string testResult = testAMKA(AMKA, SurName, FirstName, FatherName, MotherName, BirthDate);
        //    // 
        //    if (testResult.Equals(""))
        //    {
        //        return byAMKAOnlyQuery(AMKA); 
        //    }
        //        else
        //    {
        //        return testResult; 
        //    }
        //}

        //public string byAllQueryUpdate(string AMKA, string SurName, string FirstName, string FatherName, string MotherName, DateTime BirthDate)
        //{
        //    string testResult = testAMKA(AMKA, SurName,FirstName, FatherName, MotherName, BirthDate );
           
        //    if (testResult.Equals(""))
        //    {
        //        return byAMKAQueryUpdate(AMKA);
        //    }
        //    else
        //    {
        //        return testResult;
        //    }
        //}

        private string testAMKA(string AMKA, string Surname, string FirstName, string FatherName, string MotherName, DateTime BirthDate)
        {
            return "ERROR : system error Bullshit"; 
        }



        private int checkENAREKExistsAMKATable(string ENAREK)
        {
            // 1 exists
            // 0 does not exist
            // -1 error (check global var ErrorMessage 
            try
            {
              
                SqlConnection con = getConnection();
                SqlDataReader rdr = null;
                string rs = "";
                SqlCommand cmd = new SqlCommand("dbo.CheckByENAREK");
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@ENAREK", SqlDbType.NVarChar);
                param.Value = ENAREK;
                cmd.Parameters.Add(param);
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows) { errorMessage = "ERROR: NO RESULTS"; return -1; }
                rdr.Read();

                if (((int)rdr[0]).Equals(1))
                { return 1; }
                else { return 0; }
            }
            catch (Exception e) { errorMessage = "ERROR: " + e.Message; return -1; }
        }
            

        private string getFromAMKATable(string AMKA)
        {
            try
            {
                errorMessage = ""; 
                SqlConnection con = getConnection();
                SqlDataReader rdr = null;
                string rs = "";
                SqlCommand cmd = new SqlCommand("dbo.getByAMKA");
                cmd.Connection = con; 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@AMKA", SqlDbType.NVarChar);
                 param.Value = AMKA;
                // param.Value = "14087212345";
                cmd.Parameters.Add(param); 

                // cmd.Parameters.Add(new SqlParameter() { });
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows) return "";
                int counter = 0;
                while (rdr.Read())
                {
                    counter++;
                    rs = (string)rdr["ENAREK"];
                }
                if (counter > 1) { errorMessage =  "ERROR: ΠΑΝΩ ΑΠΟ ΕΝΑ ΕΝΑΡΕΚ (" + counter + " στο σύνολο) ΒΡΕΘΗΚΑΝ ΜΕ ΑΥΤΟ ΤΟ ΑΜΚΑ, Παρακαλώ Επικοινωνήστε με την Διαχείριση να αναφέρετε το Πρόβλημα"; return ""; }
                else
                {
                    return rs;
                }
            }
            catch (Exception e)
            {
                errorMessage = "ERROR: Δεν μπόρεσε να αναζητηθεί ο ΕΝΑΡΕΚ στην βάση, συστημικό error -->" + e.Message + "--> " + e.StackTrace ; return "" ;
            }
        }

        private string insertToAMKATable(string AMKA)
        {
            try
            {
                errorMessage = "";
                SqlConnection con = getConnection();
                
                string rs = "";
                SqlCommand cmd = new SqlCommand("dbo.SetbyAMKA");
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                string ENAREK = RandomGenerator.getID(); 
                
                SqlParameter paramAMKA = new SqlParameter("@AMKA", SqlDbType.NVarChar);
                SqlParameter paramENAREK = new SqlParameter("@ENAREK", SqlDbType.NVarChar);
                SqlParameter paramCreationDate = new SqlParameter("@CreationDate", SqlDbType.DateTime);
                paramAMKA.Value = AMKA;
                paramCreationDate.Value = System.DateTime.Now;
                paramENAREK.Value = ENAREK; 

                // param.Value = "14087212345";
                cmd.Parameters.Add(paramAMKA);
                cmd.Parameters.Add(paramCreationDate);
                cmd.Parameters.Add(paramENAREK);

                // cmd.Parameters.Add(new SqlParameter() { });
                cmd.ExecuteNonQuery();
                
                
                
                if (!ENAREK.Equals(getFromAMKATable(AMKA))) { errorMessage = "ERROR3: Κάποιο παραμετρικό πρόβλημα κατά την ενημέρωση του ΕΝΑΡΕΚ, Παρακαλώ Επικοινωνήστε με την Διαχείριση να αναφέρετε το Πρόβλημα"; return ""; }
                else {
                    return ENAREK;
                }
            }
            catch (Exception e)
            {
                errorMessage = "ERROR: Δεν μπόρεσε να αναζητηθεί ο ΕΝΑΡΕΚ στην βάση, συστημικό error -->" + e.Message + "--> " + e.StackTrace; return "";
            }
        }

        private SqlConnection getConnection()
        {
            try
            {
                //string connectionString = System.Web.Configuration.WebConfigurationManager.AppSettings["SQLConnectionString"];  // connectionString = "Data Source=" + oracle_datasource + ";User ID=" + oracle_userid + ";Password=" + oracle_password + "";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder["Data Source"] = "WIN81-A0389\\SQLEXPRESS";
                builder["integrated Security"] = true;
                builder["Initial Catalog"] = "ENAREK";
                
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = builder.ConnectionString;
                Log.write(conn.ConnectionString); 
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Log.write( "getSQLAdapter:" + e.Message);
                return null;
            }
        }

        
    }
}