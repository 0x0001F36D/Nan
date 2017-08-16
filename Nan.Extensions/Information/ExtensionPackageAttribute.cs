namespace Nan.Extensions.Information
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class ExtensionPackageMessageAttribute : Attribute
    {
        private readonly string message;

        public ExtensionPackageMessageAttribute(string message)
        {
            this.message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message => this.message;
    }
}
