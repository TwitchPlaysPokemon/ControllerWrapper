using System;
using System.Net;

namespace ControllerWrapper
{
    class ImpatientWebClient : WebClient
    {
        private int Timeout;

        public ImpatientWebClient(int timeout = 100) : base()
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest request = base.GetWebRequest(uri);
            //request.Timeout = Timeout;
            return request;
        }
    }
}
