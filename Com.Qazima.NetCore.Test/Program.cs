using Com.Qazima.NetCore.Library.Http;
using d = Com.Qazima.NetCore.Library.Http.Action.Directory;
using orcl = Com.Qazima.NetCore.Library.Http.Action.Database.Oracle;
using System;
using System.Collections.Generic;
using System.IO;
using Com.Qazima.NetCore.Library.Http.Action.Database.Generic;

namespace Com.Qazima.NetCore.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(true, 20, new ServerPrefixe("127.0.0.1", 8090));
            int padAmount = 50;

            string path = Path.Combine("E:", "DEV", "www");
            Console.Write(string.Format("Direction action: {0}", path).PadRight(padAmount));
            d.Directory directory = new d.Directory(path, new List<string>() { "index.html" });
            directory.StoreInCache = true;
            server.AddAction("/", directory);
            Console.WriteLine("[OK]");

            Console.Write("Linking Article Table".PadRight(padAmount));
            orcl.TableReadOnly oracleTableArticleAction = new orcl.TableReadOnly("Data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User id=Bm2s; Password=BM2S;", "ARTICLE");
            oracleTableArticleAction.FilterableColumns.Add("ID");
            oracleTableArticleAction.VisibleColumns.Add("ID");
            oracleTableArticleAction.VisibleColumns.Add("CODE");
            oracleTableArticleAction.VisibleColumns.Add("DESIGNATION");
            oracleTableArticleAction.OrderColumns.Add("ID", OrderType.Descending);
            oracleTableArticleAction.AllowGet = true;
            server.AddAction("/ro/article", oracleTableArticleAction);
            Console.WriteLine("[OK]");

            Console.Write(("Starting webserver").PadRight(padAmount));
            server.Start();
            Console.WriteLine("[OK]");
        }
    }
}
