using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;

namespace MakeGeoRefBaseMap
{
    internal class NetResource
    {
        internal static Image DownloadImage(string sourceUrl, out string errorMessage)
        {
            try
            {
                WebRequest request = WebRequest.Create(sourceUrl);
                request.UseDefaultCredentials = true;
#if DEBUG
                request.Proxy.Credentials = new NetworkCredential("LangS", "xxxx", "EUROPE");         
#endif

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response == null)
                    {
                        errorMessage = String.Format("Failed to download {0}, no response", sourceUrl);
                        return null;
                    }

                    try
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            errorMessage = String.Format("Failed to download {0}, {1}", sourceUrl, response.StatusDescription);
                            return null;
                        }

                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream == null)
                            {
                                errorMessage = String.Format("Failed to download {0}, could not read response", sourceUrl);
                                return null;
                            }

                            try
                            {
                                errorMessage = null;
                                return Image.FromStream(stream, false, true);
                            }
                            finally
                            {
                                stream.Close();
                            }
                        }
                    }
                    finally
                    {
                        response.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Exception e2 = e;
                errorMessage = "Abandon, due to failed download";
                while (e2 != null)
                {
                    errorMessage += "\n" + e.Message + "\n" + e.StackTrace;
                    e2 = e2.InnerException;
                }
                return null;
            }
        }
    }
}
