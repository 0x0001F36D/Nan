namespace Nan.Configuration
{
    using Notification;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// 類別設定檔
    /// </summary>
    public sealed class Config : INotifyFieldOperated
    {
        private const string config_extension = ".Nan-Config";
        private const string config_path = @"/Config";
        private readonly static DirectoryInfo dir;
        private readonly static string dirPath;
        private readonly ConfigMetadata configData;
        private readonly FileInfo fileInfo;

        static Config()
        {
            dirPath = Environment.CurrentDirectory + config_path;
            dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
                dir.Create();
        }

        private Config(FileInfo info)
        {
            this.fileInfo = info;
            this.configData = this.initialize(info);
        }

        /// <summary>
        /// 取得、更新欄位資料
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns></returns>
        public string this[string fieldName]
        {
            get => this.configData[fieldName];
            set => this.configData[fieldName] = value;
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

        private static string getDirName<T>()
            => typeof(T).Name;

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

        private static DirectoryInfo getSubdirectory(string pdir)
        {
            var ds = dir.EnumerateDirectories();

            return ds.FirstOrDefault(d => d.Name.Equals(pdir)) is DirectoryInfo di
                ? di
                : dir.CreateSubdirectory(pdir);
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

        /// <summary>
        /// 對欄位存取時引發事件
        /// </summary>
        public event FieldOperatedEventHandler FieldOperated
        {
            add => this.configData.FieldOperated += value;
            remove => this.configData.FieldOperated -= value;
        }

        #region CURD
        
        /// <summary>
        /// 創建欄位
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns></returns>
        public bool Create(string fieldName)
            => this.configData.Create(fieldName);

        /// <summary>
        /// 刪除欄位
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <returns></returns>
        public bool Delete(string fieldName)
            => this.configData.Delete(fieldName);

        /// <summary>
        /// 取回欄位資料
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Retrieve(string fieldName, out string value)
            => this.configData.Retrieve(fieldName, out value);

        /// <summary>
        /// 更新欄位資料
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Update(string fieldName, string value)
            => this.configData.Update(fieldName, value);

        #endregion CURD
    }
}