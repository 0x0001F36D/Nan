

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

        /*
        
        public GoogleSpeechToText(string clientSecretsPath = "client_secrets.json") : this(Thread.CurrentThread.CurrentCulture, clientSecretsPath)
        {

        }

        public GoogleSpeechToText(CultureInfo info, string clientSecretsPath = "client_secrets.json")
        {
            var path = new FileInfo(clientSecretsPath).FullName;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path, EnvironmentVariableTarget.User);

            this.speech = SpeechClient.Create();
            this.config = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRate = 16000,
                LanguageCode = info.Name,
            };
        }
        */
        public GoogleSpeechToText()
        {
            var path = new FileInfo(@"C:\Users\USER\Documents\visual studio 2017\Projects\Nan\Nan.ConsoleDebugger\bin\Debug\client_secrets.json").FullName;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path, EnvironmentVariableTarget.User);

            this.speech = SpeechClient.Create();
            this.config = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRate = 16000,
                LanguageCode = Thread.CurrentThread.CurrentCulture.Name,
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
