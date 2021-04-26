using ScriptPortal.Vegas;
using System;
using logger;
using System.Collections.Generic;
using Monad.FLParser;
using System.Windows.Forms;

namespace vegasYtpmvTools
{
    class ImportFlpCMD : CustomCommand
    {
        private const string moduleName = "Import FL Studio Project";
        private Vegas vegas;
        private Monad.FLParser.Project selectedFlp;

        public ImportFlpCMD(Vegas vegas) : base(CommandCategory.Tools, moduleName)
        {
            this.vegas = vegas;
            this.DisplayName = moduleName;
            this.Invoked += ImportFlpCMD_Invoked;
        }

        private void ImportFlpCMD_Invoked(object sender, EventArgs e)
        {
            try
            {
                string selectedFile = "C:\\Users\\Lorenz\\Desktop\\demo.flp";

                vegas.FileUtilities.SelectFileDlg(vegas.MainWindow.Handle, "Load Project File", "FL Studio Project (*.flp)", null, out selectedFile, 0);

                this.selectedFlp = Monad.FLParser.Project.Load(selectedFile, true);

                FlpForm dockView = new FlpForm("FLPForm", selectedFlp);
                dockView.OnImport += importCallback;
                this.vegas.LoadDockView(dockView);
            }
            catch (Exception err)
            {
                LogWriter.LogWrite(err.ToString());
                throw err;
            }
        }

        private void importCallback(object sender, EventArgs e)
        {
            LogWriter.LogWrite("Callback");

            var importOptions = e as ImportEventArgs;

            using (var undo = new UndoBlock(moduleName))
            {
                try
                {
                    var frameOffsetInMs = (1000 / vegas.Project.Video.FrameRate) * importOptions.FrameOffset;

                    var beatLengthInS = 60 / selectedFlp.Tempo;
                    var ppqLengthInS = beatLengthInS / selectedFlp.Ppq;


                    PlugInNode solidColor = vegas.Generators.GetChildByUniqueID("{Svfx:com.sonycreativesoftware:solidcolor}");

                    vegas.Project.Markers.Clear();

                    List<List<FullTrackTimes>> fullTrackTimesList = new List<List<FullTrackTimes>>();

                    foreach (int i in importOptions.SelectedIndices)
                    {
                        List<FullTrackTimes> fullTrackTimes = new List<FullTrackTimes>();

                        foreach (var j in selectedFlp.Tracks[i].Items)
                        {
                            if (j is PatternPlaylistItem)
                            {
                                var patternItem = j as PatternPlaylistItem;

                                foreach (Channel channel in patternItem.Pattern.Notes.Keys)
                                {
                                    foreach (Note note in patternItem.Pattern.Notes[channel])
                                    {
                                        if (note.Position < patternItem.StartOffset)
                                            continue;
                                        if (note.Position > patternItem.Length)
                                            continue;

                                        var noteStartPpq = patternItem.Position + (note.Position - patternItem.StartOffset);
                                        var startInMs = noteStartPpq * ppqLengthInS * 1000;

                                        var noteEndInMs = note.Length * ppqLengthInS * 1000;

                                        startInMs =
                                            (frameOffsetInMs + startInMs < 0) ?
                                                (int)(frameOffsetInMs + startInMs) :
                                                (int)startInMs;

                                        noteEndInMs =
                                            (frameOffsetInMs + noteEndInMs < 0) ?
                                                (int)(frameOffsetInMs + noteEndInMs) :
                                                (int)noteEndInMs;

                                        fullTrackTimes.Add(new FullTrackTimes(startInMs, noteEndInMs));
                                    }
                                }
                            }
                            //if(j is ChannelPlaylistItem)
                            //{
                            //    var channelItem = j as ChannelPlaylistItem;

                            //    if(channelItem.Channel.Data is GeneratorData)
                            //    {
                            //        var audioStartPpq = channelItem.Position;
                            //        var startInMs = audioStartPpq * ppqLengthInS * 1000;

                            //        var audioEndPpq = audioStartPpq + channelItem.Length;
                            //        var noteEndInMs = audioEndPpq * ppqLengthInS * 1000;

                            //        fullTrackTimes.Add();
                            //    }
                            //}

                        }

                        fullTrackTimesList.Add(fullTrackTimes);
                    }
                    foreach(var trackTimes in fullTrackTimesList)
                    {
                        if (importOptions.ImportType == "Solid color")
                        {
                            Media solidColorMedia = Media.CreateInstance(vegas.Project, solidColor);
                            VideoTrack v = vegas.Project.AddVideoTrack();

                            foreach (var fullTrackTimes in trackTimes)
                            {
                                if (importOptions.ImportType == "Solid color")
                                {
                                    VideoEvent ve = v.AddVideoEvent(new Timecode(fullTrackTimes.start), new Timecode(fullTrackTimes.end));
                                    //ve.
                                    ve.AddTake(solidColorMedia.Streams[0]);
                                }
                            }
                        }
                        else if (importOptions.ImportType == "Marker")
                        {
                            foreach (var fullTrackTimes in trackTimes)
                            {
                                if (importOptions.ImportType == "Marker")
                                {
                                    vegas.Project.Markers.Add(new Marker(new Timecode(fullTrackTimes.start)));
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    LogWriter.LogWrite(err.ToString());
                    throw err;
                }
            }
        }
    }

    class FullTrackTimes
    {
        public readonly double start;
        public readonly double end;
        public readonly double length;
        public readonly double startOffset;

        public FullTrackTimes(double start, double end)
        {
            this.start = start;
            this.end = end;
        }
        public FullTrackTimes(double start, double end, double startOffset, double length)
        {
            this.start = start;
            this.end = end;
            this.length = length;
            this.startOffset = startOffset;
        }
    }
}
