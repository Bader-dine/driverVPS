using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace MIP2VPS
{
    /// <summary>
    /// Class for working with one speaker audio stream session
    /// </summary>
    internal class MIP2VPSSpeakerStreamSession : BaseMIP2VPSStreamSession
    {
        private byte[] _currentSpeakerData = null;
        private AudioHeader _currentSpeakerHeader = null;

        public MIP2VPSSpeakerStreamSession(ISettingsManager settingsManager, MIP2VPSConnectionManager connectionManager, Guid sessionId, string deviceId, Guid streamId) :
             base(settingsManager, connectionManager, sessionId, deviceId, streamId)
        {
            // TODO: Set Channel to correct channel number
            Channel = 1;
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            header = null;
            data = null;
            if (_currentSpeakerData != null && _currentSpeakerHeader != null)
            {
                header = _currentSpeakerHeader.Clone();
                data = _currentSpeakerData;
                _currentSpeakerData = null;
                return true;
            }
            return false;
        }

        public void StoreFrameForLoopback(AudioHeader ah, byte[] data)
        {
            _currentSpeakerHeader = ah;
            _currentSpeakerData = data;
        }
    }
}
