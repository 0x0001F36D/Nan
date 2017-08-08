

namespace Nan.Vox.Internal
{
    using Google.Cloud.Speech.V1Beta1;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using NAudio.Wave;
    using System.Globalization;
    using System.Threading;

    internal sealed class GoogleSpeechToText : SoundRecorder
    {
        private readonly RecognitionConfig config;

        private readonly SpeechClient speech;

        public GoogleSpeechToText(CultureInfo info, string clientSecretsPath = "client_secrets.json") : this(info.Name, clientSecretsPath)
        {

        }
        public GoogleSpeechToText(string clientSecretsPath = "client_secrets.json") : this(Thread.CurrentThread.CurrentCulture, clientSecretsPath)
        {

        }

        public GoogleSpeechToText(string bcp47 = "zh-TW",string clientSecretsPath = "client_secrets.json")
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", new FileInfo(clientSecretsPath).FullName, EnvironmentVariableTarget.User);
            this.speech = SpeechClient.Create();
            this.config = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRate = 16000,
                LanguageCode = bcp47,
            };
        }


        internal event AlternativesProcessor ProcessAlternatives;

        protected override void OnStart(object sender, EventArgs e)
                            => Debug.WriteLine("Recording started");

        protected override void OnStop(object sender, StoppedEventArgs e)
        {
            var alternatives = this.speech
                .SyncRecognize(this.config, RecognitionAudio.FromStream(e.WaveStream))
                .Results
                .SelectMany(r => r.Alternatives);

            Debug.WriteLine("\n========== Results ==========");

            foreach (var alternative in alternatives)
            {
                Debug.WriteLine(alternative.Transcript + $"[{alternative.Confidence}]");
            }

            this.ProcessAlternatives?.Invoke(alternatives.ToList());

            Debug.WriteLine("=============================\n");
            Debug.WriteLine("Done!");
        }
    }
}
