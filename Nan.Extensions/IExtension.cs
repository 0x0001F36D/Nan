namespace Nan.Extensions
{
    /// <summary>
    /// 提供插件的基本功能實作介面
    /// </summary>
    public interface IExtension
    {
        /// <summary>
        /// 評估 <paramref name="text"/> 是否符合插件引動規則
        /// </summary>
        /// <param name="text">使用者的語音轉文字指令</param>
        bool Evaluate(string text);

        /// <summary>
        /// 透過使用者的語音轉文字指令引動插件
        /// </summary>
        /// <param name="text">使用者的語音轉文字指令</param>
        bool Invoke(string text);

        /// <summary>
        /// 回應當下的使用者命令
        /// </summary>
        event ResponseEventHandler Response;
    }

    /// <summary>
    /// 表示模組產生訊息回應時所引發的 <seealso cref="IExtension.Response"/> 事件的方法。
    /// </summary>
    /// <param name="sender">事件的來源。</param>
    /// <param name="e">包含事件資料的 <see cref="ResponseEventArgs"/> 。</param>
    public delegate void ResponseEventHandler(object sender, ResponseEventArgs e);

}
