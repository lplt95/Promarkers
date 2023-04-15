using System.Xml;
using System.Xml.Serialization;
using Promarkers.Database;

namespace Promarkers
{
    public class ImportFromFile
    {
        private const string _fileName = "promarkers.xml";
        public static string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

        public async Task<PromarkerList> Import()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "promarkers";
            XmlSerializer serializer = new XmlSerializer(typeof(PromarkerList), xRoot);
            using (Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(_fileName))
            {
                PromarkerList result = (PromarkerList) serializer.Deserialize(fileStream);
                return result;
            }
        }
    }
}
