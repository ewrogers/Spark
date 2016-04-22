using System;
using System.Collections.Generic;

namespace Spark.Models
{
    [Serializable]
    public sealed class ClientVersion
    {
        #region Client Version Comparaer
        sealed class ClientVersionComparer : IEqualityComparer<ClientVersion>
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

        public static readonly IEqualityComparer<ClientVersion> VersionComparer = new ClientVersionComparer();
        #endregion

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

        public static readonly ClientVersion Version740 = new ClientVersion()
        {
            Name = "US Dark Ages 7.40",
            VersionCode = 740,
            Hash = "9dc6fb13d0470331bf5ba230343fce42",
            ServerHostnamePatchAddress = 0x4341FA,
            ServerPortPatchAddress = 0x434224,
            IntroVideoPatchAddress = 0x42F48F,
            MultipleInstancePatchAddress = 0x5912AE,
            HideWallsPatchAddress = 0x624CC4
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
            yield return Version740;
        }
    }
}
