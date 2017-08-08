namespace Nan.Configuration.Notification
{
    using System;

    /// <summary>
    /// 提供 <seealso cref="INotifyFieldOperated.FieldOperated"/> 事件的資料
    /// </summary>
    public class FieldOperatedEventArgs : EventArgs
    {
        /// <summary>
        /// 初始化 <see cref="FieldOperatedEventArgs"/> 類別的新執行個體
        /// </summary>
        /// <param name="operation">操作方式</param>
        /// <param name="fieldName">欄位名稱</param>
        public FieldOperatedEventArgs(Operation operation, string fieldName)
        {
            this.FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Operation = operation;
        }

        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// 操作方式
        /// </summary>
        public Operation Operation { get; private set; }
    }
}