
namespace Nan.ConsoleDebugger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nan.Configuration;

    class Debugger
    {
        static void Main(string[] args)
        {
            var t = new Test();

            t.Config.Create("123");
            t.Config.Retrieve("123", out var v);
            Console.WriteLine(v);

            Console.ReadKey();
        }

        class Test : IConfig
        {
            public Test()
            {
                this.Config = Config.Load<Test>("test.Nan-Config");
                this.Config.FieldOperated += (_, e) => Console.WriteLine("pc:"+e.Operation);
            }
            public Config Config { get; private set; }
        }
    }
}
