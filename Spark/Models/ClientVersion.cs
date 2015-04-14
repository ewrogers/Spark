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
            Name = "7.39",
            VersionCode = 739,
            Hash = "ca31b8165ea7409d285d81616d8ca4f2",      // MD5
            ServerAddressPatchAddress = 0x4341FA,
            ServerPortPatchAddress = 0x43421D,
            IntroVideoPatchAddress = 0x42F495,
            MultipleInstancePatchAddress = 0x05911A9,
            HideWallsPatchAddress = 0x624BD5
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

        public ClientVersion()
        {

        }

        #region XML Serialization
        public XElement ToXElement()
        {
            var xml = new XElement("ClientVersion",
                        new XElement("Name", this.Name),
                        new XElement("VersionCode", this.VersionCode),
                        new XElement("Hash", this.Hash),
                        new XElement("ServerAddressPatchAddress", this.ServerAddressPatchAddress.ToString("X")),
                        new XElement("ServerPortPatchAddress", this.ServerPortPatchAddress.ToString("X")),
                        new XElement("IntroVideoPatchAddress", this.IntroVideoPatchAddress.ToString("X")),
                        new XElement("MultipleInstancePatchAddress", this.MultipleInstancePatchAddress.ToString("X")),
                        new XElement("HideWallsPatchAddress", this.HideWallsPatchAddress.ToString("X"))
                    );

            return xml;
        }

        public static IEnumerable<ClientVersion> FromXElement(XElement xml)
        {
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
