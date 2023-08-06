using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureEntityData : EntityData
{
    public ClientStats ClientStats;
    public CreatureStats CreatureStats;
    
    public CreatureEntityData() 
    {
        
    }

    public void SetClientStats(ClientStats stats)
    {
        Debug.Log($"From CreatueEntData: {stats.ClientId} {stats.Username} {stats.ConnectionAddress}");
        ClientStats = new ClientStats(stats);
    }

    public void SetCreatureStats(CreatureStats stats)
    {
        CreatureStats = new CreatureStats(stats);
    }
}

[System.Serializable]
public class CreatureStats
{
    public string Name;
    public float MovementSpeed;

    public CreatureStats()
    {

    }

    public CreatureStats(CreatureStats stats)
    {
        this.Name = stats.Name;
        this.MovementSpeed = stats.MovementSpeed;
    }
    public CreatureStats(string name, float movementSpeed)
    {
        this.Name = name;
        this.MovementSpeed = movementSpeed;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        this.MovementSpeed = movementSpeed;
    }
}

[System.Serializable]
public class ClientStats
{
    public int ClientId;
    public string Username;
    public string ConnectionAddress;

    public ClientStats()
    {

    }

    public ClientStats(ClientStats stats)
    {
        this.ClientId = stats.ClientId;
        this.Username = stats.Username;
        this.ConnectionAddress = stats.ConnectionAddress;
    }
    public ClientStats(int id, string username, string connectionAddress)
    {
        this.ClientId = id;
        this.Username = username;
        this.ConnectionAddress = connectionAddress;
    }
}