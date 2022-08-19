﻿using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using ToSic.Sxc.Dnn;

// This is a special workaround for DNN 9.4
// Review the readme.md to understand how and why

// ReSharper disable once CheckNamespace
namespace ToSic.Sxc.Dnn940
{
    public class WebApiHttpControllerActivator : IHttpControllerActivator
    {
        public IHttpControllerActivator PreviousActivator { get; set; }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            // first try to just get it from the DI - if it's there
            // note that the PreviousActivator doesn't exist
            var resultFromDnnActivator = PreviousActivator?.Create(request, controllerDescriptor, controllerType);
            if (resultFromDnnActivator != null) return resultFromDnnActivator;

            // If it's not found (null), then it's probably a dynamically compiled type from a .cs file or similar
            // Such types are never registered in the DI catalog, as they may change on-the-fly.
            // In this case we must use ActivatorUtilities, which will create the object and if it expects 
            // any DI parameters, they will come from the DependencyInjection as should be best practice
            var dnnGlobalDi = DnnStaticDi.GetGlobalServiceProvider(); // 2022-08-11 2dm cleaned up, shouldn't use duplicate code to get dnn internal object
            var resultFromUtilities = (IHttpController)ActivatorUtilities.CreateInstance(dnnGlobalDi, controllerType);
            return resultFromUtilities;
        }
    }
}