
namespace Nan.Vox
{
    using Nan.Vox.Internal;
    using Google.Cloud.Speech.V1Beta1;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public delegate void AlternativesProcessor(IList<SpeechRecognitionAlternative> alternatives);
    public class VoiceRecognize
    {
        private readonly GoogleSpeechToText stt;
        private VoiceRecognize()
        {
            this.stt = new GoogleSpeechToText();
            this.flag = false;
        }
        public static VoiceRecognize Instance
        {
            get
            {
                if (instance == null)
                    lock (locker)
                        if (instance == null)
                            instance = new VoiceRecognize();
                return instance;
            }
        }
        private static volatile VoiceRecognize instance;
        private static object locker = new object();


        private bool flag;
        public VoiceRecognize Setup(AlternativesProcessor processor)
        {
            if (processor != null && !flag)
            {
                this.stt.ProcessAlternatives += processor;
                this.flag = true;
            }
            return this;
        }

        public void Start()
            => this.stt.Start();

        public void Stop()
            => this.stt.Stop();
    }
}
