using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Management;
using Adam.Core.Orders;
using Adam.Core.Records;
using Adam.Core.Search;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Demidov", "123456");
            Console.WriteLine(status);
            if (status != LogOnStatus.LoggedOn)
            {
                throw new UnauthorizedAccessException();
            }

            Record rec = new Record(app);
            Guid recId;
            Guid.TryParse("46e678e9-24af-4364-8926-a54600dcf188", out recId);
            rec.Load(recId);
            string s = rec.Files.LatestMaster.Path;
            Console.WriteLine(s);
            Console.ReadLine();
        }
    }
}
