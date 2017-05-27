﻿using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ExpressiveAnnotations.MvcUnobtrusive.Caching;
using Moq;

namespace ExpressiveAnnotations.MvcUnobtrusive.Tests
{
    public class BaseTest
    {
        public BaseTest()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(string.Empty, "http://tempuri.org", string.Empty),
                new HttpResponse(new StringWriter())
                );

            ProcessStorage<string, CacheItem>.Clear();
        }

        protected ModelMetadata GetModelMetadata<TModel, TProp>(TModel model, Expression<Func<TModel, TProp>> expression)
        {
            var property = ((MemberExpression) expression.Body).Member.Name;
            return new ModelMetadata(ModelMetadataProviders.Current, typeof(TModel), () => model, typeof(TProp), property);
        }

        protected ControllerContext GetControllerContext()
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.HttpMethod).Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(c => c.Request).Returns(request.Object);
            var controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            return controllerContext;
        }

        protected void CulturalExecutionUI(Action action, string culture)
        {
            var temp = Thread.CurrentThread.CurrentUICulture; // backup current UI culture
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);
            action();
            Thread.CurrentThread.CurrentUICulture = temp; // restore culture
        }
    }
}
