namespace Nan.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 提供擴充方法的模組接口
    /// </summary>
    public interface IModuleExtension
    {
        /// <summary>
        /// 模組名稱
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 引動
        /// </summary>
        /// <param name="text">使用者的語音轉文字指令</param>
        void Invoke(string text);

        /// <summary>
        /// 回應當下的使用者命令的情緒
        /// </summary>
        event EmotionResponseEventHandler EmotionResponse;

        /// <summary>
        /// 回應當下的使用者命令的訊息
        /// </summary>
        event MessageResponseEventHandler MessageResponse;
    }

    /// <summary>
    /// 表示模組產生訊息回應時所引發的 <seealso cref="IModuleExtension.MessageResponse"/> 事件的方法。
    /// </summary>
    /// <param name="message">從模組內回應的訊息</param>
    public delegate void MessageResponseEventHandler(string message);

    /// <summary>
    /// 表示模組產生情緒回應時所引發的 <seealso cref="IModuleExtension.EmotionResponse"/> 事件的方法。
    /// </summary>
    /// <param name="emotion">從模組內回應的情緒</param>
    public delegate void EmotionResponseEventHandler(Emotion emotion);

    /// <summary>
    /// 情緒
    /// </summary>
    public enum Emotion
    {
        /// <summary>
        /// 正常狀態
        /// </summary>
        Normal = 0,


        /// <summary>
        /// 寧靜祥和
        /// </summary>
        Serenity = 125,
        /// <summary>
        /// 愉悅
        /// </summary>
        Joy = 150,
        /// <summary>
        /// 狂喜
        /// </summary>
        Ecstasy = 175,


        /// <summary>
        /// 極度厭惡
        /// </summary>
        Loathing = 275,
        /// <summary>
        /// 厭惡
        /// </summary>
        Disgust = 250,
        /// <summary>
        /// 無聊
        /// </summary>
        Boredom = 225,


        /// <summary>
        /// 極度哀傷
        /// </summary>
        Grief = 375,
        /// <summary>
        /// 憂傷
        /// </summary>
        Sadness = 350,
        /// <summary>
        /// 憂愁
        /// </summary>
        Pensiveness = 325,


        /// <summary>
        /// 驚訝
        /// </summary>
        Amazement = 475,
        /// <summary>
        /// 驚喜
        /// </summary>
        Surprise = 450,
        /// <summary>
        /// 分心狀態
        /// </summary>
        Distraction = 425,


        /// <summary>
        /// 極度恐慌
        /// </summary>
        Terror = 575,
        /// <summary>
        /// 懼怕
        /// </summary>
        Fear = 550,
        /// <summary>
        /// 憂懼
        /// </summary>
        Apprehension = 525,


        /// <summary>
        /// 尊敬
        /// </summary>
        Admiration = 675,
        /// <summary>
        /// 信任
        /// </summary>
        Trust = 650,
        /// <summary>
        /// 認同
        /// </summary>
        Acceptance = 625,


        /// <summary>
        /// 警覺
        /// </summary>
        Vigilance = 775,
        /// <summary>
        /// 期待
        /// </summary>
        Anticipation = 750,
        /// <summary>
        /// 趣味
        /// </summary>
        Interest = 725,


        /// <summary>
        /// 爆怒
        /// </summary>
        Rage = 875,
        /// <summary>
        /// 憤怒
        /// </summary>
        Angry = 850,
        /// <summary>
        /// 惱怒
        /// </summary>
        Annoyance = 825,
    }
}
