﻿using musicApp.packages;
using musicApp.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace musicApp
{
    static class Program
    {
        public static bool DebugMode = true;
        public static ProgramMeta metaFile;
        public static string fileMetaPath;

        public static PluginLoader loader;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            if (DebugMode) { Console.WriteLine("Hello and welcome to debug mode of the Slap My Meat Audio Mixer"); }
            if(metaFile == null)
            {
                if(fileMetaPath == null) { fileMetaPath = Path.Combine(Directory.GetCurrentDirectory(), "/metaFile"); }
                metaFile.LoadData(fileMetaPath);
            }
            if (DebugMode) { Console.WriteLine("Loaded metadata file..."); }
            //We want to load the plugins into the system!!!
            if(loader == null)
            {
                loader = new PluginLoader();
                loader.LoadPlugins();
            }
            if (DebugMode) { Console.WriteLine("Loaded all plugins!!!"); }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(stopConsole);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Timeline());
        }

        public static void stopConsole(object sender, EventArgs e)
        {
            //stopping console.
            if (DebugMode) { Console.WriteLine("Stopping Console..."); }
            metaFile.SaveData(fileMetaPath);
            
        }
    }
}
