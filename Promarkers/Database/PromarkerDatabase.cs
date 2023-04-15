using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promarkers.Database;

public class PromarkerDatabase
{
    SQLiteAsyncConnection connection;

    async Task Init()
    {
        if (connection is not null)
            return;


        connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await connection.CreateTableAsync<Promarker>();
        
    }

    public async Task<List<Promarker>> GetAllAsync()
    {
        await Init();
        return await connection.Table<Promarker>().ToListAsync();
    }

    public async Task<List<Promarker>> GetAllByType(int markerType)
    {
        await Init();
        return await connection.Table<Promarker>().Where(p => p.MarkerType == markerType).ToListAsync();
    }

    public async Task<List<Promarker>> GetAllByTypeAndColorFamily(int markerType, int colorFamily)
    {
        await Init();
        return await connection.Table<Promarker>().Where(p => p.MarkerType == markerType && p.ColorFamily == colorFamily).ToListAsync();
    }

    public async Task<Promarker> GetByColorCode(string colorCode, int markerType)
    {
        await Init();
        return await connection.Table<Promarker>().Where(p => p.ColorCode == colorCode && p.MarkerType == markerType).FirstOrDefaultAsync();
    }

    public async Task<Promarker> GetByColorName(string colorName, int markerType)
    {
        await Init();
        return await connection.Table<Promarker>().Where(p => p.ColorName == colorName && p.MarkerType == markerType).FirstOrDefaultAsync();
    }

    public async Task<int> SaveAsync(Promarker promarker)
    {
        await Init();
        return promarker.Id == 0 ? await connection.InsertAsync(promarker) : await connection.UpdateAsync(promarker);
    }

    public async Task<int> DeleteAsync(Promarker promarker)
    {
        await Init();
        return await connection.DeleteAsync(promarker);
    }

    public async Task<Promarker> GetById(int id)
    {
        await Init();
        return await connection.Table<Promarker>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }
}
