
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
            var test = new Test();

            //Console.WriteLine(test.Config.Create("field-1"));
            Console.WriteLine(test.Config.Update("field-1","hahaha"));
            //Console.WriteLine(test.Config.Retrieve("field-1", out var data));
            //Console.WriteLine(data);
            //Console.WriteLine(test.Config.Delete("field-1"));


            Console.ReadKey();
        }

        class Test : IConfig
        {
            public Test()
            {
                this.Config = Config.Load<Test>("test.Nan-Config");
            }
            public Config Config { get; private set; }
        }
    }
}
