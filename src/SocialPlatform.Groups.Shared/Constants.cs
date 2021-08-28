using System;
using System.Collections.Generic;
using System.Text;

namespace SocialPlatform.Groups.Shared
{
    public static class Constants
    {
        // Collections
        public const string GroupCollectionName = "groups";

        // Uris
        public static readonly Uri GroupRegistryUri = new Uri("fabric:/SocialPlatform/SocialPlatform.Groups.Registry");
        public static readonly Uri GroupActorUri = new Uri("fabric:/SocialPlatform/GroupActorService");
    }
}
