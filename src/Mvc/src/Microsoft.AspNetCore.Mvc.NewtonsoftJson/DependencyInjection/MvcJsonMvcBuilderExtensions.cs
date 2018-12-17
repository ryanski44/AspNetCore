// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions methods for configuring MVC via an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class MvcJsonMvcBuilderExtensions
    {
        /// <summary>
        /// Configures JSON.NET specific input and output formatters.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddNewtonsoftJsonFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            MvcJsonMvcCoreBuilderExtensions.AddServicesCore(builder.Services);
            return builder;
        }

        /// <summary>
        /// Configures JSON.NET specific input and output formatters.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setupAction">Callback to configure <see cref="MvcJsonOptions"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddNewtonsoftJsonFormatters(
            this IMvcBuilder builder,
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

            MvcJsonMvcCoreBuilderExtensions.AddServicesCore(builder.Services);
            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="MvcJsonOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setupAction">The <see cref="MvcJsonOptions"/> which need to be configured.</param>
        public static IMvcBuilder AddNewtonsoftJsonOptions(
            this IMvcBuilder builder,
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
    }
}
