using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDesable;

    void Start()
    {
        if(!isLocalPlayer)
        {
           for(int i = 0; i< componentsToDesable.Length; i++)
            {
                componentsToDesable[i].enabled = false;
            }
            gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
        }
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameMng.RegisterPlayer(_netId, _player);
    }
}
