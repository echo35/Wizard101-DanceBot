using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;

namespace Wizard101_Bot
{
    class SoundCapture
    {
        public static void mute()
        {
            
        }

        public static void listen(long ms)
        {
            using (WasapiCapture capture = new WasapiLoopbackCapture())
            {
                //if nessesary, you can choose a device here
                //to do so, simply set the device property of the capture to any MMDevice
                //to choose a device, take a look at the sample here: http://cscore.codeplex.com/

                double time = DateTime.Now.TimeOfDay.TotalMilliseconds;

                //initialize the selected device for recording
                capture.Initialize();

                //create a wavewriter to write the data to
                using (WaveWriter w = new WaveWriter("dance_r.wav", capture.WaveFormat))
                {
                    bool caught = false;
                    //setup an eventhandler to receive the recorded data
                    capture.DataAvailable += (s, e) =>
                    {
                        //save the recorded audio
                        
                        Console.WriteLine(e.ByteCount);

                        caught = true;

                        //w.Write(e.Data, e.Offset, e.ByteCount);
                    };

                    //start recording
                    capture.Start();

                    while (!caught) { }

                    //stop recording
                    capture.Stop();
                }
            }
        }
    }
}
