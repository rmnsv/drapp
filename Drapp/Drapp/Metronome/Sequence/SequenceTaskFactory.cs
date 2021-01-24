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
                        foreach (SequenceItem item in sequence.Items)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }

                            int itemTime = (int) item.GetTime();

                            if (itemTime > 0)
                            {
                                //TODO: fractioned ms
                                await Task.Delay(itemTime, cancellationToken);
                            }

                            item.Action?.Invoke();
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