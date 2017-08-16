namespace Nan.Extensions.Information
{
    using System;

    /// <summary>
    /// 表示插件描述。此類別無法被繼承。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class ExtensionDescriptionAttribute : Attribute
    {
        private readonly string description;

        /// <summary>
        /// 初始化 <see cref="ExtensionDescriptionAttribute"/> 類別的新執行個體
        /// </summary>
        /// <param name="description">作者</param>
        public ExtensionDescriptionAttribute(string description)
            => this.description = description ?? "<Unknown>";

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description => this.description;
    }

}
