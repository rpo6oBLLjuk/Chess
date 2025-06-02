using Cysharp.Threading.Tasks;
using MySqlConnector;
using System;
using System.Data;
using UnityEngine;
using Zenject;

public class AnalyticsController
{
    [Inject] private DBService dbService;
    [Inject] GameAnalyticsData gameAnalyticsData;

    public async UniTask<bool> LoadAnalyticsData()
    {
        var (success, connection) = await dbService.TryGetConnection();
        if (!success)
            return false;

        try
        {
            string query = @"
                SELECT 
                    registration_date,
                    last_login_date,
                    (SELECT wins FROM player_stats WHERE player_id = @playerId) AS wins,
                    (SELECT defeats FROM player_stats WHERE player_id = @playerId) AS defeats
                FROM players
                WHERE player_id = @playerId LIMIT 1";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@playerId", dbService.Data.PlayerId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return false;
                    }

                    await reader.ReadAsync();

                    gameAnalyticsData.registrationDate = reader.GetDateTime("registration_date");
                    gameAnalyticsData.lastLoginDate = reader.IsDBNull("last_login_date")
                        ? DateTime.MinValue
                        : reader.GetDateTime("last_login_date");
                    gameAnalyticsData.winsCount = reader.GetInt32("wins");
                    gameAnalyticsData.defeatsCount = reader.GetInt32("defeats");
                }
            }

            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
        finally
        {
            connection?.Close();
            await UniTask.SwitchToMainThread();
        }
    }

    public async UniTask<bool> UpdateBattleStats(bool isWin)
    {
        var (success, connection) = await dbService.TryGetConnection();
        if (!success)
            return false;

        try
        {
            string fieldToUpdate = isWin ? "wins" : "defeats";
            string query = $"UPDATE player_stats SET {fieldToUpdate} = {fieldToUpdate} + 1 WHERE player_id = @playerId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@playerId", dbService.Data.PlayerId);
                int affected = await cmd.ExecuteNonQueryAsync();
                return affected > 0;
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError($"Stats update failed: {ex.Message}");
            return false;
        }
        finally
        {
            connection?.Close();
            await UniTask.SwitchToMainThread();
        }
    }
}