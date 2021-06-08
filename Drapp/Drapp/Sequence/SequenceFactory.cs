using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Drapp.Metronome;
using Drapp.Metronome.Sequence;
using Drapp.Pattern;

namespace Drapp.Sequence
{
    internal static class SequenceFactory
    {
        internal static Task CreatePlaybackTask(ISequence sequence, CancellationToken cancellationToken)
        {
            return new Task(() => {
                try
                {
                    while (true)
                    {
                        sequence.ToBegin();

                        while (sequence.ToNext())
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            int idleTime = sequence.GetCurrentIdleTime();

                            if (idleTime == 0)
                            {
                                sequence.ExecuteCurrent();
                            }
                            else
                            {
                                Stopwatch sw = Stopwatch.StartNew();
                                
                                while (sw.ElapsedTicks <= idleTime)
                                {
                                    if (!cancellationToken.IsCancellationRequested)
                                    {
                                        continue;
                                    }

                                    sw.Stop();
                                    return;
                                }
                                
                                sequence.ExecuteCurrent();
                                sw.Stop();
                            }
                        }
                    }
                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine("Metronome task is cancelled.");
                }
            }, cancellationToken);
        }
        
        internal static ISequence CreateSequence(int mainInterval, MetronomePattern pattern)
        {
            //TODO: implement
            /*SequenceEntity sequence = new SequenceEntity(mainInterval, pattern.MaxMeasurements);
            
            sequence.AddItem(0, () => Console.WriteLine("Main interval begin!"));

            foreach (var item in pattern.PatternInner)
            {
                sequence.AddItem(item.Key, () =>
                {
                    OnVisualBeatOn?.Invoke(item.Value);
                    _beepService?.Beep(item.Value);
                });
            }
            
            sequence.AddItem((byte) (pattern.MaxMeasurements - 1), () => Console.WriteLine("Main interval end!"));

            return sequence;*/
            return null;
        }
        
        internal static Task GetNewOLD(SequenceEntity sequence, CancellationToken cancellationToken)
        {
            return new Task(() => {
                try
                {
                    Stopwatch sw;

                    while (true)
                    {
                        for (byte i = 0; i < sequence.MeasurementsCount; i++)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                return;
                            }
                            
                            sw = Stopwatch.StartNew();

                            sequence.PerformActionAt(i);

                            int itemTime = sequence.GetMeasurementTime();

                            if (itemTime > 0)
                            {
                                while (sw.ElapsedTicks <= itemTime)
                                {
                                    if (!cancellationToken.IsCancellationRequested) continue;
                                    sw.Stop();
                                    return;
                                }
                                
                                sw.Stop();
                            }
                        }
                    }
                } catch (TaskCanceledException e)
                {
                    Console.WriteLine("Metronome task is cancelled.");
                }
            }, cancellationToken);
        }
    }
}