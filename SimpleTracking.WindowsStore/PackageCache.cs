using System;
using System.Collections.Generic;
using Windows.Storage;
using SimpleTracking.ShipperInterface.ClientServerShared.Serialization;

namespace SimpleTracking.WindowsStore
{
    public class PackageCache
    {
        private readonly TimeSpan _clientCacheExpiration;
        private readonly IJsonSerializer _jsonSerializer;

        private const string TrackingNumbersContainerName = "TrackingNumbers";
        private const string TrackingDataCacheContainerName = "TrackingDataCache";
        
        public PackageCache(TimeSpan clientCacheExpiration, IJsonSerializer serializer)
        {
            _clientCacheExpiration = clientCacheExpiration;
            _jsonSerializer = serializer;
        }

        public void AddTrackingNumber(string trackingNumber)
        {
            var roaming = ApplicationData.Current.RoamingSettings;
            var roamingContainer = roaming.CreateContainer(TrackingNumbersContainerName, ApplicationDataCreateDisposition.Always);

            if (!roamingContainer.Values.ContainsKey(trackingNumber))
                roamingContainer.Values.Add(trackingNumber, "");
        }

        public void CacheTrackingData(PackageData packageData)
        {
            //The tracking number should have been added, but confirm
            AddTrackingNumber(packageData.TrackingNumber);

            return;

            //var local = ApplicationData.Current.LocalSettings;
            //var localContainer = local.CreateContainer(TrackingDataCacheContainerName, ApplicationDataCreateDisposition.Always);

            //if (localContainer.Values.ContainsKey(packageData.TrackingNumber))
            //    localContainer.Values[packageData.TrackingNumber] = _jsonSerializer.Serialize(packageData);
            //else
            //    localContainer.Values.Add(packageData.TrackingNumber, _jsonSerializer.Serialize(packageData));
        }

        public IEnumerable<string> GetCachedTrackingNumbers()
        {
            var roaming = ApplicationData.Current.RoamingSettings;
            var container = roaming.CreateContainer(TrackingNumbersContainerName, ApplicationDataCreateDisposition.Always);

            return container.Values.Keys;
        }

        public PackageData GetCachedTrackingData(string trackingNumber)
        {
            //Turn off caching until we test it, and figure out a way to invalidate the cache
            return null;

            //var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //var container = localSettings.CreateContainer(TrackingDataCacheContainerName, ApplicationDataCreateDisposition.Always);

            //if (container.Values.ContainsKey(trackingNumber))
            //{
            //    var json = container.Values[trackingNumber] as string;
            //    if (json != null)
            //    {
            //        //Todo:handle json parse exception

            //        return _jsonSerializer.Deserialize<PackageData>(json);
            //    }
            //}

            //return new PackageData {TrackingNumber = trackingNumber};
        }

        public IEnumerable<PackageData> GetAllCachedPackages()
        {
            var cachedTrackingNumbers = GetCachedTrackingNumbers();
            
            foreach (var trackingNumber in cachedTrackingNumbers)
            {
                var cached = GetCachedTrackingData(trackingNumber);
                if (cached != null)
                    yield return cached;
            }
        }

        public bool PackageNeedsRefreshed(PackageData packageData)
        {
            if (packageData == null)
                return true;

            if (packageData.LastClientRefresh == null)
                return true;

            return packageData.LastClientRefresh < DateTime.UtcNow.Subtract(_clientCacheExpiration);
        }

        public void ClearCaches()
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            var roamingContainer = roamingSettings.CreateContainer(TrackingNumbersContainerName, ApplicationDataCreateDisposition.Always);

            roamingContainer.Values.Clear();

            var localSettings = ApplicationData.Current.LocalSettings;
            var localContainer = localSettings.CreateContainer(TrackingDataCacheContainerName, ApplicationDataCreateDisposition.Always);

            localContainer.Values.Clear();
        }
    }
}
