
namespace Nan.Extensions.Extra.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections;
    
    [JsonObject]
    internal sealed class Situation : IEnumerable<string>, IGrouping<string, string>
    {
        [JsonConstructor]
        internal Situation(string situation)
        {
            this.Responses = new HashSet<string>();
            this.Rename(situation);
        }

        [JsonProperty("#Situation")]
        internal string Name { get; private set; }

        [JsonProperty("#Responses")]
        internal ISet<string> Responses { get; set; }

        internal bool Rename(string situation)
        {
            if (situation is null)
                return false;

            situation = situation.ToLower();
            if (this.Name != situation)
            {
                this.Name = situation;
                return true;
            }
            return false;
        }

        internal bool Add(string response)
            => this.Responses.Add(response);

        internal bool Delete(string response)
            => this.Responses.Remove(response);

        internal bool Update(string oldResponse, string newResponse)
        {
            if (this.Responses.Contains(oldResponse) && !this.Responses.Contains(newResponse))
            {
                this.Add(newResponse);
                this.Delete(oldResponse);
                return true;
            }
            return false;
        }

        public IEnumerator<string> GetEnumerator() => this.Responses.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Responses.GetEnumerator();

        [JsonIgnore]
        public string Key => this.Name;
        
    }
}

