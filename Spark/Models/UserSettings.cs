using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spark.Models
{
    [Serializable]
    public sealed class UserSettings
    {
        public static readonly string DefaultHostname = "da0.kru.com";
        public static readonly int DefaultPort = 2610;

        #region Properties
        public string ClientExecutablePath { get; set; }
        public string ClientVersion { get; set; }
        public string ServerHostname { get; set; }
        public int ServerPort { get; set; }
        public bool ShouldRedirectClient { get; set; }
        public bool ShouldSkipIntro { get; set; }
        public bool ShouldAllowMultipleInstances { get; set; }
        public bool ShouldHideWalls { get; set; }
        #endregion

        public UserSettings() { }

        public void ResetToDefaults()
        {
            // Get "Program Files (x86)" on 64bit OS, else "Program Files" on 32bit OS
            var programFilesPath = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
            this.ClientExecutablePath = Path.Combine(programFilesPath, "KRU", "Dark Ages", "Darkages.exe");
            this.ClientVersion = "Auto-Detect";

            this.ServerHostname = DefaultHostname;
            this.ServerPort = DefaultPort;

            this.ShouldRedirectClient = true;
            this.ShouldSkipIntro = true;
            this.ShouldAllowMultipleInstances = true;
            this.ShouldHideWalls = false;
        }

        public static UserSettings CreateDefaults()
        {
            var settings = new UserSettings();
            settings.ResetToDefaults();

            return settings;
        }

        #region XML Serialization
        public void SaveToFile(string filename)
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XComment("Spark User Settings"),
                new XElement("UserSettings",
                    new XElement("ClientExecutablePath", this.ClientExecutablePath),
                    new XElement("ClientVersion", this.ClientVersion),
                    new XElement("ServerHostname", this.ServerHostname),
                    new XElement("ServerPort", this.ServerPort),
                    new XElement("RedirectClient", this.ShouldRedirectClient),
                    new XElement("SkipIntro", this.ShouldSkipIntro),
                    new XElement("AllowMultipleInstances", this.ShouldAllowMultipleInstances),
                    new XElement("HideWalls", this.ShouldHideWalls)
                    )
                );


            xml.Save(filename);
        }

        public static UserSettings LoadFromFile(string filename)
        {
            var xml = XDocument.Load(filename);

            var settings = from x in xml.Descendants("UserSettings")
                           select new UserSettings()
                           {
                               ClientExecutablePath = (string)x.Element("ClientExecutablePath"),
                               ClientVersion = (string)x.Element("ClientVersion"),
                               ServerHostname = (string)x.Element("ServerHostname"),
                               ServerPort = (int)x.Element("ServerPort"),
                               ShouldRedirectClient = (bool)x.Element("RedirectClient"),
                               ShouldSkipIntro = (bool)x.Element("SkipIntro"),
                               ShouldAllowMultipleInstances = (bool)x.Element("AllowMultipleInstances"),
                               ShouldHideWalls = (bool)x.Element("HideWalls")
                           };

            return settings.FirstOrDefault();
        }
        #endregion
    }
}
