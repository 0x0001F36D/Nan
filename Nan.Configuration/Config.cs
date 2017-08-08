namespace Nan.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    public sealed class Config : INotifyPropertyChanged
    {
        private const string config_path = @"/Config";
        private const string config_extension = ".Nan-Config";
        private readonly static string dirPath;
        private readonly static DirectoryInfo dir;
        private readonly ConfigMetadata configData;
        private readonly FileInfo fileInfo;

        static Config()
        {
            dirPath = Environment.CurrentDirectory + config_path;
            dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
                dir.Create();
        }

        /// <summary>
        /// 載入設定檔
        /// </summary>
        /// <typeparam name="T">包含設定檔的類別</typeparam>
        /// <param name="fileName">設定檔的檔名(不包含副檔名)</param>
        /// <returns></returns>
        public static Config Load<T>(string fileNameWithoutExtension) where T : IConfig
        {
            var path = Path.GetFileNameWithoutExtension(fileNameWithoutExtension) + config_extension;

            var dir = getDirName<T>();
            var subDir = getSubdirectory(dir);
            var file = getFileInfo(path, subDir);
            return new Config(file);
        }

        private Config(FileInfo info)
        {
            this.fileInfo = info;
            this.configData = this.initialize(info);
        }

        private ConfigMetadata initialize(FileInfo fi)
        {
            try
            {
                using (var fs = fi.OpenRead())
                {
                    var bf = new BinaryFormatter();
                    return (ConfigMetadata)bf.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                return new ConfigMetadata();
            }
        }

        private static string getDirName<T>()
            => typeof(T).Name;

        private static DirectoryInfo getSubdirectory(string pdir)
        {
            var ds = dir.EnumerateDirectories();

            return ds.FirstOrDefault(d => d.Name.Equals(pdir)) is DirectoryInfo di
                ? di
                : dir.CreateSubdirectory(pdir);
        }

        private static FileInfo getFileInfo(string filename, DirectoryInfo di)
        {
            var fs = di.EnumerateFiles();
            if (fs.FirstOrDefault(f => f.Name.Equals(filename)) is FileInfo fi)
                return fi;
            else
            {
                var f = new FileInfo(di.FullName + "//" + filename);
                f.Create();
                return f;
            }
        }        

        /// <summary>
        /// 儲存設定
        /// </summary>
        public void Save()
        {
            try
            {
                using (var fs = this.fileInfo.OpenWrite())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fs, this.configData);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 取得、更新欄位資料
        /// </summary>
        /// <param name="itemName">欄位名稱</param>
        /// <returns></returns>
        public string this[string itemName]
        {
            get => this.configData[itemName];
            set => this.configData[itemName] = value;
        }

        #region CURD

        /// <summary>
        /// 創建欄位資料
        /// </summary>
        /// <param name="itemName">欄位名稱</param>
        /// <returns></returns>
        public bool Create(string itemName)
            => this.configData.Create(itemName);
            
        /// <summary>
        /// 更新欄位資料
        /// </summary>
        /// <param name="itemName">欄位名稱</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Update(string itemName, string value)
            => this.configData.Update(itemName, value);
            
        /// <summary>
        /// 取回欄位資料
        /// </summary>
        /// <param name="itemName">欄位名稱</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Retrieve(string itemName, out string value)
            => this.configData.Retrieve(itemName, out value);
        
        /// <summary>
        /// 刪除欄位資料
        /// </summary>
        /// <param name="itemName">欄位名稱</param>
        /// <returns></returns>
        public bool Delete(string itemName)
            => this.configData.Delete(itemName);

        /// <summary>
        /// 當欄位的值變更時引發
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => this.configData.PropertyChanged += value;
            remove => this.configData.PropertyChanged -= value;
        }



        #endregion

    }
}
