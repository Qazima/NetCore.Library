using Com.Qazima.NetCore.Library.Http;
using d=Com.Qazima.NetCore.Library.Http.Action.Directory;
using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Qazima.NetCore.Test {
    class Program {
        static void Main(string[] args) {
            Server server = new Server(new ServerPrefixe("api.qazima.com", 8090));
            int padAmount = 50;

            string path = Path.Combine("C:", "DEV", "html", "www");
            Console.Write(string.Format("Direction action: {0}", path).PadRight(padAmount));
            d.Directory directory = new d.Directory(path, new List<string>() { "index.html" });
            server.AddAction("/", directory);
            Console.WriteLine("[OK]");

            //Console.Write("Linking Article Table".PadRight(padAmount));
            //TableReadOnlyAction oracleTableArticleAction = new TableReadOnlyAction("Data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User id=Bm2s; Password=BM2S;", "ARTICLE");
            //oracleTableArticleAction.FilterableColumns.Add("ID");
            //oracleTableArticleAction.VisibleColumns.Add("CODE");
            //oracleTableArticleAction.VisibleColumns.Add("DESIGNATION");
            //server.AddAction("/ro/article", oracleTableArticleAction);
            //Console.WriteLine("[OK]");

            Console.Write(("Starting webserver").PadRight(padAmount));
            server.Start();
            Console.WriteLine("[OK]");
        }
    }
}
