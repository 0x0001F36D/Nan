

namespace Nan.Extensions.Information
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// 取得插件的附加資訊
    /// </summary>
    internal static class ExtensionInfo
    {
        /// <summary>
        /// 取得插件作者
        /// </summary>
        /// <param name="extension">插件</param>
        /// <returns></returns>
        public static string GetExtensionAuthor(this IExtension extension)
            => getAttribute<ExtensionAuthorAttribute>(extension).Author;

        /// <summary>
        /// 取得插件名稱
        /// </summary>
        /// <param name="extension">插件</param>
        /// <returns></returns>
        public static string GetExtensionName(this IExtension extension)
            => getAttribute<ExtensionNameAttribute>(extension).Name;

        /// <summary>
        /// 取得插件描述
        /// </summary>
        /// <param name="extension">插件</param>
        /// <returns></returns>
        public static string GetExtensionDescription(this IExtension extension)
            => getAttribute<ExtensionDescriptionAttribute>(extension).Description;

        private static A getAttribute<A>(IExtension e) where A : Attribute
            => e.GetType().GetCustomAttributes<A>().SingleOrDefault();
        
    }


}