namespace Nan.Extensions
{
    using System;
    /// <summary>
    /// 提供 <seealso cref="IExtension.Response"/> 事件的資料
    /// </summary>
    public class ResponseEventArgs : EventArgs
    {
        private readonly Emotion _emotion;
        private readonly string _message;

        /// <summary>
        /// 初始化 <see cref="ResponseEventArgs"/> 類別的新執行個體
        /// </summary>
        /// <param name="message">回應的訊息</param>
        /// <param name="emotion">回應的情緒</param>
        public ResponseEventArgs(string message, Emotion emotion)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _emotion = emotion;
        }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message => this._message;

        /// <summary>
        /// 情緒
        /// </summary>
        public Emotion Emotion => this._emotion;
    }
}
