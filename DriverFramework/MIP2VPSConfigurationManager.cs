using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;

namespace MIP2VPS
{
    /// <summary>
    /// This class returns information about the hardware including capabilities and settings supported.
    /// TODO: Update it to match what is supported by your hardware.
    /// </summary>
    public class MIP2VPSConfigurationManager : ConfigurationManager
    {
        private const string _firmware = "MIP2VPS Firmware";
        private const string _firmwareVersion = "1.0";
        private const string _hardwareName = "MIP2VPS Hardware";
        private const string _serialNumber = "12345";

        private new MIP2VPSContainer Container => base.Container as MIP2VPSContainer;

        public MIP2VPSConfigurationManager(MIP2VPSContainer container) : base(container)
        {
        }

        protected override ProductInformation FetchProductInformation()
        {
            if (!Container.ConnectionManager.IsConnected)
            {
                throw new ConnectionLostException("Connection not established");
            }

            var driverInfo = Container.Definition.DriverInfo;
            var product = driverInfo.SupportedProducts.FirstOrDefault();
            var macAddress = "DE:AD:C0:DE:56:78"; // TODO: Make request to hardware

            return new ProductInformation
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductVersion = driverInfo.Version,
                MacAddress = macAddress,
                FirmwareVersion = _firmwareVersion,
                Firmware = _firmware,
                HardwareName = _hardwareName,
                SerialNumber = _serialNumber
            };
        }

        protected override IDictionary<string, string> BuildHardwareSettings()
        {
            return new Dictionary<string, string>()
            {
                // TODO: Add settings supported by the hardware
            };
        }

        protected override ICollection<ISetupField> BuildFields()
        {
            var fields = new List<ISetupField>();

            // TODO: Add definition of setup fields supported by hardware and devices

            return fields;
        }

        protected override ICollection<EventDefinition> BuildHardwareEvents()
        {
            var hardwareEvents = new List<EventDefinition>();

            // TODO: Add supported hardware level events

            return hardwareEvents;
        }

        protected override ICollection<DeviceDefinitionBase> BuildDevices()
        {
            var devices = new List<DeviceDefinitionBase>();

            devices.Add(new CameraDeviceDefinition()
            {
                DisplayName = "MIP2VPS camera",
                DeviceId = Constants.Video1.ToString(),
                DeviceEvents = BuildDeviceEvents(),
                Settings = new Dictionary<string, string>()
                {
                    // TODO: Add settings supported by the device - also for the other devices below.
                },
                Streams = BuildCameraStreams(),
                // Leave PtzSupport set to null if PTZ is not supported
                PtzSupport = BuildPtzSupport(),
            });

            // TODO: If supported by the hardware, add more camera devices (same for below device types). Also remove the devices not supported.

            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "MIP2VPS metadata device",
                DeviceId = Constants.Metadata1.ToString(),
                Streams = BuildMetadataStreams(),
            });
            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "MIP2VPS metadata device",
                DeviceId = Constants.Metadata2.ToString(),
                Streams = BuildMetadataStreams(),
            });
            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "MIP2VPS metadata device",
                DeviceId = Constants.Metadata3.ToString(),
                Streams = BuildMetadataStreams(),
            });
            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "MIP2VPS metadata device",
                DeviceId = Constants.Metadata4.ToString(),
                Streams = BuildMetadataStreams(),
            });

            devices.Add(new MicrophoneDeviceDefinition()
            {
                DisplayName = "MIP2VPS microphone",
                DeviceId = Constants.Audio1.ToString(),
                Streams = BuildAudioStream(),
            });

            devices.Add(new OutputDeviceDefinition()
            {
                DisplayName = "MIP2VPS output",
                DeviceId = Constants.Output1.ToString(),
                SupportSetState = true,
                SupportTrigger = true,
            });

            devices.Add(new InputDeviceDefinition()
            {
                DisplayName = "MIP2VPS input",
                DeviceId = Constants.Input1.ToString(),
            });

            devices.Add(new SpeakerDeviceDefinition()
            {
                DisplayName = "MIP2VPS speaker",
                DeviceId = Constants.Speaker1.ToString(),
                Streams = BuildSpeakerStream()
            });

            return devices;
        }

        private static ICollection<StreamDefinition> BuildCameraStreams()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "MIP2VPS video stream",
                ReferenceId = Constants.VideoStream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    // TODO: Add settings supported by the stream
                },
                RemotePlaybackSupport = true,
            });

            return streams;
        }

        private static ICollection<StreamDefinition> BuildAudioStream()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "MIP2VPS audio stream",
                ReferenceId = Constants.AudioStream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    // TODO: Add settings supported by the stream
                },
            });
            return streams;
        }

        private static ICollection<StreamDefinition> BuildSpeakerStream()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "MIP2VPS speaker stream",
                ReferenceId = Constants.SpeakerStream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    // TODO: Add settings supported by the stream
                },
            });
            return streams;
        }

        private static ICollection<StreamDefinition> BuildMetadataStreams()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "MIP2VPS metadata stream",
                ReferenceId = MetadataType.BoundingBoxDisplayId.ToString(), // TODO: Potentially change this to one of the other supported meatadata stream types
                MetadataTypes = new List<MetadataTypeDefinition>()
                {
                    // TODO: Add metadata types
                }
            });
            return streams;
        }

        private static ICollection<EventDefinition> BuildDeviceEvents()
        {
            var deviceEvents = new List<EventDefinition>();

            // TODO: Add events supported by device.
            return deviceEvents;
        }

        private static PtzSupport BuildPtzSupport()
        {
            // TODO: Update below to reflect actual PTZ support.

            PtzMoveSupport moveSupport = new PtzMoveSupport()
            {
                AbsoluteSupport = true,
                AutomaticSupport = false,
                RelativeSupport = true,
                SpeedSupport = true,
                StartSupport = true,
                StopSupport = true,
            };

            PtzMoveSupport moveSupportZoom = new PtzMoveSupport()
            {
                AbsoluteSupport = true,
                AutomaticSupport = false,
                RelativeSupport = true,
                SpeedSupport = false,
                StartSupport = true,
                StopSupport = true,
            };

            PresetSupport presetSupport = new PresetSupport()
            {
                AbsoluteSpeedSupport = false,
                LoadFromDeviceSupport = true,
                QueryAbsolutePositionSupport = true,
                SetPresetSupport = true,
                SpeedSupport = true,
            };

            PtzSupport ptzSupport = new PtzSupport()
            {
                CenterSupport = true,
                DiagonalSupport = true,
                HomeSupport = true,
                RectangleSupport = true,
                PanSupport = moveSupport,
                TiltSupport = moveSupport,
                ZoomSupport = moveSupportZoom,
                PresetSupport = presetSupport,
            };

            return ptzSupport;
        }
    }
}
