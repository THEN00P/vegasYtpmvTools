using ScriptPortal.Vegas;
using System;
using System.Collections.Generic;

namespace vegasYtpmvTools
{
    class FlipVideoCMD : CustomCommand
    {
        private const string moduleName = "Filp Even Videos";
        private Vegas vegas;

        public FlipVideoCMD(Vegas vegas) : base(CommandCategory.Tools, moduleName)
        {
            this.vegas = vegas;
            this.DisplayName = moduleName;
            this.Invoked += FlipVideoCMD_Invoked;
        }

        private void FlipVideoCMD_Invoked(object sender, EventArgs e)
        {
            using (var undo = new UndoBlock(moduleName))
            {
                foreach(var track in vegas.Project.Tracks)
                {
                    if(track.IsVideo())
                    {
                        var videoTrack = track as VideoTrack;

                        var selectedItemCount = 0;

                        foreach(var trackEvent in videoTrack.Events)
                        {
                            if(trackEvent.IsVideo())
                            {
                                var videoTrackEvent = trackEvent as VideoEvent;

                                if(videoTrackEvent.Selected)
                                {
                                    selectedItemCount++;

                                    if(selectedItemCount % 2 == 0)
                                    {
                                        foreach(var keyframe in videoTrackEvent.VideoMotion.Keyframes)
                                        {
                                            keyframe.Bounds = new VideoMotionBounds(
                                                keyframe.TopRight,
                                                keyframe.TopLeft,
                                                keyframe.BottomLeft,
                                                keyframe.BottomRight
                                            );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
