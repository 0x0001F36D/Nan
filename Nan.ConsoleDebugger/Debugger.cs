
namespace Nan.ConsoleDebugger
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using Nan.Configuration;
    using System.Text.RegularExpressions;
    using System.Net.Http;
    using Nan.Management.Extensions;
    using Nan.Vox;
    using Google.Cloud.Speech.V1Beta1;

    class Debugger
    {
        [STAThread]   
        static void Main(string[] args)
        {
            //告訴我我的IP位置

            var em = ExtensionManager
                .Instance
                .Setup((_, e) => Console.WriteLine(e.Message))
                .Initialize(e => Console.WriteLine("Error: " + e));

                
            var voxr = VoiceRecognize.Instance.Setup((alternatives)=> 
            {
                var alt = alternatives.FirstOrDefault();
                if (alt is SpeechRecognitionAlternative alternative)
                {
                    Console.WriteLine("Voice input: "+alternative.Transcript);
                    Console.WriteLine("Invoke: ",   em.Invoke(alternative.Transcript));
                }
            });
            while (true)
            {
                Console.WriteLine("- start Recording -");
                voxr.Start();
                Console.WriteLine(" Press any to continue ...");
                Console.ReadKey();
                Console.WriteLine("- stop  Recording -");
                voxr.Stop();

                Console.WriteLine("press 'x' to exit");
                if (Console.ReadKey().Key == ConsoleKey.X)
                    break;
            }
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
