/**
 * This script resizes all generated media to match the size and pixel
 * aspect ratio of the project. The script will operate on the
 * selected video event or, if none are selected, it will operate on
 * all video events.
 *
 * Revision Date: March 26, 2010.
 **/

using System;
using System.Collections;
using ScriptPortal.Vegas;
using vegasYtpmvTools;
using logger;

namespace vegasYtpmvTools
{
    public class vegasYtpmvTools : ICustomCommandModule
    {
        private Vegas myVegas;
        CustomCommand MainCMD = new CustomCommand(CommandCategory.Tools, "YTPMV Tools") { DisplayName = "YTPMV Tools" };

        public void InitializeModule(Vegas vegas)
        {
            myVegas = vegas;

            LogWriter.m_exePath = vegas.InstallationDirectory;
        }

        public ICollection GetCustomCommands()
        {
            MainCMD.AddChild(new ImportFlpCMD(myVegas));
            MainCMD.AddChild(new FlipVideoCMD(myVegas));

            return new CustomCommand[] { MainCMD };
        }

    }

}
