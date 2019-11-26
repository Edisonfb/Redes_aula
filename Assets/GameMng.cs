using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _NetID, Player _player)
    {
        string _playerID = "Player " + _NetID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }
}
