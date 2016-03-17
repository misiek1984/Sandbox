using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;

namespace LinqTo
{
    public class LinqToDataset
    {
        public void Test()
        {
            Console.WriteLine("******************** LINQ to dataset ********************");

            using (
                var c =
                    new SqlConnection(
                        ConfigurationManager.ConnectionStrings[
                            "LinqToSql.Properties.Settings.ExecutionTraceDBConnectionString"].ConnectionString))
            {
                var adapter = new SqlDataAdapter("select * from dbo.Namespaces", c);
                
                
                
                var ds = new DataSet();
                adapter.Fill(ds);

                var res = (from ns in ds.Tables[0].AsEnumerable()
                           where ns.Field<string>("Name").Contains("System")
                           orderby ns["Id"]
                           select new {Id = ns["Id"], Name = ns.Field<string>("Name")}).Take(10).ToList();

                foreach (var x in res)
                    Console.WriteLine(x);



                var ds1 = new DataSet1();
                adapter.Fill(ds1, "Namespaces");

                var res2 = (from ns in ds1.Namespaces
                            where ns.Name.Contains("System")
                            orderby ns.Id
                            select new {Id = ns.Id, Name = ns.Name}).Take(10).ToList();

                foreach (var x in res2)
                    Console.WriteLine(x);

            }
        }
    }
}
