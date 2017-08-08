
namespace Nan.Error
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    
    public sealed class NanException<T> : Exception
    {
        public NanException(T target, string message) : base(message)
        {
            this.Target = target;
        }
        public NanException(T target, string message, Exception inner) : base(message,inner)
        {
            this.Target = target;
        }

        public T Target { get; private set; }

    }
}
