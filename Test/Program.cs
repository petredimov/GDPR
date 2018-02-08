using DatabaseNamespace;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=STOLEPC\SENSE;Initial Catalog=Test;User Id=sa;Password=Sense17*; Integrated Security=True";

            Exporter exporter = new Exporter(connectionString);
            exporter.BackupDatabase(@"D:\database.bak");

            exporter.SqlToCsv("SELECT * FROM Test.dbo.ContractUsers", @"D:\contractUsers.csv");

            Console.ReadLine();

        }
    }
}
