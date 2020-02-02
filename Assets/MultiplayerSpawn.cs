using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSpawn : MonoBehaviour
{
    public static MultiplayerSpawn Instance;

    public Transform Player1Position;
    public Transform Player2Position;
    public int amtOfPlayers = 0;

    private void Start() {
        Instance = this;
    }

    public void PlayerAdded() {
        amtOfPlayers++;
    }
}
