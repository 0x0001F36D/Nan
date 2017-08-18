
namespace Ipify.org
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Nan.Extensions;
    using Nan.Extensions.Information;
    using Nan.Extensions.Extra;
    using System;

    [ExtensionAuthor("Viyrex(aka Yuyu)")]
    [ExtensionName("Ipify")]
    [ExtensionDescription(@"Get your public IP address")]

    public sealed class Ipify : IExtension
    {
        private readonly static string[] situations = { "Found", "NotFound" };
        private Ipify()
        {

            this.manager = SituationManager.Initialize<Ipify>();
            this.init();
            this.httpClient = new HttpClient();
            this.rule = new Dictionary<string, Delegate>
            {
                [@"(查詢|告訴我|找出|我想知道|查|搜尋)?.*我的?.*ip(address|地址|位置|位址)?"] = new Func<Task<string>>(async () => await httpClient.GetStringAsync("https://api.ipify.org"))
            };
        }

        private void init()
        {
            if (this.manager.CreateSituation(situations[0]) | this.manager.CreateSituation(situations[1]))
            {
                //Found
                this.manager.SubjoinResponse(situations[0], "找到您的IP了w，IP是 '{0}'");
                this.manager.SubjoinResponse(situations[0], "您的IP是 '{0}'");
                this.manager.SubjoinResponse(situations[0], "IP是 '{0}'");
                this.manager.SubjoinResponse(situations[0], "位置找到了，是 '{0}' 唷www");
                this.manager.SubjoinResponse(situations[0], "搜索到位址了!! 您的位址是 '{0}'");
                this.manager.SubjoinResponse(situations[0], "找到位址了!! 位址是 '{0}'");

                //Not Found
                this.manager.SubjoinResponse(situations[1], "找不到IP呢，因為發生錯誤QAQQQ");
                this.manager.SubjoinResponse(situations[1], "沒能搜索到呢OvQ，因為網站說發生了錯誤QuQ");
                this.manager.SubjoinResponse(situations[1], "我找不到QAQ，因為發生錯誤TAT");
                this.manager.SubjoinResponse(situations[1], "沒能找到IP呢...發生了些許錯誤QvQ");

                this.manager.Save();
            }
        }

        public bool Evaluate(string text)
            => this.isMatch(text, out var _);


        private readonly HttpClient httpClient;
        private IDictionary<string, Delegate> rule;
        private readonly SituationManager manager;

        private bool isMatch(string text, out Delegate @delegate)
        {
            var m = default(Match);
            @delegate = null;
            foreach (var r in this.rule)
            {
                m = Regex.Match(text, r.Key, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    @delegate = r.Value;
                    return true;
                }
            }
            return false;
        }

        public bool Invoke(string text)
        {
            if (this.isMatch(text, out var d))
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                try
                {
                    var ip = (d.DynamicInvoke() as Task<string>).Result;
                    var msg = taker.TakeOnce(situations[0], ip);
                    this.Response?.Invoke(this, new ResponseEventArgs(msg, Emotion.Joy));
                }
                catch (Exception)
                {
                    var msg = taker.TakeOnce(situations[1]);
                    this.Response?.Invoke(this, new ResponseEventArgs(msg, Emotion.Sadness));
                }
                return true;
            }

            return false;
        }

        public event ResponseEventHandler Response;
    }

}
