using Com.Qazima.NetCore.Library.Http.Action.Database.Structure;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public class DatabaseAction : Action, IDatabaseAction {
        /// <summary>
        /// Event handler for the Get action
        /// </summary>
        public event EventHandler<ActionGetEventArgs> OnActionGet;

        /// <summary>
        /// Event handler for the Delete action
        /// </summary>
        public event EventHandler<ActionDeleteEventArgs<string>> OnActionDelete;

        /// <summary>
        /// Event handler for the Post action
        /// </summary>
        public event EventHandler<ActionPostEventArgs<string>> OnActionPost;

        /// <summary>
        /// Event handler for the Put action
        /// </summary>
        public event EventHandler<ActionPutEventArgs<string>> OnActionPut;

        /// <summary>
        /// The connection string to the database
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The list of schemas
        /// </summary>
        public List<Schema> Schemas { get; set; }

        public string SqlUrl { get; set; }

        public string TableUrl { get; set; }

        /// <summary>
        /// Have to expose the list of schemas
        /// </summary>
        public bool ExposeDataModel { get; set; }

        public virtual DbCommand GetCommand(string cmdText, DbConnection dbConnection) {
            return null;
        }

        public virtual DbConnection GetConnection(string connectionString) {
            return null;
        }

        public virtual SchemaQuery SchemaQuery { get; }

        public virtual TableQuery TableQuery { get; }

        public virtual ColumnQuery ColumnQuery { get; }

        public virtual PrimaryKeyQuery PrimaryKeyQuery { get; }

        public virtual PrimaryKeyColumnsQuery PrimaryKeyColumnsQuery { get; }

        public virtual ForeignKeyQuery ForeignKeyQuery { get; }

        public virtual ForeignKeyColumnsQuery ForeignKeyColumnsQuery { get; }

        public void FetchSchemas() {
            List<Schema> schemas = new List<Schema>();
            using (DbConnection conn = GetConnection(ConnectionString)) {
                conn.Open();
                // Retrieve all schemas
                SchemaQuery schemaQuery = SchemaQuery;
                using (DbCommand cmd = GetCommand(schemaQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            schemas.Add(new Schema() { Name = reader.GetString(schemaQuery.IndexOfName), Owner = reader.GetString(schemaQuery.IndexOfOwner) });
                        }
                    }
                }
                // Retrieve all tables
                TableQuery tableQuery = TableQuery;
                using (DbCommand cmd = GetCommand(TableQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema schema = schemas.FirstOrDefault(s => s.Name == reader.GetString(TableQuery.IndexOfOwner));
                            schema?.Tables.Add(new Table() { Name = reader.GetString(TableQuery.IndexOfName) });
                        }
                    }
                }
                // Retrieve all columns
                ColumnQuery columnQuery = ColumnQuery;
                using (DbCommand cmd = GetCommand(ColumnQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema schema = schemas.FirstOrDefault(s => s.Name == reader.GetString(ColumnQuery.IndexOfTableSchemaName));
                            Table table = schema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(ColumnQuery.IndexOfTableName));
                            table?.Columns.Add(new Column() { Name = reader.GetString(ColumnQuery.IndexOfName), Type = reader.GetString(ColumnQuery.IndexOfDataType) });
                        }
                    }
                }
                // Retrieve all primary keys
                PrimaryKeyQuery primaryKeyQuery = PrimaryKeyQuery;
                using (DbCommand cmd = GetCommand(primaryKeyQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyQuery.IndexOfTableSchemaName));
                            Schema primaryKeychema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyQuery.IndexOfPrimaryKeySchemaName));
                            Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyQuery.IndexOfTableName));
                            PrimaryKey primaryKey = new PrimaryKey() { Name = reader.GetString(primaryKeyQuery.IndexOfName) };
                            primaryKeychema?.PrimaryKeys.Add(primaryKey);
                            table?.PrimaryKeys.Add(primaryKey);
                        }
                    }
                }
                // Retrieve all primary keys column
                PrimaryKeyColumnsQuery primaryKeyColumnsQuery = PrimaryKeyColumnsQuery;
                using (DbCommand cmd = GetCommand(primaryKeyColumnsQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema primaryKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfPrimaryKeySchemaName));
                            Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfTableSchemaName));
                            Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfTableName));
                            PrimaryKey primaryKey = primaryKeySchema?.PrimaryKeys.FirstOrDefault(t => t.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfPrimaryKeyName));
                            Column column = table?.Columns.FirstOrDefault(c => c.Name == reader.GetString(primaryKeyColumnsQuery.IndexOfName));
                            primaryKey?.Columns.Add(column);
                        }
                    }
                }
                // Retrieve all foreign keys
                ForeignKeyQuery foreignKeyQuery = ForeignKeyQuery;
                using (DbCommand cmd = GetCommand(foreignKeyQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema primaryKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfReferencedConstraintSchemaName));
                            Schema foreignKeyShema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfForeignKeySchemaName));
                            Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyQuery.IndexOfTableSchemaName));
                            PrimaryKey primaryKey = primaryKeySchema?.PrimaryKeys.FirstOrDefault(pk => pk.Name == reader.GetString(foreignKeyQuery.IndexOfReferencedConstraintName));
                            Table table = foreignKeyShema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyQuery.IndexOfTableName));
                            ForeignKey foreignKey = new ForeignKey() { Name = reader.GetString(foreignKeyQuery.IndexOfName), ReferencedConstraint = primaryKey };
                            table?.ForeignKeys.Add(foreignKey);
                            foreignKeyShema?.ForeignKeys.Add(foreignKey);
                        }
                    }
                }
                // Retrieve all foreign keys column
                ForeignKeyColumnsQuery foreignKeyColumnsQuery = ForeignKeyColumnsQuery;
                using (DbCommand cmd = GetCommand(foreignKeyColumnsQuery.Query, conn)) {
                    using (DbDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Schema tableSchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfTableSchemaName));
                            Schema foreignKeySchema = schemas.FirstOrDefault(s => s.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfForeignKeySchemaName));
                            Table table = tableSchema?.Tables.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfTableName));
                            ForeignKey foreignKey = tableSchema?.ForeignKeys.FirstOrDefault(t => t.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfForeignKeyName));
                            Column column = table?.Columns.FirstOrDefault(c => c.Name == reader.GetString(foreignKeyColumnsQuery.IndexOfName));
                            foreignKey?.Columns.Add(column);
                        }
                    }
                }
            }

            Schemas = schemas;
        }

        public bool Process(HttpListenerContext context, string rawUrl) {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper()) {
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
        /// <summary>
        /// Process the get of the current context
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="rawUrl">Raw url</param>
        /// <returns>True if every thing was fine</returns>
        public bool ProcessGet(HttpListenerContext context, string rawUrl) {
            if (rawUrl.EndsWith("/")) {
                rawUrl = rawUrl.Substring(0, rawUrl.Length - 1);
            }

            bool result;
            if (string.IsNullOrWhiteSpace(rawUrl)) {
                result = ProcessGetSchema(context);
            } else if (rawUrl.ToLower().StartsWith("/" + SqlUrl + "/")) {
                result = ProcessGetSql(context, rawUrl.Substring(("/" + SqlUrl + "/").Length));
            } else if (rawUrl.ToLower().StartsWith("/" + TableUrl + "/")) {
                string[] args = rawUrl.Substring(("/" + TableUrl + "/").Length).Split('/');
                string objectName = args[args.Length - 1];
                string catalogName = args.Length <= 1 ? "" : args[0];
                result = ProcessGetTable(context, objectName, catalogName);
            } else {
                result = Process404(context);
            }
            return result;
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnGetAction(ActionGetEventArgs e) {
            OnActionGet?.Invoke(this, e);
        }

        /// <summary>
        /// Get the schemas
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>True if every thing was fine</returns>
        protected bool ProcessGetSchema(HttpListenerContext context) {
            ActionGetEventArgs eventArgs = new ActionGetEventArgs() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try {
                DateTime currDate = DateTime.Now;
                string strItem = null;
                if (ExposeDataModel) {
                    context.Response.ContentType = "application/json";
                    strItem = JsonSerializer.Serialize(Schemas);
                } else {
                    context.Response.ContentType = "text/plain";
                    strItem = " ";
                }

                byte[] buffer = Encoding.UTF8.GetBytes(strItem.ToCharArray());
                int bytesCount = buffer.Length;
                //Adding permanent http response headers
                context.Response.ContentLength64 = bytesCount;
                context.Response.AddHeader("Date", currDate.ToString("r"));
                context.Response.AddHeader("Last-Modified", currDate.ToString("r"));
                context.Response.OutputStream.Write(buffer, 0, bytesCount);
                context.Response.OutputStream.Flush();
            } catch (Exception) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnGetAction(eventArgs);

            return result;
        }

        /// <summary>
        /// Get the result of a sql query
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="sqlQuery">Sql query</param>
        /// <returns>True if every thing was fine</returns>
        protected virtual bool ProcessGetSql(HttpListenerContext context, string sqlQuery) {
            return true;
        }

        /// <summary>
        /// Get the contents of a table or a view
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="objectName">Table or view name</param>
        /// <param name="catalogName">Catalog name</param>
        /// <returns>True if every thing was fine</returns>
        protected virtual bool ProcessGetTable(HttpListenerContext context, string objectName, string catalogName) {
            return true;
        }
        #endregion Get

        #region Post
        /// <summary>
        /// Process the post of the current context
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="rawUrl">Raw url</param>
        /// <returns>True if every thing was fine</returns>
        public virtual bool ProcessPost(HttpListenerContext context, string rawUrl) {
            return true;
        }

        /// <summary>
        /// Insert into a table or a view
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="objectName">Table or view name</param>
        /// <param name="catalogName">Catalog name</param>
        /// <returns>True if every thing was fine</returns>
        protected virtual bool ProcessPostTable(HttpListenerContext context, string objectName, string catalogName) {
            return true;
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPostAction(ActionPostEventArgs<string> e) {
            OnActionPost?.Invoke(this, e);
        }
        #endregion Post

        #region Put
        /// <summary>
        /// Process the put of the current context
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="rawUrl">Raw url</param>
        /// <returns>True if every thing was fine</returns>
        public virtual bool ProcessPut(HttpListenerContext context, string rawUrl) {
            return true;
        }

        /// <summary>
        /// Update a table or a view
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="objectName">Table or view name</param>
        /// <param name="catalogName">Catalog name</param>
        /// <returns>True if every thing was fine</returns>
        protected virtual bool ProcessPutTable(HttpListenerContext context, string objectName, string catalogName) {
            return true;
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPutAction(ActionPutEventArgs<string> e) {
            OnActionPut?.Invoke(this, e);
        }
        #endregion Put

        #region Delete
        /// <summary>
        /// Process the delete of the current context
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="rawUrl">Raw url</param>
        /// <returns>True if every thing was fine</returns>
        public virtual bool ProcessDelete(HttpListenerContext context, string rawUrl) {
            return true;
        }

        /// <summary>
        /// Delete from a table or a view
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="objectName">Table or view name</param>
        /// <param name="catalogName">Catalog name</param>
        /// <returns>True if every thing was fine</returns>
        protected virtual bool ProcessDeleteTable(HttpListenerContext context, string objectName, string catalogName) {
            return true;
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnDeleteAction(ActionDeleteEventArgs<string> e) {
            OnActionDelete?.Invoke(this, e);
        }
        #endregion Delete

        /// <summary>
        /// Process the 404 Error code
        /// </summary>
        /// <param name="context">Current context</param>
        /// <returns>True if every thing was fine</returns>
        protected bool Process404(HttpListenerContext context) {
            return ProcessError(context, HttpStatusCode.NotFound);
        }
    }
}
