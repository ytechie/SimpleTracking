using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace SimpleTracking.WindowsStore.DesignerData
{
    public class PackagesDesignerData
    {
        public ObservableCollection<PackageData> Packages { get; private set; }

        public PackagesDesignerData()
        {
            Packages = new ObservableCollection<PackageData>();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            var json = GetSamplePackageJson();

            var package = Newtonsoft.Json.JsonConvert.DeserializeObject<PackageData>(json);
            package.DistanceFromHere = 1234;

            Packages.Add(package);

            Packages.Add(AddNewPackageTemplateSelector.GetAddItem());
        }

        public static string GetSamplePackageJson()
        {
            return GetEmbeddedJsonSample("RealUpsExample");
        }

        public static string GetEmbeddedJsonSample(string fileName)
        {
            var assembly = typeof(PackagesDesignerData).GetTypeInfo().Assembly;
            //SimpleTracking.WindowsStore.DesignerData.SampleTrackingData.json
            var stream =
                assembly.GetManifestResourceStream(typeof(PackagesDesignerData).Namespace + "." + fileName + ".json");
            // var embeddedFiles = assembly.GetManifestResourceNames();
            //var embedded = await Package.Current.InstalledLocation.GetFileAsync("SampleTrackingData.json");
            //var stream = await embedded.OpenReadAsync();
            var sr = new StreamReader(stream);
            var json = sr.ReadToEnd();
            return json;
        }
}
}
