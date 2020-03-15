using Com.Qazima.NetCore.Library.Http.Action.Database.Structure;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Database.Generic
{
    public class Database : Action
    {
        public event EventHandler<GetEventArgs> OnGet;

        public event EventHandler<DeleteEventArgs<string>> OnDelete;

        public event EventHandler<PostEventArgs<string>> OnPost;

        public event EventHandler<PutEventArgs<string>> OnPut;

        public string ConnectionString { get; set; }

        public List<Schema> Schemas { get; set; }

        public string SqlUrl { get; set; }

        public string TableUrl { get; set; }

        public bool ExposeDataModel { get; set; }

        protected virtual DbCommand GetCommand(string cmdText, DbConnection dbConnection)
        {
            return null;
        }

        protected virtual DbConnection GetConnection(string connectionString)
        {
            return null;
        }

        public virtual SchemaQuery SchemaQuery { get; }

        public virtual TableQuery TableQuery { get; }

        public virtual ColumnQuery ColumnQuery { get; }

        public virtual PrimaryKeyQuery PrimaryKeyQuery { get; }

        public virtual PrimaryKeyColumnsQuery PrimaryKeyColumnsQuery { get; }

        public virtual ForeignKeyQuery ForeignKeyQuery { get; }

        public virtual ForeignKeyColumnsQuery ForeignKeyColumnsQuery { get; }

        public void FetchSchemas()
        {
            List<Schema> schemas = new List<Schema>();
            using (DbConnection conn = GetConnection(ConnectionString))
            {
                conn.Open();
                // Retrieve all schemas
                SchemaQuery schemaQuery = SchemaQuery;
                using (DbCommand cmd = GetCommand(schemaQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        schemas.Add(new Schema() { Name = reader.GetString(schemaQuery.IndexOfName), Owner = reader.GetString(schemaQuery.IndexOfOwner) });
                    }
                }
                // Retrieve all tables
                TableQuery tableQuery = TableQuery;
                using (DbCommand cmd = GetCommand(TableQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema schema = schemas.FirstOrDefault(s => s.Name == reader.GetString(TableQuery.IndexOfOwner));
                        schema?.Tables.Add(new Structure.Table() { Name = reader.GetString(TableQuery.IndexOfName) });
                    }
                }
                // Retrieve all columns
                ColumnQuery columnQuery = ColumnQuery;
                using (DbCommand cmd = GetCommand(ColumnQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema schema = schemas.FirstOrDefault(s => s.Name == reader.GetString(ColumnQuery.IndexOfTableSchemaName));
                        Structure.Table table = schema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(ColumnQuery.IndexOfTableName));
                        table?.Columns.Add(new Column() { Name = reader.GetString(ColumnQuery.IndexOfName), Type = reader.GetString(ColumnQuery.IndexOfType) });
                    }
                }
                // Retrieve all primary keys
                PrimaryKeyQuery primaryKeyQuery = PrimaryKeyQuery;
                using (DbCommand cmd = GetCommand(primaryKeyQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyQuery.IndexOfTableSchemaName));
                        Schema primaryKeychema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyQuery.IndexOfPrimaryKeySchemaName));
                        Structure.Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyQuery.IndexOfTableName));
                        PrimaryKey primaryKey = new PrimaryKey() { Name = reader.GetString(primaryKeyQuery.IndexOfName) };
                        primaryKeychema?.PrimaryKeys.Add(primaryKey);
                        table?.PrimaryKeys.Add(primaryKey);
                    }
                }
                // Retrieve all primary keys column
                PrimaryKeyColumnsQuery primaryKeyColumnsQuery = PrimaryKeyColumnsQuery;
                using (DbCommand cmd = GetCommand(primaryKeyColumnsQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema primaryKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfPrimaryKeySchemaName));
                        Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfTableSchemaName));
                        Structure.Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfTableName));
                        PrimaryKey primaryKey = primaryKeySchema?.PrimaryKeys.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfPrimaryKeyName));
                        Column column = table?.Columns.FirstOrDefault(c => c.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfName));
                        primaryKey?.Columns.Add(column);
                    }
                }
                // Retrieve all foreign keys
                ForeignKeyQuery foreignKeyQuery = ForeignKeyQuery;
                using (DbCommand cmd = GetCommand(foreignKeyQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema primaryKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfReferencedConstraintSchemaName));
                        Schema foreignKeyShema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfForeignKeySchemaName));
                        Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfTableSchemaName));
                        PrimaryKey primaryKey = primaryKeySchema?.PrimaryKeys.FirstOrDefault(pk => pk.Name == reader.GetString(foreignKeyQuery.IndexOfReferencedConstraintName));
                        Structure.Table table = foreignKeyShema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyQuery.IndexOfTableName));
                        ForeignKey foreignKey = new ForeignKey() { Name = reader.GetString(foreignKeyQuery.IndexOfName), ReferencedConstraint = primaryKey };
                        table?.ForeignKeys.Add(foreignKey);
                        foreignKeyShema?.ForeignKeys.Add(foreignKey);
                    }
                }
                // Retrieve all foreign keys column
                ForeignKeyColumnsQuery foreignKeyColumnsQuery = ForeignKeyColumnsQuery;
                using (DbCommand cmd = GetCommand(foreignKeyColumnsQuery.Query, conn))
                {
                    using DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfTableSchemaName));
                        Schema foreignKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfForeignKeySchemaName));
                        Structure.Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfTableName));
                        ForeignKey foreignKey = tableSchema?.ForeignKeys.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfForeignKeyName));
                        Column column = table?.Columns.FirstOrDefault(c => c.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfName));
                        foreignKey?.Columns.Add(column);
                    }
                }
                conn.Close();
            }

            Schemas = schemas;
        }

        public override bool Process(HttpListenerContext context, string rawUrl)
        {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    result = ProcessGet(context, rawUrl);
                    break;
                case "HEAD":
                    FetchSchemas();
                    result = true;
                    break;
                case "POST":
                    result = ProcessPost(context, rawUrl);
                    break;
                case "PUT":
                    result = ProcessPut(context, rawUrl);
                    break;
                case "DELETE":
                    result = ProcessDelete(context, rawUrl);
                    break;
            }

            return result;
        }

        #region Get
        public bool ProcessGet(HttpListenerContext context, string rawUrl)
        {
            if (rawUrl.EndsWith("/"))
            {
                rawUrl = rawUrl.Substring(0, rawUrl.Length - 1);
            }

            bool result;
            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                result = ProcessGetSchema(context);
            }
            else if (rawUrl.ToLower().StartsWith("/" + SqlUrl + "/"))
            {
                result = ProcessGetSql(context, rawUrl.Substring(("/" + SqlUrl + "/").Length));
            }
            else if (rawUrl.ToLower().StartsWith("/" + TableUrl + "/"))
            {
                string[] args = rawUrl.Substring(("/" + TableUrl + "/").Length).Split('/');
                string objectName = args[args.Length - 1];
                string catalogName = args.Length <= 1 ? "" : args[0];
                result = ProcessGetTable(context, objectName, catalogName);
            }
            else
            {
                result = Process404(context);
            }
            return result;
        }

        protected virtual void OnGetAction(GetEventArgs e)
        {
            OnGet?.Invoke(this, e);
        }

        protected bool ProcessGetSchema(HttpListenerContext context)
        {
            GetEventArgs eventArgs = new GetEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try
            {
                DateTime currDate = DateTime.Now;
                string strItem = null;
                if (ExposeDataModel)
                {
                    context.Response.ContentType = "application/json";
                    strItem = JsonSerializer.Serialize(Schemas);
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    strItem = " ";
                }

                byte[] buffer = Encoding.UTF8.GetBytes(strItem);
                int bytesCount = buffer.Length;
                //Adding permanent http response headers
                context.Response.ContentLength64 = bytesCount;
                context.Response.AddHeader("Date", currDate.ToString("r"));
                context.Response.AddHeader("Last-Modified", currDate.ToString("r"));
                context.Response.OutputStream.Write(buffer, 0, bytesCount);
                context.Response.OutputStream.Flush();
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnGetAction(eventArgs);

            return result;
        }

        protected virtual bool ProcessGetSql(HttpListenerContext context, string sqlQuery)
        {
            return true;
        }

        protected virtual bool ProcessGetTable(HttpListenerContext context, string objectName, string catalogName)
        {
            return true;
        }
        #endregion Get

        #region Post
        public virtual bool ProcessPost(HttpListenerContext context, string rawUrl)
        {
            return true;
        }

        protected virtual bool ProcessPostTable(HttpListenerContext context, string objectName, string catalogName)
        {
            return true;
        }

        protected virtual void OnPostAction(PostEventArgs<string> e)
        {
            OnPost?.Invoke(this, e);
        }
        #endregion Post

        #region Put
        public virtual bool ProcessPut(HttpListenerContext context, string rawUrl)
        {
            return true;
        }

        protected virtual bool ProcessPutTable(HttpListenerContext context, string objectName, string catalogName)
        {
            return true;
        }

        protected virtual void OnPutAction(PutEventArgs<string> e)
        {
            OnPut?.Invoke(this, e);
        }
        #endregion Put

        #region Delete
        public virtual bool ProcessDelete(HttpListenerContext context, string rawUrl)
        {
            return true;
        }

        protected virtual bool ProcessDeleteTable(HttpListenerContext context, string objectName, string catalogName)
        {
            return true;
        }

        protected virtual void OnDeleteAction(DeleteEventArgs<string> e)
        {
            OnDelete?.Invoke(this, e);
        }
        #endregion Delete
    }
}
