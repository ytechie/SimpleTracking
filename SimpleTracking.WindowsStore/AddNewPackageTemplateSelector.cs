using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimpleTracking.WindowsStore
{
    public class AddNewPackageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate AddNewItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var package = item as PackageData;

            if (IsAddItem(package))
                return AddNewItemTemplate;
            
            return DefaultTemplate;
        }

        //Gets a special item that will be a placeholder for the "+" item
        public static PackageData GetAddItem()
        {
            return new PackageData {TrackingNumber = "Add Package"};
        }

        public static bool IsAddItem(PackageData packageData)
        {
            if (packageData == null)
                return false;

            return packageData.TrackingNumber == GetAddItem().TrackingNumber;

        }
    }
}
