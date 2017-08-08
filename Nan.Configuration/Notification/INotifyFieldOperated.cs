namespace Nan.Configuration.Notification
{
    /// <summary>
    /// 表示處理設定檔上欄位變更時所引發的 <seealso cref="INotifyFieldOperated.FieldOperated"/> 是件的方法。
    /// </summary>
    /// <param name="sender">事件的來源。</param>
    /// <param name="e">包含事件資料的 Nan.Config.FieldOperatedEventArgs。</param>
    public delegate void FieldOperatedEventHandler(object sender, FieldOperatedEventArgs e);

    /// <summary>
    /// 告知用戶端欄位已進行操作。
    /// </summary>
    public interface INotifyFieldOperated
    {
        /// <summary>
        /// 當欄位已進行操作
        /// </summary>
        event FieldOperatedEventHandler FieldOperated;
    }
}