using SQLite;
using System.Xml.Serialization;

namespace Promarkers.Database;

public class Promarker
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [XmlElement("ColorName")]
    public string ColorName { get; set; }

    [XmlElement("ColorCode")]
    public string ColorCode { get; set; }

    [XmlElement("ColorFamily")]
    public int ColorFamily { get; set; }

    [XmlElement("MarkerType")]
    public int MarkerType { get; set; }

    [XmlElement("IsOwned")]
    public bool IsOwned { get; set; } = false;

    [XmlElement("UniqueTo")]
    public int UniqueTo { get; set; }

    [XmlElement("Hex")]
    public string Hex { get; set; }
}

public class PromarkerList
{
    [XmlElement("promarker")]
    public List<Promarker> Promarkers { get; set; }
}
