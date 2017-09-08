using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;

namespace AudioFadeLibrary
{
    
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Audio
    {
        private bool _muteIfZero = false;
        private bool _isFading = false;
        private MMDeviceCollection activeDevices;
        BackgroundWorker m_oWorker = null;
        public Audio (bool muteIfZero = true)
        {
            _muteIfZero = muteIfZero;
            NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            activeDevices = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.Render,NAudio.CoreAudioApi.DeviceState.Active);
        }
        /// <summary>
        /// Singles this instance.
        /// </summary>
        /// <returns></returns>
        private void SetVolume(float level)
        {
            if (activeDevices != null)
            {
                activeDevices[0].AudioEndpointVolume.MasterVolumeLevelScalar = level;
                
            }

            activeDevices[0].AudioEndpointVolume.Mute = (_muteIfZero && level == 0);
            
        }
        private float GetVolume()
        {
            if (activeDevices != null)
            {
                return activeDevices[0].AudioEndpointVolume.MasterVolumeLevelScalar;
            }
            return 0;
        }
        private float clamp(float value, float lower, float upper)
        {
            if (value > upper)
            {
                value = upper;
            }
            if (value < lower)
            { value = lower; }
            return value;
        }
        private bool isMuted()
        {
            if (activeDevices != null)
            {
                return activeDevices[0].AudioEndpointVolume.Mute;
            }
            return false;
        }
        public float Volume
        {
            get { return GetVolume(); }
            set { SetVolume(value); }
        }
        public bool Muted
        {
            get { return isMuted(); }
            set { activeDevices[0].AudioEndpointVolume.Mute = value; }
        }
        public void FadeOut(int time, float startingVolume, float endingVolume)
        {
            // Work out how much to decrement every 50....
            var slices = time / 50;
            if (time % 50 > 0)
            {
                slices = slices + 1;
            }
            var fadeAmount = (startingVolume - endingVolume) / slices;
            while (slices > 0 )
            {
                var newval = clamp(GetVolume() - fadeAmount,0f,1f);
                SetVolume(newval);
                Thread.Sleep( 50);
                slices --;
            }

        }
    }
}
