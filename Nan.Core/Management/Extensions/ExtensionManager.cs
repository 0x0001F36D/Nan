namespace Nan.Management.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Nan.Extensions;

    /// <summary>
    /// 插件管理器
    /// </summary>
    public class ExtensionManager
    {
        private const string extensions_path = @"/Extensions";
        private readonly static DirectoryInfo dir;
        private readonly static string dirPath;
        private static volatile ExtensionManager instance;
        private static object locker = new object();
        private readonly List<Assembly> assemblies;
        private readonly List<Lazy<IExtension>> extensions;
        private readonly string moduleExtName;

        static ExtensionManager()
        {
            dirPath = Environment.CurrentDirectory + extensions_path;
            dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
                dir.Create();
        }

        private ExtensionManager()
        {
            this.assemblies = new List<Assembly>();
            this.moduleExtName = typeof(IExtension).Name;

            this.extensions = new List<Lazy<IExtension>>();
        }
        

        /// <summary>
        /// 回應當下的使用者命令的訊息
        /// </summary>
        public event ResponseEventHandler Response;

        /// <summary>
        /// 取得執行個體
        /// </summary>
        public static ExtensionManager Instance
        {
            get
            {
                if (instance == null)
                    lock (locker)
                        if (instance == null)
                            instance = new ExtensionManager();
                return instance;
            }
        }

        /// <summary>
        /// 安裝回應委派
        /// </summary>
        /// <param name="handler">委派</param>
        /// <returns></returns>
        public ExtensionManager Setup(ResponseEventHandler handler)
        {
            if (this.Response is null && !(handler is null))
                this.Response += handler;
            return this;
        }

        /// <summary>
        /// 透過使用者的語音轉文字指令引動插件
        /// </summary>
        /// <param name="text">使用者的語音轉文字指令</param>
        /// <returns></returns>
        public bool Invoke(string text)
        {
            var flag = false;
            foreach (var mod in this.extensions.Where(x => x.Value.Evaluate(text)))
            {
                flag |= mod.Value.Invoke(text);
            }
            return flag;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="onError">處理例外的委派方法</param>
        /// <returns></returns>
        public ExtensionManager Initialize(Action<Exception> onError)
        {
            this.extensions.Clear();

            var types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes());

            this.extensions.AddRange(this.createLazyObjects(types));


            var dlls = dir.EnumerateFiles("*.dll");

            foreach (var dll in dlls)
            {
                try
                {
                    var asm = Assembly.LoadFile(dll.FullName);
                    if (!this.assemblies.Any(a => a.FullName.Equals(asm.FullName)))
                    {
                        this.assemblies.Add(asm);
                        this.extensions.AddRange(this.createLazyObjects(asm.GetTypes()));
                    }
                }
                catch(ReflectionTypeLoadException e)
                {
                    foreach (var item in e.LoaderExceptions)
                    {
                        onError?.Invoke(item);
                    }
                    
                }
                catch(Exception e)
                {
                    // Todo: 載入錯誤的處理方式
                    onError?.Invoke( e);
                }

            }

            return this;
                
        }

        private IEnumerable<Lazy<IExtension>> createLazyObjects(IEnumerable<Type> rawTypes)
        {
            var types = rawTypes.Where(type => type.GetInterface(this.moduleExtName) != null && !type.IsAbstract && !type.IsInterface);
            foreach (var t in types)
            {
                var ctors = t.GetConstructors((BindingFlags)548);
                var ctor = ctors.FirstOrDefault();
                if (ctor is null)
                {
                    continue;
                }
                else
                {
                    yield return new Lazy<IExtension>(() =>
                    {
                        var o = ctor.Invoke(null) as IExtension;
                        o.Response += this.Response;
                        return o;
                    });
                }
            }
            yield break;
        }
        
    }
}