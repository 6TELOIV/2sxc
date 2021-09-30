﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToSic.Sxc.Web;
using ToSic.Testing.Shared;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ToSic.Sxc.Tests.WebTests
{
    [TestClass()]
    public class LinkHelperUnknownTests: EavTestBase
    {
        [TestMethod()]
        public void ToNoUrlOrParamsTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=a#fragment", linkHelper.To(api: "", part: "full"));
        }

        [TestMethod()]
        public void ToCommonUrlsTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/", linkHelper.To(api: "/", part: "full"));
            AreEqual("https://unknown.2sxc.org/?a=1&b=2#fragment", linkHelper.To(api: "/", parameters: "a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/api", linkHelper.To(api: "/api", part: "full"));
            AreEqual("https://unknown.2sxc.org/api?a=1&b=2#fragment", linkHelper.To(api: "/api", parameters: "a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/app/", linkHelper.To(api: "/app/", part: "full"));
            AreEqual("https://unknown.2sxc.org/app/?a=1&b=2#fragment", linkHelper.To(api: "/app/", parameters: "a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/app/api", linkHelper.To(api: "/app/api", part: "full"));
            AreEqual("https://unknown.2sxc.org/app/api?a=1&b=2#fragment", linkHelper.To(api: "/app/api", parameters: "a=1&b=2#fragment", part: "full"));
        }

        [TestMethod()]
        public void ToUrlPathIsMissingTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=b&b=3&c=3#fragment", linkHelper.To(api: "", parameters: "param=b&b=3&c=3", part: "full"));
        }

        [TestMethod()]
        public void ToWithoutProtocolTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/api?param=b&b=3&c=3", linkHelper.To(api: "//unknown.2sxc.org/api", parameters: "param=b&b=3&c=3", part: "full"));
        }

        [TestMethod()]
        public void ToUrlWithTildeTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/api?p=1&r=2", linkHelper.To(api: "~/api", parameters: "p=1&r=2", part: "full"));
            AreEqual("https://unknown.2sxc.org/app/", linkHelper.To(api: "~/app/", part: "full"));
        }

        [TestMethod()]
        public void ToWithAbsoluteUrlTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown2.2sxc.org/", linkHelper.To(api: "https://unknown2.2sxc.org/", part: "full"));
            AreEqual("https://unknown2.2sxc.org/api", linkHelper.To(api: "https://unknown2.2sxc.org/api", part: "full"));
            AreEqual("https://unknown2.2sxc.org/app/api?a=1", linkHelper.To(api: "https://unknown2.2sxc.org/app/api", parameters: "a=1", part: "full"));
        }

        [TestMethod()]
        public void ToWithInvalidUrlTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("hello2:there", linkHelper.To(api: "hello2:there", part: "full"));
            AreEqual("file:123", linkHelper.To(api: "file:123", part: "full"));
            AreEqual("../api", linkHelper.To(api: "../api", part: "full"));
            AreEqual("/sibling1/../sibling2/api", linkHelper.To(api: "/sibling1/../sibling2/api", part: "full"));
        }

        [TestMethod()]
        public void ImageNoUrlOrParamsTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=a#fragment", linkHelper.Image(url: "", part: "full"));
        }

        [TestMethod()]
        public void ImageCommonUrlsTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/", linkHelper.Image(url: "/", part: "full"));
            AreEqual("https://unknown.2sxc.org/?a=1&b=2#fragment", linkHelper.Image(url: "/?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/page", linkHelper.Image(url: "/page", part: "full"));
            AreEqual("https://unknown.2sxc.org/page?a=1&b=2#fragment", linkHelper.Image(url: "/page?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/file.ext", linkHelper.Image(url: "/file.ext", part: "full"));
            AreEqual("https://unknown.2sxc.org/file.ext?a=1&b=2#fragment", linkHelper.Image(url: "/file.ext?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/", linkHelper.Image(url: "/folder/", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/?a=1&b=2#fragment", linkHelper.Image(url: "/folder/?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/page", linkHelper.Image(url: "/folder/page", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/page?a=1&b=2#fragment", linkHelper.Image(url: "/folder/page?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/file.ext", linkHelper.Image(url: "/folder/file.ext", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/file.ext?a=1&b=2#fragment", linkHelper.Image(url: "/folder/file.ext?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/", linkHelper.Image(url: "/folder/subfolder/", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/?a=1&b=2#fragment", linkHelper.Image(url: "/folder/subfolder/?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page", linkHelper.Image(url: "/folder/subfolder/page", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?a=1&b=2#fragment", linkHelper.Image(url: "/folder/subfolder/page?a=1&b=2#fragment", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/file.ext", linkHelper.Image(url: "/folder/subfolder/file.ext", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/file.ext?a=1&b=2#fragment", linkHelper.Image(url: "/folder/subfolder/file.ext?a=1&b=2#fragment", part: "full"));
        }

        [TestMethod()]
        public void ImageUrlPathIsMissingTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=a&c=3#fragment", linkHelper.Image(url: "?c=3", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=c#fragment", linkHelper.Image(url: "?param=c", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=b&b=3&c=3#fragment", linkHelper.Image(url: "?param=b&b=3&c=3", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=a#fragmentB", linkHelper.Image(url: "?#fragmentB", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=c#fragmentB", linkHelper.Image(url: "?param=c#fragmentB", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/subfolder/page?param=a#fragmentC", linkHelper.Image(url: "#fragmentC", part: "full"));
        }

        [TestMethod()]
        public void ImageWithoutProtocolTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/test", linkHelper.Image(url: "//unknown.2sxc.org/test", part: "full"));
        }

        [TestMethod()]
        public void ImageUrlWithTildeTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown.2sxc.org/", linkHelper.Image(url: "~", part: "full"));
            AreEqual("https://unknown.2sxc.org/", linkHelper.Image(url: "~/", part: "full"));
            AreEqual("https://unknown.2sxc.org/page", linkHelper.Image(url: "~/page", part: "full"));
            AreEqual("https://unknown.2sxc.org/file.ext", linkHelper.Image(url: "~/file.ext", part: "full"));
            AreEqual("https://unknown.2sxc.org/folder/", linkHelper.Image(url: "~/folder/", part: "full"));
        }

        [TestMethod()]
        public void ImageWithAbsoluteUrlTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("https://unknown2.2sxc.org/", linkHelper.Image(url: "https://unknown2.2sxc.org/", part: "full"));
            AreEqual("https://unknown2.2sxc.org/page", linkHelper.Image(url: "https://unknown2.2sxc.org/page", part: "full"));
            AreEqual("https://unknown2.2sxc.org/file.ext", linkHelper.Image(url: "https://unknown2.2sxc.org/file.ext", part: "full"));
            AreEqual("https://unknown2.2sxc.org/folder/", linkHelper.Image(url: "https://unknown2.2sxc.org/folder/", part: "full"));
        }

        [TestMethod()]
        public void ImageWithInvalidUrlTest()
        {
            var linkHelper = LinkHelperUnknown();
            AreEqual("hello:there", linkHelper.Image(url: "hello:there", part: "full"));
            AreEqual("file:593902", linkHelper.Image(url: "file:593902", part: "full"));
            AreEqual("../file.ext", linkHelper.Image(url: "../file.ext", part: "full"));
            AreEqual("/sibling1/../sibling2/image.jpg", linkHelper.Image(url: "/sibling1/../sibling2/image.jpg", part: "full"));
        }

        /// <summary>
        /// test accessor
        /// </summary>
        /// <returns></returns>
        private static LinkHelperUnknown LinkHelperUnknown()
        {
            var conv = Resolve<LinkHelperUnknown>();
            return conv;
        }
    }
}