using MySql.Data.MySqlClient;
using System;
using UnityEngine;

public class DBService : MonoService
{
    [SerializeField] DBMainData mainData;


    private void Start()
    {
        GetConnection();
    }

    public void GetConnection()
    {
        MySqlConnection conn = new MySqlConnection(mainData.ConnectionString);
        try
        {
            this.LogError("Connecting to MySQL...");

            conn.Open();

            string sql = "SELECT 1+1";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader()
;

            while (rdr.Read())
            {
                this.LogError(rdr[0].ToString());
            }
            rdr.Close();

        }
        catch (Exception ex)
        {
            this.LogError(ex.ToString());

        }
        finally
        {
            conn.Close();

        }

        this.LogError("Done.");
    }

}
