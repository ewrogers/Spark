using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Spark.Models
{
    [Serializable]
    public sealed class ClientVersion
    {
        #region Standard Client Versions
        public static readonly ClientVersion Version739 = new ClientVersion()
        {
            Name = "US Dark Ages 7.39",
            VersionCode = 739,
            Hash = "ca31b8165ea7409d285d81616d8ca4f2",      // MD5
            ServerAddressPatchAddress = 0x4341FA,
            ServerPortPatchAddress = 0x43421D,
            IntroVideoPatchAddress = 0x42F48F,
            MultipleInstancePatchAddress = 0x5911AE,
            HideWallsPatchAddress = 0x6B10F8
        };
        #endregion

        #region Properties
        public string Name { get; set; }
        public int VersionCode { get; set; }
        public string Hash { get; set; }
        public long ServerAddressPatchAddress { get; set; }
        public long ServerPortPatchAddress { get; set; }
        public long IntroVideoPatchAddress { get; set; }
        public long MultipleInstancePatchAddress { get; set; }
        public long HideWallsPatchAddress { get; set; }
        #endregion

        public ClientVersion() { }

        #region XML Serialization
        public static void SaveToFile(string filename, IEnumerable<ClientVersion> versions)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            if (versions == null)
                throw new ArgumentNullException("versions");

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Dark Ages Client Versions"),
                new XElement("ClientVersions",
                    from version in versions
                    select new XElement("ClientVersion",
                        new XElement("Name", version.Name),
                        new XElement("VersionCode", version.VersionCode),
                        new XElement("Hash", version.Hash),
                        new XElement("ServerAddressPatchAddress", version.ServerAddressPatchAddress.ToString("X")),
                        new XElement("ServerPortPatchAddress", version.ServerPortPatchAddress.ToString("X")),
                        new XElement("IntroVideoPatchAddress", version.IntroVideoPatchAddress.ToString("X")),
                        new XElement("MultipleInstancePatchAddress", version.MultipleInstancePatchAddress.ToString("X")),
                        new XElement("HideWallsPatchAddress", version.HideWallsPatchAddress.ToString("X"))
                        )
                    )
                );

            xml.Save(filename);
        }

        public static IEnumerable<ClientVersion> LoadFromFile(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            var xml = XDocument.Load(filename);

            var versions = from x in xml.Descendants("ClientVersion")
                           select new ClientVersion()
                           {
                               Name = (string)x.Element("Name"),
                               VersionCode = (int)x.Element("VersionCode"),
                               Hash = (string)x.Element("Hash"),
                               ServerAddressPatchAddress = ParseHexInteger(x.Element("ServerAddressPatchAddress")),
                               ServerPortPatchAddress = ParseHexInteger(x.Element("ServerPortPatchAddress")),
                               IntroVideoPatchAddress = ParseHexInteger(x.Element("IntroVideoPatchAddress")),
                               MultipleInstancePatchAddress = ParseHexInteger(x.Element("MultipleInstancePatchAddress")),
                               HideWallsPatchAddress = ParseHexInteger(x.Element("HideWallsPatchAddress"))
                           };

            return versions;
        }
        #endregion

        static long ParseHexInteger(XElement element)
        {
            return long.Parse((string)element, NumberStyles.HexNumber, null);
        }
    }
}
