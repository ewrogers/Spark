using System;
using System.Collections.Generic;
using System.Text;

namespace Spark.Models
{
    [Serializable]
    public sealed class ClientVersion
    {
        public sealed class VersionComparer : IEqualityComparer<ClientVersion>
        {
            public bool Equals(ClientVersion a, ClientVersion b)
            {
                if (Object.ReferenceEquals(a, b))
                    return true;

                if (a == null || b == null)
                    return false;

                return a.VersionCode == b.VersionCode && a.Name.Equals(b.Name, StringComparison.Ordinal);
            }

            public int GetHashCode(ClientVersion version)
            {
                return version.Name.GetHashCode() ^ version.VersionCode.GetHashCode();
            }
        }

        #region Standard Client Versions
        public static readonly ClientVersion Version739 = new ClientVersion()
        {
            Name = "US Dark Ages 7.39",
            VersionCode = 739,
            Hash = "ca31b8165ea7409d285d81616d8ca4f2",      // MD5
            ServerHostnamePatchAddress = 0x4341FA,
            ServerPortPatchAddress = 0x434224,
            IntroVideoPatchAddress = 0x42F48F,
            MultipleInstancePatchAddress = 0x5911AE,
            HideWallsPatchAddress = 0x624BC4
        };

        public static readonly ClientVersion Version737 = new ClientVersion()
        {
            Name = "US Dark Ages 7.37",
            VersionCode = 737,
            Hash = "36f4689b09a4a91c74555b3c3603b196",
            ServerHostnamePatchAddress = 0x4341FA,
            ServerPortPatchAddress = 0x434224,
            IntroVideoPatchAddress = 0x42F48F,
            MultipleInstancePatchAddress = 0x5911AE,
            HideWallsPatchAddress = 0x624BC4
        };
        #endregion

        #region Properties
        public string Name { get; set; }
        public int VersionCode { get; set; }
        public string Hash { get; set; }
        public long ServerHostnamePatchAddress { get; set; }
        public long ServerPortPatchAddress { get; set; }
        public long IntroVideoPatchAddress { get; set; }
        public long MultipleInstancePatchAddress { get; set; }
        public long HideWallsPatchAddress { get; set; }
        #endregion

        public ClientVersion() { }

        public static IEnumerable<ClientVersion> GetDefaultVersions()
        {
            yield return Version737;
            yield return Version739;
        }
    }
}
