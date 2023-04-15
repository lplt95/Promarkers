using Promarkers.Database;
using Promarkers.Models;
using Promarkers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promarkers;

public class PromarkerService
{
    private readonly PromarkerDatabase database;
    private readonly ImportFromFile importFromFile;

    public PromarkerService()
    {
        database = new PromarkerDatabase();
        importFromFile = new ImportFromFile();
    }

    public async Task<PromarkerModel> GetMarkerByColorName(string colorName, int markerType)
    {
        if (string.IsNullOrWhiteSpace(colorName))
        {
            return null;
        }

        var result = await database.GetByColorName(colorName, markerType);
        if (result == null)
        {
            return null;
        }
        return PromarkerModel.FromDto(result);
    }

    public async Task<PromarkerModel> GetMarkerByColorCode(string colorCode, int markerType)
    {
        if (string.IsNullOrWhiteSpace (colorCode))
        {
            return null;
        }

        var result = await database.GetByColorCode(colorCode, markerType);
        if (result == null)
        {
            return null;
        }
        return PromarkerModel.FromDto(result);
    }

    public async Task<List<PromarkerModel>> GetAllMarkers()
    {
        var result = await database.GetAllAsync();
        return result.Select(PromarkerModel.FromDto).ToList();
    }

    public async Task<List<PromarkerModel>> GetMarkers(MarkerTypeEnum markerType, int colorFamily)
    {
        if (colorFamily == 0)
        {
            var result = await database.GetAllByType((int)markerType);
            return result.Select(PromarkerModel.FromDto).ToList();
        }
        else
        {
            var result = await database.GetAllByTypeAndColorFamily((int)markerType, colorFamily);
            return result.Select(PromarkerModel.FromDto).ToList();
        }
    }

    public async Task<PromarkerModel> ChangeStatus(PromarkerModel model)
    {
        model.ChangeStatus();
        await database.SaveAsync(model.ToDto());
        var result = await database.GetById(model.Id);
        return PromarkerModel.FromDto(result);
    }

    public async Task<string> ImportDataFromFile(bool updateExisting)
    {
        var importedList = (await importFromFile.Import()).Promarkers;
        int importedCount = importedList.Count;
        int savedCount = 0;
        int updatedCount = 0;
        int existingCount = 0;
        foreach(var promarker in importedList)
        {
            var existing = await database.GetByColorCode(promarker.ColorCode, promarker.MarkerType);
            if (existing == null)
            {
                await database.SaveAsync(promarker);
                savedCount++;
            }
            else if (updateExisting)
            {
                promarker.Id = existing.Id;
                promarker.IsOwned = existing.IsOwned;
                await database.SaveAsync(promarker);
                updatedCount++;
            }
            else
            {
                existingCount++;
            }
        }
        string message = updateExisting ? string.Format("Zaaktualizowane markery: {0}", updatedCount) : string.Format("Istniejące markery: {0}", existingCount);

        return string.Format("Zakończono importowanie. \nŁączna ilość markerów w pliku: {0} \nNowe markery: {1} \n{2}", importedCount, savedCount, message);

    }
}
