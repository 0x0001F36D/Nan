namespace Nan.Configuration
{
    using Notification;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 設定檔的中繼資料
    /// </summary>
    [Serializable]
    internal sealed class ConfigMetadata : ISerializable, INotifyFieldOperated
    {
        private IDictionary<string, string> data;

        internal ConfigMetadata()
            => this.data = new Dictionary<string, string>();

        private ConfigMetadata(SerializationInfo info, StreamingContext context)
            => this.data = info.GetValue("config", typeof(Dictionary<string, string>)) as IDictionary<string, string>;

        public event FieldOperatedEventHandler FieldOperated;

        internal string this[string name]
        {
            get => this.Retrieve(name, out var o) ? o : string.Empty;
            set => this.Update(name, value);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
            => info.AddValue("config", this.data);

        internal bool Create(string name)
        {
            var n = name.ToLower();
            if (!this.data.ContainsKey(n))
            {
                this.data.Add(n, string.Empty);
                this.FieldOperated?.Invoke(this, new FieldOperatedEventArgs(Operation.Create, name));
                return true;
            }
            return false;
        }

        internal bool Delete(string name)
        {
            var n = name.ToLower();
            if (this.data.ContainsKey(n))
            {
                this.data.Remove(n);
                this.FieldOperated?.Invoke(this, new FieldOperatedEventArgs(Operation.Delete, name));
                return true;
            }
            return false;
        }

        internal bool Retrieve(string name, out string value)
        {
            var n = name.ToLower();
            this.FieldOperated?.Invoke(this, new FieldOperatedEventArgs(Operation.Retrieve, name));
            return this.data.TryGetValue(n, out value);
        }

        internal bool Update(string name, string value)
        {
            var n = name.ToLower();
            if (this.data.ContainsKey(n) && !this.data[n].Equals(value))
            {
                this.data[n] = value;
                this.FieldOperated?.Invoke(this, new FieldOperatedEventArgs(Operation.Update, name));
                return true;
            }
            return false;
        }

        private bool checkName(string name)
                                                    => string.IsNullOrWhiteSpace(name);
    }
}