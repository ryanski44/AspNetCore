// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcJsonMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddNewtonsoftJsonFormatters(this IMvcCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddServicesCore(builder.Services);
            return builder;
        }

        public static IMvcCoreBuilder AddNewtonsoftJsonFormatters(
            this IMvcCoreBuilder builder,
            Action<MvcJsonOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            AddServicesCore(builder.Services);

            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="MvcJsonOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <param name="setupAction">The <see cref="MvcJsonOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcCoreBuilder"/>.</returns>
        public static IMvcCoreBuilder AddNewtonsoftJsonOptions(
           this IMvcCoreBuilder builder,
           Action<MvcJsonOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);
            return builder;
        }

        // Internal for testing.
        internal static void AddServicesCore(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcJsonMvcOptionsSetup>());
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IPostConfigureOptions<MvcJsonOptions>, MvcJsonOptionsConfigureCompatibilityOptions>());
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApiDescriptionProvider, JsonPatchOperationsArrayProvider>());
            services.TryAddSingleton<JsonResultExecutor>();

            var tempDataSerializer = services.FirstOrDefault(f => f.ServiceType == typeof(TempDataSerializer));
            if (tempDataSerializer != null)
            {
                services.Remove(tempDataSerializer);
            }
            services.TryAddSingleton<TempDataSerializer, BsonTempDataSerializer>();

            //
            // JSON Helper
            //
            var jsonHelper = services.FirstOrDefault(f => f.ServiceType == typeof(IJsonHelper));
            if (jsonHelper != null)
            {
                services.Remove(jsonHelper);
            }

            services.TryAddSingleton<IJsonHelper, JsonHelper>();
            services.TryAdd(ServiceDescriptor.Singleton(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MvcJsonOptions>>().Value;
                var charPool = serviceProvider.GetRequiredService<ArrayPool<char>>();
                return new JsonOutputFormatter(options.SerializerSettings, charPool);
            }));
        }
    }
}
