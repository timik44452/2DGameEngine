using System;
using System.Threading;

namespace WindowsFormsApp5
{
    public class Audio
    {
        Thread thread;

        public void Play(AudioClip clip)
        {
            thread = new Thread(() =>
            {
                for (int i = 0; i < clip.data.Length; i++)
                {
                    Console.Beep(clip.data[i], clip.durations[i]);
                }
            });

            thread.Start();
        }

        public bool IsPlaying()
        {
            return thread != null && thread.IsAlive;
        }
    }
}
