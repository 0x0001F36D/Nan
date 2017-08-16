
namespace Nan.ConsoleDebugger
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Nan.Configuration;
    using System.Text.RegularExpressions;
    using System.Net.Http;
    using Nan.Management.Extensions;

    class Debugger
    {
        
        static void Main(string[] args)
        {
            var em = ExtensionManager
                .Instance
                .Setup((_, e) => Console.WriteLine(e.Message))
                .Initialize(e => Console.WriteLine(e))

                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我的IP位置")
                    
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置")
                .Invoke("告訴我我的IP位置");


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
