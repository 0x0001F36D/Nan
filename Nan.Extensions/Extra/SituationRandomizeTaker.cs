
namespace Nan.Extensions.Extra
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class SituationRandomizeTaker
    {
        protected readonly SituationManager manager;

        protected SituationRandomizeTaker(SituationManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 隨機取得一個情境內容
        /// </summary>
        /// <param name="situationName">情境名稱</param>
        /// <param name="args">參數</param>
        /// <returns></returns>
        public abstract string TakeOnce(string situationName, params object[] args);
        
    }
}
