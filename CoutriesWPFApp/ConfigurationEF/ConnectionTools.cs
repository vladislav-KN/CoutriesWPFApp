using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace CoutriesWPFApp.ConfigurationEF
{
    public static class ConnectionTools
    {
        public static void ChangeDatabase(
            this DbContext source,
            string newConnectionStringName)
        {
            try
            {
                 
                var configNameEf = string.IsNullOrEmpty(newConnectionStringName)
                    ? source.GetType().Name
                    : newConnectionStringName;
                var entityCnxStringBuilder = new EntityConnectionStringBuilder
                                                (System.Configuration.ConfigurationManager.ConnectionStrings[configNameEf].ConnectionString);
                var sqlCnxStringBuilder = new SqlConnectionStringBuilder
                                            (entityCnxStringBuilder.ProviderConnectionString);
                source.Database.Connection.ConnectionString = sqlCnxStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter("log.log"))
                {
                    string line = DateTime.Now.ToLongTimeString() + " - " + ex.Message;
                    sw.WriteLine(line);
                }
            }
        }
    }
}
