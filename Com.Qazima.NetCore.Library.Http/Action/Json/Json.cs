using Com.Qazima.NetCore.Library.Attribute;
using Com.Qazima.NetCore.Library.Http.Action.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Com.Qazima.NetCore.Library.Http.Action.Json {
    public class Json<ListObjectType, ObjectType> : JsonReadOnly<ListObjectType, ObjectType> where ListObjectType : IList<ObjectType> {
        public event EventHandler<DeleteEventArgs<ObjectType>> OnDelete;

        public event EventHandler<PostEventArgs<ObjectType>> OnPost;

        public event EventHandler<PutEventArgs<ObjectType>> OnPut;

        public Json(ListObjectType item) : base(item) { }

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

        protected virtual void OnDeleteAction(DeleteEventArgs<ObjectType> e) {
            OnDelete?.Invoke(this, e);
        }

        protected virtual void OnPostAction(PostEventArgs<ObjectType> e) {
            OnPost?.Invoke(this, e);
        }

        protected virtual void OnPutAction(PutEventArgs<ObjectType> e) {
            OnPut?.Invoke(this, e);
        }

        protected bool ProcessPost(HttpListenerContext context) {
            PostEventArgs<ObjectType> eventArgs = new PostEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
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

        protected bool ProcessPut(HttpListenerContext context) {
            PutEventArgs<ObjectType> eventArgs = new PutEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
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

        protected bool ProcessDelete(HttpListenerContext context) {
            DeleteEventArgs<ObjectType> eventArgs = new DeleteEventArgs<ObjectType>() { AskedDate = DateTime.Now, AskedUrl = context.Request.Url };
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
