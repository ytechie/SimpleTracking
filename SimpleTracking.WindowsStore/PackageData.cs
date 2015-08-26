using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.WindowsStore.Annotations;

namespace SimpleTracking.WindowsStore
{
    public class PackageData : TrackModel, INotifyPropertyChanged
    {
        private int _distanceFromHere;
        private bool _refreshing = false;

        public DateTime? LastClientRefresh { get; set; }

        public int DistanceFromHere
        {
            get
            {
                return _distanceFromHere;
            }
            set
            {
                _distanceFromHere = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string CurrentActivityDescription
        {
            get
            {
                return TrackingData == null ? null : TrackingData.GetCurrentActivityStatus();
                
            }
        }

        public Visibility DeliveryEstimateVisible
        {
            get
            {
                if (TrackingData == null)
                    return Visibility.Collapsed;

                if (TrackingData.EstimatedDelivery != null &&
                    TrackingData.EstimatedDelivery != new DateTime() &&
                    TrackingData.EstimatedDelivery > DateTime.MinValue)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
           
        }

        public bool Refreshing
        {
            get { return _refreshing; }
            set
            {
                _refreshing = value;
                OnPropertyChanged();
            }
        }
    }
}
