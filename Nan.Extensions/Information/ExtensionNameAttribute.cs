namespace Nan.Extensions.Information
{
    using System;

    /// <summary>
    /// 表示插件名稱。此類別無法被繼承。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class ExtensionNameAttribute : Attribute
    {
        private readonly string name;

        /// <summary>
        /// 初始化 <see cref="ExtensionNameAttribute"/> 類別的新執行個體
        /// </summary>
        /// <param name="name">作者</param>
        public ExtensionNameAttribute(string name)
            => this.name = name ?? "<Unknown>";

        /// <summary>
        /// 插件名稱
        /// </summary>
        public string Name => this.name;
    }

}
