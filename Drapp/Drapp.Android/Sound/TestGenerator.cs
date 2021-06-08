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

[assembly: Dependency(typeof(TestGenerator))]
namespace Drapp.Android.Sound
{
    public class TestGenerator : ITestGenerator
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

        private Dictionary<Drum, int> drumIds = new Dictionary<Drum, int>();

        private Dictionary<Note, byte[]> notesBytes = new Dictionary<Note, byte[]>();
        private Dictionary<Drum, byte[]> drumsBytes = new Dictionary<Drum, byte[]>();

        private SoundGenerator _generator;
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
            
            _generator = new SoundGenerator(sampleRate);

            _mixer = new SoundMixer();

            PlayNote(Note.E5);
        
            _audioTrack.Play();
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

        public void PlayNote(Note note)
        {
            byte[] noteBytes; 
            
            if (!notesBytes.ContainsKey(note))
            {
                noteBytes = _generator.TestGetSineWave16BitPcm(2000, notesFreqs[note]);
                notesBytes.Add(note, noteBytes);
            }
            else
            {
                noteBytes = notesBytes[note];

            }
            
            _audioTrack.Write(noteBytes, 0, noteBytes.Length);
        }

        public void PlayDrum(Drum drum)
        {
            byte[] drumBytes;

            if (!drumsBytes.ContainsKey(drum))
            {
                drumBytes = _provider.GetSound(drumIds[drum]);
                drumsBytes.Add(drum, drumBytes);
            }
            else
            {
                drumBytes = drumsBytes[drum];
            }
            
            _audioTrack.Write(drumBytes, 0, drumBytes.Length);
        }

        public void PlayMix1()
        {
            byte[] mixBytes;

            mixBytes = _mixer.TextMix2SinWaves(
                40000,
                notesFreqs[Note.A5],
                0.2f,
                20000,
                notesFreqs[Note.D5],
                0.2f);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }

        public void PlayMix2()
        {
            Drum drumForMix = Drum.Snare;
            
            byte[] mixBytes;
            
            byte[] drumBytes;

            if (!drumsBytes.ContainsKey(drumForMix))
            {
                drumBytes = _provider.GetSound(drumIds[drumForMix]);
                drumsBytes.Add(drumForMix, drumBytes);
            }
            else
            {
                drumBytes = drumsBytes[drumForMix];
            }
            
            mixBytes = _mixer.TextMixSinWaveWithSoundSample(
                10000,
                notesFreqs[Note.G5],
                0.2f,
                drumBytes,
                drumBytes.Length,
                0.9f);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }

        public void PlayMix3()
        {
            byte[] mixBytes;
            
            Drum drumForMix1 = Drum.Kick;
            Drum drumForMix2 = Drum.Snare;

            byte[] drumBytes1;

            if (!drumsBytes.ContainsKey(drumForMix1))
            {
                drumBytes1 = _provider.GetSound(drumIds[drumForMix1]);
                drumsBytes.Add(drumForMix1, drumBytes1);
            }
            else
            {
                drumBytes1 = drumsBytes[drumForMix1];
            }
            
            byte[] drumBytes2;

            if (!drumsBytes.ContainsKey(drumForMix2))
            {
                drumBytes2 = _provider.GetSound(drumIds[drumForMix2]);
                drumsBytes.Add(drumForMix2, drumBytes2);
            }
            else
            {
                drumBytes2 = drumsBytes[drumForMix2];
            }

            mixBytes = _mixer.TextMix2SoundSamples(
                drumBytes1,
                drumBytes1.Length,
                0.8f,
                drumBytes2,
                drumBytes2.Length,
                0.3f);
            
            _audioTrack.Write(mixBytes, 0, mixBytes.Length);
        }
    }
}