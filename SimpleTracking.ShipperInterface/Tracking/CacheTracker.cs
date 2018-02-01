using System;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Caching;
using SimpleTracking.ShipperInterface.ClientServerShared;

namespace SimpleTracking.ShipperInterface.Tracking
{
	/// <summary>
	///		Caches the tracking data that passes through this tracker.
	/// </summary>
	public class CacheTracker : ITracker
	{
		//private readonly static Cache _cache;

		private const int CACHE_DEFAULT_MINUTES = 10;
		private const string CACHE_KEY_PREFIX = "si-tn-";
		private readonly TimeSpan _cacheTime;
		private readonly ITracker _linkedTracker;

		/// <summary>
		///		Creates a new instance of the <see cref="CacheTracker"/> and defaults to
		///		caching the data for 10 minutes.
		/// </summary>
		/// <param name="linkedTracker">
		///		The tracker whose data will be cached. This is only read from if the
		///		tracking data is not in the cache.
		/// </param>
		public CacheTracker(ITracker linkedTracker) : this(linkedTracker, TimeSpan.FromMinutes(CACHE_DEFAULT_MINUTES))
		{
		}

		/// <summary>
		///		Creates a new instance of the <see cref="CacheTracker"/>
		/// </summary>
		/// <param name="linkedTracker">
		///		The tracker whose data will be cached. This is only read from if the
		///		tracking data is not in the cache.
		/// </param>
		/// <param name="cacheTime">
		///		The amount of time to cache the tracking data for. This defaults to
		///		the number of minutes in <see cref="CACHE_DEFAULT_MINUTES" />.
		/// </param>
		public CacheTracker(ITracker linkedTracker, TimeSpan cacheTime)
		{
			_linkedTracker = linkedTracker;
			_cacheTime = cacheTime;
		}

		#region ITracker Members

		/// <summary>
		///		Gets the tracking data for the specified tracking number.
		/// </summary>
		/// <param name="trackingNumber">
		///		The tracking number to load tracking data for.
		/// </param>
		/// <returns>
		///		If the <see cref="TrackingData" /> is cached for this tracking number, then that
		///		data is returned. If the tracking data is not cached, the base
		///		tracker is called and that <see cref="TrackingData" /> is returned.
		/// </returns>
		public TrackingData GetTrackingData(string trackingNumber)
		{
			var td = HttpRuntime.Cache[CACHE_KEY_PREFIX + trackingNumber] as TrackingData;
			if (td == null)
			{
				td = _linkedTracker.GetTrackingData(trackingNumber);
				//If we now have tracking data, cache it
				if(td != null)
					HttpRuntime.Cache.Add(CACHE_KEY_PREFIX + trackingNumber, td, null, DateTime.Now.Add(_cacheTime),
				                      Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
				return td;
			}
			else
			{
				return td;
			}
		}

		#endregion

		/// <summary>
		///		Clears the static cache data. This is only called in debug
		///		mode, and it's only here for unit testing.
		/// </summary>
		[Conditional("DEBUG")]
		public static void ClearCache()
		{
			foreach (DictionaryEntry currCacheObj in HttpRuntime.Cache)
			{
				var cacheKey = (string) currCacheObj.Key;
				if (cacheKey.StartsWith(CACHE_KEY_PREFIX))
					HttpRuntime.Cache.Remove(cacheKey);
			}
		}
	}
}