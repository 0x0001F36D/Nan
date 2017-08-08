namespace Nan.Vox.Internal
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using NAudio.Wave;
    internal abstract class SoundRecorder
    {
        public sealed class StoppedEventArgs : EventArgs
        {
            internal StoppedEventArgs(WaveStream stream)
                => this.WaveStream = stream;

            public WaveStream WaveStream { get; private set; }
        }

        private readonly List<byte> raw;
        private readonly WaveInEvent wave;

        protected SoundRecorder()
        {
            this.wave = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };
            this.raw = new List<byte>();

            this.wave.DataAvailable += (_, e) => this.raw.AddRange(e.Buffer);
            this.RecordingStarted += OnStart;
            this.RecordingStopped += OnStop;
        }

        protected event EventHandler RecordingStarted;

        protected event EventHandler<StoppedEventArgs> RecordingStopped;

        public WaveInCapabilities InputDevice { get; set; }

        public void Start()
        {
            this.raw.Clear();
            this.wave.StartRecording();
            this.RecordingStarted?.Invoke(this, new EventArgs());
        }

        public void Stop()
        {
            this.wave.StopRecording();
            this.RecordingStopped?.Invoke(this, new StoppedEventArgs(this.getWaveStream()));
        }

        protected virtual void OnStart(object sender, EventArgs e)
        {
        }

        protected virtual void OnStop(object sender, StoppedEventArgs e)
        {
        }

        private WaveStream getWaveStream()
            => new RawSourceWaveStream(new MemoryStream(this.raw.ToArray()), this.wave.WaveFormat);
    }
}
