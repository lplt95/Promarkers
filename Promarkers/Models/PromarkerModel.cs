using Promarkers.Database;
using Promarkers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Promarkers.Models;

public class PromarkerModel
{
    public int Id { get; }

    public string ColorName { get; set; }

    public string ColorCode { get; }

    public RegularColorFamilyEnum ColorFamily { get; }

    public MarkerTypeEnum MarkerType { get; }

    public bool IsOwned { get; private set; }

    public string Owned { get; private set; }
    
    public int UniqueTo { get; }

    public string Unique { get; }

    public string Hex { get; }

    public Color Color { get; }

    public PromarkerModel(int id, string colorName, string colorCode, RegularColorFamilyEnum colorFamily, MarkerTypeEnum markerType, int uniqueTo, bool isOwned, string hex)
    {
        Id = id;
        ColorName = colorName;
        ColorCode = colorCode;
        ColorFamily = colorFamily;
        MarkerType = markerType;
        IsOwned = isOwned;
        Owned = IsOwned ? "Tak" : "Nie";
        UniqueTo = uniqueTo;
        Unique = uniqueTo > 0 ? ((MarkerTypeEnum)UniqueTo).ToString() : "Nie";
        Hex = hex;
        Color = Color.FromArgb(hex);
    }

    public Promarker ToDto()
    {
        return new Promarker()
        {
            Id = Id,
            ColorName = ColorName,
            ColorCode = ColorCode,
            ColorFamily = (int)ColorFamily,
            MarkerType = (int)MarkerType,
            UniqueTo = UniqueTo,
            IsOwned = IsOwned,
            Hex = Hex
        };
    }

    public static PromarkerModel FromDto(Promarker promarker)
    {
        return new PromarkerModel(
            promarker.Id,
            promarker.ColorName,
            promarker.ColorCode,
            (RegularColorFamilyEnum)promarker.ColorFamily,
            (MarkerTypeEnum)promarker.MarkerType,
            promarker.UniqueTo,
            promarker.IsOwned,
            promarker.Hex);
    }

    public Color GetColor()
    {
        return Color.FromArgb(Hex);
    }

    public void ChangeStatus()
    {
        IsOwned = !IsOwned;
        Owned = IsOwned ? "Tak" : "Nie";
    }
}
