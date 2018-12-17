// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.NewtonsoftJson.Test
{
    public class ControllerExtensionsTest
    {
        [Fact]
        public void Controller_Json_WithParameterValue_SetsResultData()
        {
            // Arrange
            var controller = new TestController();
            var data = new object();

            // Act
            var actualJsonResult = controller.Json(data);

            // Assert
            Assert.IsType<JsonResult>(actualJsonResult);
            Assert.Same(data, actualJsonResult.Value);
        }

        [Fact]
        public void Controller_Json_WithParameterValue_SetsRespectiveProperty()
        {
            // Arrange
            var controller = new TestController();
            var data = new object();
            var serializerSettings = new JsonSerializerSettings();

            // Act
            var actualJsonResult = controller.Json(data, serializerSettings);

            // Assert
            Assert.IsType<JsonResult>(actualJsonResult);
            Assert.Same(data, actualJsonResult.Value);
        }

        private class TestController : Controller { }
    }
}
