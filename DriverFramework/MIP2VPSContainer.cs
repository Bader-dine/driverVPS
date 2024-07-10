using VideoOS.Platform.DriverFramework;

namespace MIP2VPS
{
    /// <summary>
    /// Container holding all the different managers.
    /// TODO: If your hardware does not support some of the functionality, you can remove the class and the instantiation below.
    /// </summary>
    public class MIP2VPSContainer : Container
    {
        public new MIP2VPSConnectionManager ConnectionManager => base.ConnectionManager as MIP2VPSConnectionManager;
        public new MIP2VPSStreamManager StreamManager => base.StreamManager as MIP2VPSStreamManager;

        public MIP2VPSContainer(DriverDefinition definition)
            : base(definition)
        {
            base.StreamManager = new MIP2VPSStreamManager(this);
            base.PtzManager = new MIP2VPSPtzManager(this);
            base.OutputManager = new MIP2VPSOutputManager(this);
            base.SpeakerManager = new MIP2VPSSpeakerManager(this);
            base.PlaybackManager = new MIP2VPSPlaybackManager(this);
            base.ConnectionManager = new MIP2VPSConnectionManager(this);
            base.ConfigurationManager = new MIP2VPSConfigurationManager(this);
        }
    }
}
