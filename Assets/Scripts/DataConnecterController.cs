using Harmonies.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataConnecterController
{
    private ushort _port = 7777;
    private UserType _userType = UserType.None;
    private string _ip;
    private static DataConnecterController _instance;
    public static DataConnecterController Singlton
    {
        get
        {
            if (_instance == null)
                _instance = new DataConnecterController();
            return _instance;
        }
    }

    public void StartAsHost() => _userType = UserType.Host;
    public void StartAsClient(string ip)
    {
        _userType = UserType.Client;
        _ip = ip;
    }

    public (UserType userType, string ip, ushort port) GetData => (_userType, _ip, _port);
}