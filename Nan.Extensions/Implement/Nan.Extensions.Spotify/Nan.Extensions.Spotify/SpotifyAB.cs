
namespace Nan.Extensions.Spotify
{
    using SpotifyAPI.Web.Auth;
    using SpotifyAPI.Web.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpotifyAPI.Web;
    using System.Collections;
    using System.Text.RegularExpressions;
    using Nan.Extensions.Extra;
    using SpotifyAPI.Local;
    using SpotifyAPI.Web.Models;

    public class SpotifyAB : IExtension
    {
        private static SpotifyWebAPI _spotify { get; }
        static SpotifyAB()
        {
            var webApiFactory = new WebAPIFactory(
                "http://localhost",
                48000,
                "334f6e3b588c4daf85008cd30116e736",
                Scope.UserTopRead |
                Scope.UserReadRecentlyPlayed |
                Scope.UserReadPrivate |
                Scope.UserReadPlaybackState |
                Scope.UserReadEmail |
                Scope.UserReadBirthdate |
                Scope.UserModifyPlaybackState |
                Scope.UserLibraryRead |
                Scope.UserLibraryModify |
                Scope.UserFollowRead |
                Scope.UserFollowModify |
                Scope.PlaylistReadPrivate |
                Scope.PlaylistReadCollaborative |
                Scope.PlaylistModifyPublic |
                Scope.PlaylistModifyPrivate |
                Scope.Streaming,
                TimeSpan.FromSeconds(20)
                );

            try
            {
                _spotify = webApiFactory.GetWebApi().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private readonly SituationManager manager;
        private Dictionary<string, Delegate> table;
        private SpotifyAB()
        {
            this.manager = SituationManager.Initialize<SpotifyAB>();
            this.table = new Dictionary<string, Delegate>
            {
                [@"換?(播放|放)?下一首(歌曲|歌|曲目|曲子|音樂)?"] = new Action(next),
                [@"換?(播放|放)?上一首(歌曲|歌|曲目|曲子|音樂)?"] = new Action(previous),
                [@"(停止|暫停|中止)(歌曲|音樂)?"] = new Action(pause),
                [@"把(歌曲|歌|音樂)(中止|暫停|停止)"] = new Action(pause),
                [@"放(歌曲|歌|音樂)?"] = new Action(play),
                [@"播放(歌曲|音樂)?"] = new Action(play),
                [@"((把|讓)?(音樂播放器|播放器|spotify))?(開啟靜音|靜音|消音)"] = new Action(mute),
                [@"((把|讓)?(音樂播放器|播放器|spotify))?(開啟聲音|關閉消音|關閉靜音)"] = new Action(unmute),
            };
            this.init();
        }

        private readonly string[] list = new[] { "Default" };
        private int volumeCache;

        private void init()
        {
            if (this.manager.CreateSituation(list[0]))
            {
                this.manager.SubjoinResponse(list[0], "事情已經照您所吩咐完成了!");
                this.manager.SubjoinResponse(list[0], "好的!已經照您所吩咐的完成了");
                this.manager.SubjoinResponse(list[0], "好的!已經處理好了了");


                this.manager.Save();
            }
        }
        private Device getDevice()
        {
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                SpotifyLocalAPI.RunSpotifyWebHelper();
                Task.Run(() =>
                {
                    while (!SpotifyLocalAPI.IsSpotifyWebHelperRunning()) ;
                }).Wait();
            }
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                SpotifyLocalAPI.RunSpotify();
                Task.Run(() =>
                {
                    while (!SpotifyLocalAPI.IsSpotifyRunning()) ;
                }).Wait();
            }
            var result = default(Device);
            while ((result = _spotify
                .GetDevices()
                .Devices?
                .FirstOrDefault(x => x.IsActive)) is null)
                Task.Delay(100).Wait();
            return result;
        }

        private void mute()
        {
            var vol = this.getDevice().VolumePercent;
            if (!vol.Equals(0) && !_spotify.SetVolume(0).HasError())
            {
                this.volumeCache = vol;
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }
        private void unmute()
        {
            var vol = this.getDevice().VolumePercent;
            if (vol.Equals(0) && !_spotify.SetVolume(this.volumeCache).HasError())
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }
        private void play()
        {
            if (!_spotify.ResumePlayback().HasError())
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }
        private void pause()
        {
            if (!_spotify.PausePlayback().HasError())
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }

        private void previous()
        {
            if (!_spotify.SkipPlaybackToPrevious().HasError())
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }

        private void next()
        {
            if (!_spotify.SkipPlaybackToNext().HasError())
            {
                var taker = new DefaultRandomizeTaker(this.manager);
                this.Response?.Invoke(this, new ResponseEventArgs(taker.TakeOnce(list[0]), Emotion.Joy));
            }
        }


        private bool isMatch(string text, out Delegate @delegate)
        {
            var m = default(Match);
            @delegate = null;
            foreach (var r in this.table)
            {
                m = Regex.Match(text, r.Key, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    @delegate = r.Value;
                    return true;
                }
            }
            return false;
        }

        public bool Evaluate(string text)
            => this.isMatch(text, out var _);

        public bool Invoke(string text)
        {
            if(this.isMatch(text,out var @delegate))
            {
                @delegate.DynamicInvoke();
                return true;
            }
            return false;

        }

        public event ResponseEventHandler Response;

        public bool ContainsKey(string key) => ((IReadOnlyDictionary<string, Delegate>)this.table).ContainsKey(key);
        public bool TryGetValue(string key, out Delegate value) => ((IReadOnlyDictionary<string, Delegate>)this.table).TryGetValue(key, out value);
        
    }
}
