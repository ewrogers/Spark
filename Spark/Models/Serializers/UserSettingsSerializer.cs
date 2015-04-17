using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Spark.Models.Serializers
{
    public static class UserSettingsSerializer
    {
        #region Serialize Methods
        public static IEnumerable<XElement> SerializeAll(this IEnumerable<UserSettings> userSettings)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");

            return from x in userSettings select Serialize(x);
        }

        public static XElement Serialize(this UserSettings userSettings)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");

            return new XElement("UserSettings",
                new XElement("ClientExecutablePath", userSettings.ClientExecutablePath),
                new XElement("ClientVersion", userSettings.ClientVersion,
                        new XAttribute("AutoDetect", userSettings.ShouldAutoDetectClientVersion)),
                new XElement("ServerHostname", userSettings.ServerHostname),
                new XElement("ServerPort", userSettings.ServerPort),
                new XElement("RedirectClient", userSettings.ShouldRedirectClient),
                new XElement("SkipIntro", userSettings.ShouldSkipIntro),
                new XElement("AllowMultipleInstances", userSettings.ShouldAllowMultipleInstances),
                new XElement("HideWalls", userSettings.ShouldHideWalls)
                );
        }
        #endregion

        #region Deserialize Methods
        public static IEnumerable<UserSettings> DeserializeAll(XContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("xml");

            return from x in container.Descendants("UserSettings") select Deserialize(x);
        }

        public static UserSettings Deserialize(XElement node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            return new UserSettings()
            {
                ClientExecutablePath = (string)node.Element("ClientExecutablePath"),
                ClientVersion = (string)node.Element("ClientVersion"),
                ShouldAutoDetectClientVersion = (bool)node.Element("ClientVersion").Attribute("AutoDetect"),
                ServerHostname = (string)node.Element("ServerHostname"),
                ServerPort = (int)node.Element("ServerPort"),
                ShouldRedirectClient = (bool)node.Element("RedirectClient"),
                ShouldSkipIntro = (bool)node.Element("SkipIntro"),
                ShouldAllowMultipleInstances = (bool)node.Element("AllowMultipleInstances"),
                ShouldHideWalls = (bool)node.Element("HideWalls")
            };
        }
        #endregion
    }
}
