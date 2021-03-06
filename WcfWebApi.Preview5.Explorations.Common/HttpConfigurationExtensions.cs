﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using Microsoft.ApplicationServer.Http;
using Microsoft.ApplicationServer.Http.Description;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace WcfWebApi.Preview5.Explorations.Common
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration AddRequestHandlers(this HttpConfiguration conf, Action<Collection<HttpOperationHandler>, ServiceEndpoint, HttpOperationDescription> requestHandlerDelegate)
        {
            var old = conf.RequestHandlers;
            conf.RequestHandlers = old == null ? requestHandlerDelegate :
                                        (coll, ep, desc) =>
                                        {
                                            old(coll, ep, desc);
                                            requestHandlerDelegate(coll, ep, desc);
                                        };
            return conf;
        }

        public static HttpConfiguration EnableAuthorizeAttribute(this HttpConfiguration conf)
        {
            conf.AddRequestHandlers((coll, ep, desc) =>
                                        {
                                            var attr =
                                                desc.Attributes.Where(t => t.GetType() == typeof (AuthorizeAttribute)).
                                                    FirstOrDefault();
                                            if (attr != null)
                                            {
                                                coll.Add(new AuthorizeOperationHandler(attr as AuthorizeAttribute));
                                            }
                                        });
            return conf;
        }
    }
}
