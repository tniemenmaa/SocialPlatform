using System;

namespace SocialPlatform.Groups.Shared
{
    public static class Constants
    {
        /// <summary>
        /// Group collection name for reliable collections.
        /// </summary>
        public const string GroupCollectionName = "groups";

        /// <summary>
        /// Uri for group registry service
        /// </summary>
        public static readonly Uri GroupRegistryUri = new Uri("fabric:/SocialPlatform/SocialPlatform.Groups.Registry");

        /// <summary>
        /// Uri for group actor service
        /// </summary>
        public static readonly Uri GroupActorUri = new Uri("fabric:/SocialPlatform/GroupActorService");
    }
}
