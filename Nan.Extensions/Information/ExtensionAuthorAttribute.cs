namespace Nan.Extensions.Information
{
    using System;

    /// <summary>
    /// 表示插件作者。此類別無法被繼承。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class ExtensionAuthorAttribute : Attribute
    {
        private readonly string author;

        /// <summary>
        /// 初始化 <see cref="ExtensionAuthorAttribute"/> 類別的新執行個體
        /// </summary>
        /// <param name="author">作者</param>
        public ExtensionAuthorAttribute(string author)
            => this.author = author ?? "<Unknown>";

        /// <summary>
        /// 插件作者
        /// </summary>
        public string Author => this.author;
    }

}
