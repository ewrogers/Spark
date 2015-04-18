using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Spark.Models.Serializers
{
    public static class ClientVersionSerializer
    {
        #region Serialize Methods
        public static IEnumerable<XElement> SerializeAll(this IEnumerable<ClientVersion> clientVersions)
        {
            if (clientVersions == null)
                throw new ArgumentNullException("clientVersions");

            return from x in clientVersions select Serialize(x);
        }

        public static XElement Serialize(this ClientVersion clientVersion)
        {
            if (clientVersion == null)
                throw new ArgumentNullException("clientVersion");

            return new XElement("ClientVersion",
                new XAttribute("Name", clientVersion.Name),
                new XAttribute("Version", clientVersion.VersionCode),
                new XAttribute("Hash", clientVersion.Hash),
                    new XElement("ServerHostnamePatchAddress", clientVersion.ServerHostnamePatchAddress.ToString("X")),
                    new XElement("ServerPortPatchAddress", clientVersion.ServerPortPatchAddress.ToString("X")),
                    new XElement("IntroVideoPatchAddress", clientVersion.IntroVideoPatchAddress.ToString("X")),
                    new XElement("MultipleInstancePatchAddress", clientVersion.MultipleInstancePatchAddress.ToString("X")),
                    new XElement("HideWallsPatchAddress", clientVersion.HideWallsPatchAddress.ToString("X"))
                );
        }
        #endregion

        #region Deserialize Methods
        public static IEnumerable<ClientVersion> DeserializeAll(XContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            return from x in container.Descendants("ClientVersion") select Deserialize(x);
        }

        public static ClientVersion Deserialize(XElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            return new ClientVersion()
            {
                Name = (string)node.Attribute("Name"),
                VersionCode = (int)node.Attribute("Version"),
                Hash = (string)node.Attribute("Hash"),
                ServerHostnamePatchAddress = ParseHexInteger(node.Element("ServerHostnamePatchAddress")),
                ServerPortPatchAddress = ParseHexInteger(node.Element("ServerPortPatchAddress")),
                IntroVideoPatchAddress = ParseHexInteger(node.Element("IntroVideoPatchAddress")),
                MultipleInstancePatchAddress = ParseHexInteger(node.Element("MultipleInstancePatchAddress")),
                HideWallsPatchAddress = ParseHexInteger(node.Element("HideWallsPatchAddress"))
            };
        }
        #endregion

        static long ParseHexInteger(XElement element)
        {
            return long.Parse((string)element, NumberStyles.HexNumber, null);
        }
    }
}
