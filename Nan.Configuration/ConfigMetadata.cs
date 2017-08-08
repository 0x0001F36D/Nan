
namespace Nan.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    internal sealed class ConfigMetadata : ISerializable,INotifyPropertyChanged
    {
        private IDictionary<string, string> data;

        internal ConfigMetadata()
            => this.data = new Dictionary<string, string>();

        internal string this[string name]
        {
            get => this.Retrieve(name, out var o) ? o : string.Empty;
            set => this.Update(name, value);
        }

        private bool checkName(string name)
            => string.IsNullOrWhiteSpace(name);

        internal bool Create(string name)
        {

            var n = name.ToLower();
            if (!this.data.ContainsKey(n))
            {
                this.data.Add(n, string.Empty);
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                return true;
            }
            return false;
        }

        internal bool Update(string name, string value)
        {
            var n = name.ToLower();
            if (this.data.ContainsKey(n) && !this.data[n].Equals(value))
            {                
                this.data[n] = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                return true;
            }
            return false;
        }

        internal bool Retrieve(string name, out string value)
        {
            var n = name.ToLower();
            return this.data.TryGetValue(n, out value);
        }

        internal bool Delete(string name)
        {
            var n = name.ToLower();
            if (this.data.ContainsKey(n))
            {
                this.data.Remove(n);
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                return true;
            }
            return false;
        }

        private ConfigMetadata(SerializationInfo info, StreamingContext context)
            => this.data = info.GetValue("config", typeof(Dictionary<string, string>)) as IDictionary<string, string>;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
            => info.AddValue("config", this.data);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
