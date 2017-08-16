
namespace Nan.Extensions.Extra
{
    using System;
    public class DefaultRandomizeTaker : SituationRandomizeTaker
    {

        public  DefaultRandomizeTaker (SituationManager manager):base(manager)
        {
            this.lastTimeAccess = default(int);
        }

        private int lastTimeAccess;

        /// <summary>
        /// 隨機取得一個情境內容的回應
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <param name="args">參數</param>
        /// <returns></returns>
        public override string TakeOnce(string situationName, params object[] args)
        {
            if (this.manager.RetrieveSituation(situationName, out var list))
            {
                Again:
                var rand = new Random(Guid.NewGuid().GetHashCode()).Next(0, list.Count);
                if (lastTimeAccess == rand)
                    goto Again;
                return string.Format(list[this.lastTimeAccess = rand], args);
            }
            return string.Empty;
        }


    }
}
