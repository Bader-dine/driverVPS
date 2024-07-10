using System;
using System.Collections.Generic;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;


namespace MIP2VPS
{
    /// <summary>
    /// This class should implement support for requesting stored data from the device to the VMS for remote retrieval and remote playback.
    /// TODO: If this is not supported, remove the class.
    /// </summary>
    public class MIP2VPSPlaybackManager : PlaybackManager
    {
        private readonly object _playbackLockObj = new object();
        private readonly Dictionary<Guid, DateTime> _playbackCursors = new Dictionary<Guid, DateTime>();
        private readonly Dictionary<Guid, int> _sequenceNumbers = new Dictionary<Guid, int>();

        private new MIP2VPSContainer Container => base.Container as MIP2VPSContainer;

        public MIP2VPSPlaybackManager(MIP2VPSContainer container) : base(container)
        {
            base.MaxParallelDevices = 2;
        }

        /// <summary>
        /// Creates a playback session for a device. Note, the recording server can create and destroy sessions for any number of reasons, do not assume
        /// a session will exist for the duration of a playback sequence.
        /// </summary>
        /// <param name="deviceId">The device to start a playback session for.</param>
        /// <returns>A new session id for the created playback session.</returns>
        public override Guid Create(string deviceId)
        {
            if (deviceId != Constants.Video1.ToString())
            {
                throw new NotSupportedException();
            }
            lock (_playbackLockObj)
            {
                Guid id = Guid.NewGuid();
                _playbackCursors[id] = DateTime.UtcNow;
                _sequenceNumbers[id] = 0;
                return id;
            }
        }

        /// <summary>
        /// Destroys a playback session.
        /// </summary>
        /// <param name="playbackId">The playback session id of the session to be destroyed.</param>
        public override void Destroy(Guid playbackId)
        {
            lock (_playbackLockObj)
            {
                if (_playbackCursors.ContainsKey(playbackId))
                {
                    _playbackCursors.Remove(playbackId);
                    _sequenceNumbers.Remove(playbackId);
                }
            }
        }

        /// <summary>
        /// Called to get a list of available sequences in a time window. Called by recording server to provide data to clients when device is set to 
        /// play back recordings directly from the device.
        /// </summary>
        /// <param name="playbackId">Id of the current playback session</param>
        /// <param name="sequenceType">Whether to search for recordings or motion</param>
        /// <param name="dateTime">The central point of the playback window</param>
        /// <param name="maxTimeBefore">Sequences wholly outside this time should not be included.</param>
        /// <param name="maxCountBefore">Maximum number of preceding sequences to include.</param>
        /// <param name="maxTimeAfter">Sequences wholly outside this time should not be included.</param>
        /// <param name="maxCountAfter">Maximum number of following sequences to include.</param>
        /// <returns></returns>
        public override ICollection<SequenceEntry> GetSequences(Guid playbackId, SequenceType sequenceType, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
        {
            // TODO: make request
            return new List<SequenceEntry>();
        }

        /// <summary>
        /// Moves the playback cursor to a specific time. Should attempt to hit an actual frame time.
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <param name="dateTime">The datetime to move to</param>
        /// <param name="moveCriteria">Search criteria specifying how to find the target time:
        /// Before/After: Will look for the first frame before or after, but not at, the initial datetime
        /// AtOrBefore/AtOrAfter: Will include a frame that is exactly at the specified time.
        /// </param>
        /// <returns></returns>
        public override bool MoveTo(Guid playbackId, DateTime dateTime, MoveCriteria moveCriteria)
        {
            lock (_playbackLockObj)
            {
                if (!_playbackCursors.ContainsKey(playbackId))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }
            DateTime cur = dateTime;

            // TODO: implement below to do proper update of cursor
            switch (moveCriteria)
            {
                case MoveCriteria.After:
                    break;
                case MoveCriteria.AtOrAfter:
                    break;
                case MoveCriteria.AtOrBefore:
                    break;
                case MoveCriteria.Before:
                    break;
            }
            lock (_playbackLockObj)
            {
                _playbackCursors[playbackId] = cur;
            }
            return true;
        }

        /// <summary>
        /// Moves the playback cursor in a specified direction. Behavior is undefined if a MoveTo has not been called for this playback session.
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <param name="navigateCriteria">Specifies where to navigate:
        /// First: Moves to the first frame of the first sequence contained on the device.
        /// Last: Moves to the end of the last sequence contained on the device.
        /// Previous: Moves to the previous frame (if JPEG) or GOP (if H.264/H.265). If at the start of a sequence, move to the end of the previous sequence.
        /// Next: Moves to the next frame (if JPEG) or GOP (if H.264/H.265). If at the end of a sequence, move to the start of the next sequence.
        /// PreviousSequence: Moves to the end of the previous sequence.
        /// NextSequence: Moves to the start of the next sequence.
        /// </param>
        /// <returns></returns>
        public override bool Navigate(Guid playbackId, NavigateCriteria navigateCriteria)
        {
            DateTime cur;
            lock (_playbackLockObj)
            {
                if (!_playbackCursors.TryGetValue(playbackId, out cur))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }

            // TODO: implement below to do proper update of cursor
            switch (navigateCriteria)
            {
                case NavigateCriteria.First:
                    break;
                case NavigateCriteria.Last:
                    break;
                case NavigateCriteria.Previous:
                    break;
                case NavigateCriteria.Next:
                    break;
                case NavigateCriteria.PreviousSequence:
                    break;
                case NavigateCriteria.NextSequence:
                    break;
            }
            lock (_playbackLockObj)
            {
                _playbackCursors[playbackId] = cur;
            }
            return true;
        }

        /// <summary>
        /// Retrieves the frame (for JPEG) or GOP (for H.264/H.265) at the playback cursor
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <returns>A response containing the frame or GOP data at the current time of the playback cursor.</returns>
        public override PlaybackReadResponse ReadData(Guid playbackId)
        {
            DateTime cur;
            lock (_playbackLockObj)
            {
                if (!_playbackCursors.TryGetValue(playbackId, out cur))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }

            try
            {
                // TODO: request real data
                byte[] data = new byte[] { };
                if (data == null)
                {
                    Toolbox.Log.Trace("--- No data returned ");
                    return null;
                }

                // TODO: Change to correct values - potentially different codec.
                VideoHeader jpegHeader = new VideoHeader();
                jpegHeader.CodecType = VideoCodecType.JPEG;
                jpegHeader.SyncFrame = true;

                PlaybackFrame frame = new PlaybackFrame() { Data = data, Header = jpegHeader, AnyMotion = true };
                jpegHeader.SequenceNumber = 0;
                jpegHeader.Length = (ulong)data.Length;
                jpegHeader.TimestampFrame = cur;
                jpegHeader.TimestampSync = cur;
                DateTime prev = cur - TimeSpan.FromSeconds(1);
                DateTime next = cur + TimeSpan.FromSeconds(1);
                return new PlaybackReadResponse()
                {
                    SequenceNumber = _sequenceNumbers[playbackId],
                    Next = next,
                    Previous = prev,
                    Frames = new[] { frame },
                };
            }
            catch (Exception e)
            {
                Toolbox.Log.Trace("{0}: Exception={1}", nameof(ReadData), e.Message + e.StackTrace);
                return null;
            }
        }
    }
}
