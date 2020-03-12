using Com.Qazima.NetCore.Library.Attribute;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Json {
    public class Json<ListObjectType, ObjectType> : JsonReadOnly<ListObjectType, ObjectType> where ListObjectType : IList<ObjectType> {
        /// <summary>
        /// Event handler for the Delete action
        /// </summary>
        public event EventHandler<ActionDeleteEventArgs<ObjectType>> OnActionDelete;

        /// <summary>
        /// Event handler for the Post action
        /// </summary>
        public event EventHandler<ActionPostEventArgs<ObjectType>> OnActionPost;

        /// <summary>
        /// Event handler for the Put action
        /// </summary>
        public event EventHandler<ActionPutEventArgs<ObjectType>> OnActionPut;

        /// <summary>
        /// Constructor of a json action
        /// </summary>
        /// <param name="item">item to parse</param>
        public Json(ListObjectType item) : base(item) { }

        /// <summary>
        /// Process the current context
        /// </summary>
        /// <param name="context">Context to process</param>
        /// <param name="rawUrl">Raw url without the mount point</param>
        /// <returns>True if handled, else false</returns>
        public override bool Process(HttpListenerContext context, string rawUrl) {
            bool result = false;
            switch (context.Request.HttpMethod.ToUpper()) {
                case "GET":
                    result = ProcessGet(context);
                    break;
                case "HEAD":
                    result = ProcessHead(context);
                    break;
                case "POST":
                    result = ProcessPost(context);
                    break;
                case "PUT":
                    result = ProcessPut(context);
                    break;
                case "DELETE":
                    result = ProcessDelete(context);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnDeleteAction(ActionDeleteEventArgs<ObjectType> e) {
            OnActionDelete?.Invoke(this, e);
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPostAction(ActionPostEventArgs<ObjectType> e) {
            OnActionPost?.Invoke(this, e);
        }

        /// <summary>
        /// Fire event on an action
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPutAction(ActionPutEventArgs<ObjectType> e) {
            OnActionPut?.Invoke(this, e);
        }

        /// <summary>
        /// Generate object from Json flux and add it to the collection
        /// </summary>
        /// <param name="context">Context to process</param>
        /// <returns>True if handled, else false</returns>
        protected bool ProcessPost(HttpListenerContext context) {
            ActionPostEventArgs<ObjectType> eventArgs = new ActionPostEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try {
                if (!context.Request.HasEntityBody) {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    result = false;
                } else {
                    string parameters = null;
                    using (System.IO.Stream body = context.Request.InputStream) {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(body, context.Request.ContentEncoding)) {
                            parameters = reader.ReadToEnd();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(parameters)) {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        result = false;
                    } else {
                        ObjectType objFromParameters = JsonSerializer.Deserialize<ObjectType>(parameters);

                        if (!Item.Any(item => item.GetType().GetProperties().Where(prop => System.Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))).All(prop => prop.GetValue(item).Equals(prop.GetValue(objFromParameters))))) {
                            Item.Add(objFromParameters);
                            eventArgs.New = objFromParameters;
                        } else {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            result = false;
                        }
                    }
                }
            } catch {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPostAction(eventArgs);

            PrepareResponse(context);
            return result;
        }

        /// <summary>
        /// Generate object from Json flux and replace the old one (based on fields with primary key attribute) with the new one
        /// </summary>
        /// <param name="context">Context to process</param>
        /// <returns>True if handled, else false</returns>
        protected bool ProcessPut(HttpListenerContext context) {
            ActionPutEventArgs<ObjectType> eventArgs = new ActionPutEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try {
                if (!context.Request.HasEntityBody) {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    result = false;
                } else {
                    string parameters = null;
                    using (System.IO.Stream body = context.Request.InputStream) {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(body, context.Request.ContentEncoding)) {
                            parameters = reader.ReadToEnd();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(parameters)) {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        result = false;
                    } else {
                        ObjectType objFromParameters = JsonSerializer.Deserialize<ObjectType>(parameters);
                        if (objFromParameters == null) {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            result = false;
                        } else {
                            eventArgs.New = objFromParameters;
                            ObjectType objFromCollection = Item.FirstOrDefault(item => item.GetType().GetProperties().Where(prop => System.Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))).All(prop => prop.GetValue(item).Equals(prop.GetValue(objFromParameters))));
                            if (objFromCollection == null) {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                result = false;
                            } else {
                                eventArgs.Old = objFromCollection;
                                int index = Item.IndexOf(objFromCollection);
                                Item[index] = objFromParameters;
                            }
                        }
                    }
                }
            } catch {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnPutAction(eventArgs);

            PrepareResponse(context);
            return result;
        }

        /// <summary>
        /// Delete object from collection (based on id)
        /// </summary>
        /// <param name="context">Context to process</param>
        /// <returns>True if handled, else false</returns>
        protected bool ProcessDelete(HttpListenerContext context) {
            ActionDeleteEventArgs<ObjectType> eventArgs = new ActionDeleteEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
            bool result = true;
            try {
                if (!context.Request.HasEntityBody) {
                    result = false;
                } else {
                    string parameters = null;
                    using (System.IO.Stream body = context.Request.InputStream) {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(body, context.Request.ContentEncoding)) {
                            parameters = reader.ReadToEnd();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(parameters)) {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        result = false;
                    } else {
                        ObjectType objFromParameters = JsonSerializer.Deserialize<ObjectType>(parameters);
                        if (objFromParameters == null) {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            result = false;
                        } else {
                            ObjectType objFromCollection = Item.FirstOrDefault(item => item.GetType().GetProperties().Where(prop => System.Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))).All(prop => prop.GetValue(item).Equals(prop.GetValue(objFromParameters))));
                            if (objFromCollection == null) {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                result = false;
                            } else {
                                eventArgs.Old = objFromCollection;
                                Item.Remove(objFromCollection);
                            }
                        }
                    }
                }
            } catch {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = false;
            }

            eventArgs.EndDate = DateTime.Now;
            eventArgs.ResponseHttpStatusCode = (HttpStatusCode)context.Response.StatusCode;
            OnDeleteAction(eventArgs);

            PrepareResponse(context);
            return result;
        }
    }
}
