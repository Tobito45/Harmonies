using System;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NetworkTools 
{
    public static void FindNetworkObjectAndMakeAction(ulong id, Action<NetworkObject> action)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out var networkObject))
            action(networkObject);
        else
            Debug.LogError("ERROR");
    }

    public static void CloseAndGoToMain()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("mainScene");
    }
}
