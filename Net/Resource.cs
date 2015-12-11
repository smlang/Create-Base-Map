using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Net
{
    public class Resource : IDisposable
    {
        public Stream Stream;
        private HttpWebResponse _response;

        public Resource(string sourceUrl)
        {
            WebRequest request = WebRequest.Create(sourceUrl);
            request.UseDefaultCredentials = true;

            _response = (HttpWebResponse)request.GetResponse();
            try
            {
                if (_response == null)
                {
                    throw new ApplicationException(String.Format("Failed to download {0}, no response", sourceUrl));
                }

                if (_response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException(String.Format("Failed to download {0}, {1}", sourceUrl, _response.StatusDescription));
                }

                this.Stream = _response.GetResponseStream();
            }
            catch
            {
                _response.Close();
                _response = null;
                throw;
            }
        }

        public void Dispose()
        {
            if (_response != null)
            {
                if (Stream != null)
                {
                    Stream.Dispose();
                    Stream = null;
                }
                _response.Close();
                _response = null;
            }
        }
    }
}
