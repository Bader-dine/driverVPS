using System;
using System.Linq;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace MIP2VPS
{
    /// <summary>
    /// Class for overall stream handling.
    /// TODO: Update CreateSession to include the stream types supported by your hardware.
    /// </summary>
    public class MIP2VPSStreamManager : SessionEnabledStreamManager
    {
        private new MIP2VPSContainer Container => base.Container as MIP2VPSContainer;

        public MIP2VPSStreamManager(MIP2VPSContainer container) : base(container)
        {
        }

        public override GetLiveFrameResult GetLiveFrame(Guid sessionId, TimeSpan timeout)
        {
            try
            {
                return base.GetLiveFrame(sessionId, timeout);
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Toolbox.Log.Trace("MIP2VPS.StreamManager.GetLiveFrame: Exception={0}", ex.Message);
                return GetLiveFrameResult.ErrorResult(StreamLiveStatus.NoConnection);
            }
        }

        internal BaseStreamSession GetSession(int channel)
        {
            return GetAllSessions().OfType<BaseMIP2VPSStreamSession>()
                .FirstOrDefault(s => s.Channel == channel);
        }

        protected override BaseStreamSession CreateSession(string deviceId, Guid streamId, Guid sessionId)
        {
            Guid dev = new Guid(deviceId);
            // TODO: Modify below to reflect the streams supported by your device
            if (dev == Constants.Video1)
            {
                return new MIP2VPSVideoStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId);
            }
            if (dev == Constants.Speaker1)
            {
                return new MIP2VPSSpeakerStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId);
            }
            if (dev == Constants.Audio1)
            {
                return new MIP2VPSMicrophoneStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId);
            }
            if (dev == Constants.Metadata1)
            {
                return new MIP2VPSMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, 1 /* TODO: replace with correct channel ID */);
            }
            if (dev == Constants.Metadata2)
            {
                return new MIP2VPSMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, 1 /* TODO: replace with correct channel ID */);
            }
            if (dev == Constants.Metadata3)
            {
                return new MIP2VPSMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, 1 /* TODO: replace with correct channel ID */);
            }
            if (dev == Constants.Metadata4)
            {
                return new MIP2VPSMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, 1 /* TODO: replace with correct channel ID */);
            }

            Toolbox.Log.LogError("This device ID: {0} is not supported", deviceId);
            throw new  MIPDriverException();
        }
    }
}
