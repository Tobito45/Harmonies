using System;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

public static class NetworkTools 
{
    public static void FindNetworkObjectAndMakeAction(ulong id, Action<NetworkObject> action)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out var networkObject))
            action(networkObject);
        else
            Debug.LogError("ERROR");
    }
}
