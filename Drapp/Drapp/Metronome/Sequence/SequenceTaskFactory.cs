using System;
using System.Threading;
using System.Threading.Tasks;

namespace Drapp.Metronome.Sequence
{
    internal static class SequenceTaskFactory
    {
        internal static Task GetNew(SequenceEntity sequence, CancellationToken cancellationToken)
        {
            return new Task(async () => {
                try
                {
                    while (true)
                    {
                        for (byte i = 0; i < sequence.Segmentation; i++)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                            
                            sequence.PerformActionAt(i);

                            int itemTime = (int) sequence.GetSegmentTime();

                            if (itemTime > 0)
                            {
                                //TODO: fractioned ms
                                await Task.Delay(itemTime, cancellationToken);
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