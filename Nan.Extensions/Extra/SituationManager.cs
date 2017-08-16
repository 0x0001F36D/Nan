
namespace Nan.Extensions.Extra
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using Internal;

    /// <summary>
    /// 情境資料管理器
    /// </summary>
    public sealed class SituationManager
    {

        private SituationManager(string filename) : this(filename, null)
        {
        }
        
        private SituationManager(string filename, string json)
        {
            this.filename = filename?? throw new ArgumentNullException(nameof(filename));
            if (string.IsNullOrWhiteSpace(json))
                this.situations = new List<Situation>();
            else
                try
                {
                    this.situations = JsonConvert
                        .DeserializeAnonymousType(json, new { Situations = default(IList<Situation>) }).Situations.ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
        }

        /// <summary>
        /// 初始化特定類別的情境資料(若類別的情境資料不存在，則自動建立情境資料檔)
        /// </summary>
        /// <param name="type">包含情境資料的類別型態宣告</param>
        /// <returns></returns>
        internal static SituationManager Initialize(Type type)
        {
            var n = type.Name;
            using (var r = new StreamReader(File.Open(n, FileMode.OpenOrCreate)))
            {
                var json = r.ReadToEnd();
                return string.IsNullOrWhiteSpace(json) ? new SituationManager(n) : new SituationManager(n, json);
            }
        }

        /// <summary>
        /// 初始化特定類別的情境資料(若類別的情境資料不存在，則自動建立情境資料檔)
        /// </summary>
        /// <typeparam name="T">包含情境資料的類別 (具有 <see cref="IExtension"/> 介面) </typeparam>
        /// <returns></returns>
        public static SituationManager Initialize<T>() where T: IExtension
            => Initialize(typeof(T));

        /// <summary>
        /// 將情境資料儲存到檔案中
        /// </summary>
        public void Save()
        {
            using (var file = new StreamWriter(File.OpenWrite(filename)))
                file.Write(this.AsJson());
        }

        /// <summary>
        /// 將情境集合轉成 Json 格式
        /// </summary>
        /// <returns></returns>
        public string AsJson()
            => JsonConvert.SerializeObject(new {  Situations = this.situations }, Formatting.Indented);
        
        private readonly IList<Situation> situations;
        private readonly string filename;

        /// <summary>
        /// 建立情境
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <returns></returns>
        public bool CreateSituation(string situationName)
        {
            if (!this.isContain(situationName, out var _))
            {
                this.situations.Add(new Situation(situationName));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重新命名情境
        /// </summary>
        /// <param name="oldSituationName">舊情境名稱</param>
        /// <param name="newSituationName">新情境名稱</param>
        /// <returns></returns>
        public bool RenameSituation(string oldSituationName, string newSituationName)
        {
            if (this.isContain(oldSituationName, out var index))
            {
                this.situations[index].Rename(newSituationName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加入在情境內的回應
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <param name="response">回應</param>
        /// <returns></returns>
        public bool SubjoinResponse(string situationName, string response)
        {
            if (this.isContain(situationName, out var index))
            {
                this.situations[index].Add(response);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 刪除情境內的回應
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <param name="response">回應</param>
        /// <returns></returns>
        public bool DeleteResponse(string situationName, string response)
        {
            if (this.isContain(situationName, out var index))
            {
                this.situations[index].Delete(response);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新情境內的回應
        /// </summary>
        /// <param name="situation">情境名稱</param>
        /// <param name="oldResponseName">舊情境名稱</param>
        /// <param name="newResponseName">新情境名稱</param>
        /// <returns></returns>
        public bool UpdateResponse(string situation, string oldResponseName, string newResponseName)
        {
            if (this.isContain(situation, out var index))
            {
                this.situations[index].Update(oldResponseName, newResponseName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得情境內的回應
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <param name="responses">回應</param>
        /// <returns></returns>
        public bool RetrieveSituation(string situationName, out IList<string> responses)
        {
            responses = default(IList<string>);
            if (this.isContain(situationName, out var index))
            {
                responses = this.situations[index].ToList();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 刪除情境
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <returns></returns>
        public bool DeleteSituation(string situationName)
        {
            if (this.isContain(situationName, out var index))
            {
                this.situations.RemoveAt(index);
                return true;
            }
            return false;
        }

        private bool isContain(string situation, out int index)
        {
            index = -1;
            var obj = this.situations.FirstOrDefault(x => x.Key.Equals(situation.ToLower()));
            if (obj is null)
            {
                return false;
            }
            else
            {
                index = this.situations.IndexOf(obj);
                return true;
            }

        }
    }
}

