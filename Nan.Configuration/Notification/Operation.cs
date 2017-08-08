namespace Nan.Configuration.Notification
{
    /// <summary>
    /// 操作方式
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// 創建欄位
        /// </summary>
        Create = 1,

        /// <summary>
        /// 更新欄位資料
        /// </summary>
        Update,

        /// <summary>
        /// 取回欄位資料
        /// </summary>
        Retrieve,

        /// <summary>
        /// 刪除欄位
        /// </summary>
        Delete
    }
}