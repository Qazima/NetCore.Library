using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Com.Qazima.NetCore.Library.Http.Cache
{
    public sealed class Manager
    {
        private class Element : IDisposable
        {
            public byte[] Content { get; set; }

            public string AskedUrl { get; set; }

            public DateTime Expires { get; set; }

            public string OrderedQueryString { get; set; }

            public Element(string askedUrl, byte[] content, NameValueCollection queryString, double cacheDuration)
            {
                AskedUrl = askedUrl;
                Content = content;
                Expires = DateTime.UtcNow.AddSeconds(cacheDuration);
                OrderedQueryString = GetOrderedQueryString(queryString);
            }

            public static string GetOrderedQueryString(NameValueCollection queryString)
            {
                return queryString == null ? string.Empty : string.Join("&", queryString?.AllKeys.Select(k => k + "=" + queryString[k]));
            }

            #region IDisposable Support
            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                    }

                    AskedUrl = null;
                    Content = null;
                    Expires = DateTime.MinValue;
                    OrderedQueryString = null;

                    disposedValue = true;
                }
            }

            //~Element()
            //{
            //    Dispose(false);
            //}

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
        private class Nested
        {
            internal static readonly Manager instance = new Manager();
        }

        public bool Enabled { get; set; }

        public static Manager Instance { get { return Nested.instance; } }

        private List<Element> Elements { get; set; }

        private Timer ExpireTimer { get; set; }

        public double CacheDuration { get; set; }

        private Manager()
        {
            Elements = new List<Element>();
            ExpireTimer = new Timer(1000);
            ExpireTimer.Elapsed += elapsedEvent;
            ExpireTimer.Start();
        }

        private void elapsedEvent(object sender, ElapsedEventArgs e)
        {
            RemoveExpired();
        }

        private void RemoveExpired()
        {
            if (Enabled)
            {
                DateTime current = DateTime.UtcNow;
                List<Element> elements = Elements.Where(e => e.Expires <= current).ToList();
                for (int i = 0; i < elements.Count; i++)
                {
                    Elements.Remove(elements[i]);
                    elements[i].Dispose();
                    GC.Collect();
                }
            }
        }

        public async void AddAsync(string askedUrl, NameValueCollection queryString, byte[] content)
        {
            if (Enabled)
            {
                await Task.Run(() => RemoveExpired());
                if (!Exists(queryString))
                {
                    await Task.Run(() => Elements.Add(new Element(askedUrl, content, queryString, CacheDuration)));
                }
            }
        }

        public void ClearCache()
        {
            if (Enabled)
            {
                Elements.Clear();
            }
        }

        public bool Exists(NameValueCollection queryString)
        {
            if (Enabled)
            {
                RemoveExpired();
                string orderedQueryString = Element.GetOrderedQueryString(queryString);
                return Elements.Any(e => e.OrderedQueryString == orderedQueryString && e.Expires < DateTime.UtcNow);
            }

            return true;
        }

        public bool Exists(NameValueCollection queryString, out byte[] content)
        {
            if (Enabled)
            {
                RemoveExpired();
                string orderedQueryString = Element.GetOrderedQueryString(queryString);
                Element result = Elements.FirstOrDefault(e => e.OrderedQueryString == orderedQueryString && e.Expires < DateTime.UtcNow);
                content = result?.Content;
                return result != null;
            }
            content = null;
            return true;
        }
    }
}
