using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.Util;
using Drapp.Android.Sound;
using Drapp.Metronome.Sound;
using DrappNative;
using Xamarin.Essentials;
using Xamarin.Forms;
using Stream = System.IO.Stream;

[assembly: Dependency(typeof(TestMixer))]
namespace Drapp.Android.Sound
{
    public class TestMixer : ITestMixer
    {
        private Dictionary<Note, double> notesFreqs = new Dictionary<Note, double>()
        {
            {Note.C5, 523.25},
            {Note.D5, 587.33},
            {Note.E5, 659.25},
            {Note.F5, 698.46},
            {Note.G5, 783.99},
            {Note.A5, 880.00},
            {Note.B5, 987.77},
            {Note.C6, 1046.50},
            {Note.D6, 1174.66},
            {Note.E6, 1318.51},
            {Note.F6, 1396.91},
            {Note.G6, 1567.98},
            {Note.A6, 1760.00},
            {Note.B6, 1975.53},
        };

        private float _gain1;
        private float _gain2;

        private Dictionary<Drum, int> drumIds = new Dictionary<Drum, int>();
        private Dictionary<Drum, byte[]> drumsBytes = new Dictionary<Drum, byte[]>();
        
        private SoundProvider _provider;
        private SoundMixer _mixer;

        private AudioTrack _audioTrack;
        
        public void Init()
        {
            int sampleRate = 48000;
            
            _provider = new SoundProvider();

            drumIds.Add(Drum.Kick, PutSoundToProvider("sounds/drums/kick_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Snare, PutSoundToProvider("sounds/drums/snare_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Tom1, PutSoundToProvider("sounds/drums/tom1_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Tom2, PutSoundToProvider("sounds/drums/tom2_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.FloorTom, PutSoundToProvider("sounds/drums/floor_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.HiHat, PutSoundToProvider("sounds/drums/hh_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Ride, PutSoundToProvider("sounds/drums/ride_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Crash1, PutSoundToProvider("sounds/drums/crash1_ez_test_48k_kHz.wav"));
            drumIds.Add(Drum.Crash2, PutSoundToProvider("sounds/drums/crash2_ez_test_48k_kHz.wav"));

            _audioTrack = new AudioTrack.Builder()
                .SetAudioFormat(new AudioFormat.Builder()
                    .SetChannelMask(ChannelOut.Mono)
                    ?.SetEncoding(Encoding.Pcm16bit)
                    ?.SetSampleRate(sampleRate)
                    ?.Build())
                ?.SetTransferMode(AudioTrackMode.Stream)
                ?.Build();

            _gain1 = 0.5f;
            _gain2 = 0.5f;

            _mixer = new SoundMixer();

            _audioTrack.Play();
        }

        public void SetGain1(float gain1)
        {
            _gain1 = gain1;
        }

        public void SetGain2(float gain2)
        {
            _gain2 = gain2;
        }

        private int PutSoundToProvider(string path)
        {
            byte[] bytesToStore = OpenSoundAsset(path);
            return _provider.PutSound(bytesToStore, bytesToStore.Length);
        }

        private byte[] OpenSoundAsset(string path)
        {
            Context context = Platform.AppContext;
            
            if (context?.Assets == null)
            {
                throw new AndroidException();
            }

            AssetFileDescriptor afd = context.Assets.OpenFd(path);

            if (afd == null)
            {
                throw new AndroidException();
            }

            Stream inputStream = afd.CreateInputStream();

            if (inputStream == null)
            {
                throw new NullReferenceException();
            }

            int offset = 0;
            int chunkLength = 20000;
            
            byte[] resultBytes = new byte[afd.Length];

            while (inputStream.Read(resultBytes, offset, chunkLength) > 0)
            {
                offset += chunkLength;
            }
            
            return resultBytes;
        }

        public void Mix(Note note1, Note note2)
        {
            var mixBytes = _mixer.TextMix2SinWaves(
                20000,
                notesFreqs[note1],
                _gain1,
                20000,
                notesFreqs[note2],
                _gain2);

            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }

        public void Mix(Note note1, Drum drum2)
        {
            byte[] drumBytes;

            if (!drumsBytes.ContainsKey(drum2))
            {
                drumBytes = _provider.GetSound(drumIds[drum2]);
                drumsBytes.Add(drum2, drumBytes);
            }
            else
            {
                drumBytes = drumsBytes[drum2];
            }
            
            byte[] mixBytes = _mixer.TextMixSinWaveWithSoundSample(
                20000,
                notesFreqs[note1],
                _gain1,
                drumBytes,
                drumBytes.Length,
                _gain2);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }

        public void Mix(Drum drum1, Note note2)
        {
            byte[] drumBytes;

            if (!drumsBytes.ContainsKey(drum1))
            {
                drumBytes = _provider.GetSound(drumIds[drum1]);
                drumsBytes.Add(drum1, drumBytes);
            }
            else
            {
                drumBytes = drumsBytes[drum1];
            }
            
            byte[] mixBytes = _mixer.TextMixSinWaveWithSoundSample(
                20000,
                notesFreqs[note2],
                _gain2,
                drumBytes,
                drumBytes.Length,
                _gain1);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }

        public void Mix(Drum drum1, Drum drum2)
        {
            byte[] drumBytes1;

            if (!drumsBytes.ContainsKey(drum1))
            {
                drumBytes1 = _provider.GetSound(drumIds[drum1]);
                drumsBytes.Add(drum1, drumBytes1);
            }
            else
            {
                drumBytes1 = drumsBytes[drum1];
            }
            
            byte[] drumBytes2;

            if (!drumsBytes.ContainsKey(drum2))
            {
                drumBytes2 = _provider.GetSound(drumIds[drum2]);
                drumsBytes.Add(drum2, drumBytes2);
            }
            else
            {
                drumBytes2 = drumsBytes[drum2];
            }

            var mixBytes = _mixer.TextMix2SoundSamples(
                drumBytes1,
                drumBytes1.Length,
                _gain1,
                drumBytes2,
                drumBytes2.Length,
                _gain2);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }
    }
}