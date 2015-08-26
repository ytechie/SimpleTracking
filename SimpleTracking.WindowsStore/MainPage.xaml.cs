using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Bing.Maps;
using Newtonsoft.Json;
using SimpleTracking.ShipperInterface.ClientServerShared;
using SimpleTracking.ShipperInterface.ClientServerShared.Serialization;
using SimpleTracking.WindowsStore.DesignerData;
using SimpleTracking.WindowsStore.Common;

namespace SimpleTracking.WindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<PackageData> Packages { get; private set; }

        //private readonly PackageCache _packageCache;

        private const string TrackingNumbersRoamingKey = "tracking-numbers";

        public MainPage()
        {
            Packages = new ObservableCollection<PackageData>();         

            var navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;

            
            
            this.InitializeComponent();
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
         { }
         private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
         { }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //_packageCache.ClearCaches();
            LoadPackages();
            InitGps();
            
            base.OnNavigatedTo(e);
        }

        private async void LoadPackages()
        {
            var packageRefreshTasks = new List<Task>();
            //Load tracking numbers
            var trackingNumbers = ApplicationData.Current.RoamingSettings.Values[TrackingNumbersRoamingKey] as string[];

            if (trackingNumbers != null)
            {

                foreach (var trackingNumber in trackingNumbers)
                {
                    if (trackingNumber == AddNewPackageTemplateSelector.GetAddItem().TrackingNumber)
                        continue;
                    if (Packages.Count(x => x.TrackingNumber == trackingNumber) > 0)
                        continue;

                    var package = new PackageData {TrackingNumber = trackingNumber};
                    Packages.Add(package);
                    packageRefreshTasks.Add(RefreshPackage(package));
                }
            }

            Packages.Add(AddNewPackageTemplateSelector.GetAddItem());
            await Task.WhenAll(packageRefreshTasks.ToArray());
        }

        private async Task RefreshPackage(PackageData packageData)
        {
            packageData.Refreshing = true;

            string json;
            using (var http = new HttpClient())
            {
                //json = await http.GetStringAsync("http://localhost/SimpleTracking.Web/track/sim1.json");
                //json = await http.GetStringAsync("http://localhost/SimpleTracking.Web/track/1Z20377A0297737558.json");
                json = PackagesDesignerData.GetEmbeddedJsonSample("RealUpsExample");
                await Task.Delay(TimeSpan.FromSeconds(3));
            }

            var trackingNumber = packageData.TrackingNumber;
            JsonConvert.PopulateObject(json, packageData, new JsonSerializerSettings());
            if (trackingNumber != null)
                packageData.TrackingNumber = trackingNumber;

            packageData.LastClientRefresh = DateTime.UtcNow;
            packageData.Refreshing = false;
            //_packageCache.CacheTrackingData(packageData);
        }

        private void MyMap_OnLoaded(object sender, RoutedEventArgs e)
        {
            var map = (Bing.Maps.Map) sender;
            var mapShapeLayer = new MapShapeLayer();
            var polyLine = new MapPolyline();

            var activities = map.Tag as List<Activity>;
            if (activities == null)
                return;

            var pinNumber = 0;
            foreach (var activity in activities
                .GroupBy(x => x.Latitude + x.Longitude) //ignore dupe coordinates
                .Select(g => g.First())
                .OrderBy(x => x.Timestamp))
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (activity.Latitude != 0 && activity.Longitude != 0)
                {
                    polyLine.Locations.Add(new Location(activity.Latitude, activity.Longitude));

                    pinNumber++;
                    var pin = new Pushpin();
                    pin.Text = pinNumber.ToString();
                    pin.FontSize = 13; //this size works good for single or double digits
                    
                    MapLayer.SetPosition(pin, new Location(activity.Latitude, activity.Longitude));
                    map.Children.Add(pin);
                }
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }
            
            polyLine.Width = 3;
            mapShapeLayer.Shapes.Add(polyLine);
            map.ShapeLayers.Add(mapShapeLayer);

            //Calculate a bounding box for the route
            var t = activities.Where(x => x.Latitude != 0).Max(x => x.Latitude);
            var b = activities.Where(x => x.Latitude != 0).Min(x => x.Latitude);
            var l = activities.Where(x => x.Longitude != 0).Min(x => x.Longitude);
            var r = activities.Where(x => x.Longitude != 0).Max(x => x.Longitude);

            //Pad the box
            var latPad = (t - b)*.3;
            var longPad = (r - l)*.3;
            
            map.SetView(new LocationRect(new Location(t + latPad,l-longPad), new Location(b - latPad,r + longPad)), MapAnimationDuration.None);
        }

        private async void InitGps()
        {
            var loc = new Geolocator();
            try
            {
                loc.DesiredAccuracy = PositionAccuracy.Default;
                var pos = await loc.GetGeopositionAsync();
                //var lat = pos.Coordinate.Latitude;
                //var lang = pos.Coordinate.Longitude;
                //Console.WriteLine(lat + " " + lang);

                // Windows.Devices.Geolocation.
                //pos.Coordinate.GetDistanceTo()
                foreach (var package in Packages)
                {
                    var activity = package.TrackingData.Activity
                        .Where(x => x.Latitude != 0 && x.Longitude != 0)
                        .OrderBy(x => x.Timestamp);
                    if (activity.Any())
                    {
                        var packageGeo = new Location
                        {
                            Latitude = activity.Last().Latitude,
                            Longitude = activity.Last().Longitude
                        };

                        //Distance is in meters, convert to miles
                        package.DistanceFromHere = (int) (pos.Coordinate.GetDistanceTo(packageGeo)/1609.344);
                    }
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                // handle error
            }
            catch (Exception)
            {
                
            }
        }

        private async void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var packageData = e.ClickedItem as PackageData;

            if (packageData == null)
                return;

            if (AddNewPackageTemplateSelector.IsAddItem(packageData))
            {
                LoginDialog.IsOpen = true;

                //No idea why this works, but I can't get it to focus any other way...
                //I think it just needs to be invoked out of the event handler
                await Task.Delay(1);

                NewTrackingNumber.Focus(FocusState.Programmatic);
            }
            else
            {
                var json = JsonConvert.SerializeObject(packageData);
                this.Frame.Navigate(typeof(PackageDetail),json);
            }
        }

        private void ButtonAddPackage_OnClick(object sender, RoutedEventArgs e)
        {
            LoginDialog.IsOpen = true;
        }

        private async void TrackingNumberEntered(object sender, RoutedEventArgs e)
        {
            LoginDialog.IsOpen = false;

            var trackingNumber = NewTrackingNumber.Text.Trim();

            var package = new PackageData {TrackingNumber = trackingNumber};
            //Insert the package before the "add" item
            Packages.Insert(Packages.Count-1, package);

            //Remember the tracking number for app restarts
            ApplicationData.Current.RoamingSettings.Values[TrackingNumbersRoamingKey] =
                Packages.Where(x => !AddNewPackageTemplateSelector.IsAddItem(x))
                    .Select(x => x.TrackingNumber).ToArray();

            await RefreshPackage(package);
        }

        private void TrackingNumberEntryCanceled(object sender, RoutedEventArgs e)
        {
            LoginDialog.IsOpen = false;
        }

        private async void RefreshAllClick(object sender, RoutedEventArgs e)
        {
            var tasks = new List<Task>();
            foreach (var package in Packages)
            {
                tasks.Add(RefreshPackage(package));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        private void AddPackageButtonClick(object sender, RoutedEventArgs e)
        {
            LoginDialog.IsOpen = true;
        }
    }
}
