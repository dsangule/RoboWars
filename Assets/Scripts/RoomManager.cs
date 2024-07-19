using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class RoomManager : MonoBehaviourPunCallbacks {
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform[] spawnPoints;
    [Space]
    public GameObject roomCam;
    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    public string roomNameToJoin = "test";

    private string nickname = "unnamed";

    void Awake() {
        instance = this;
    }

    public void ChangeNickname(string name) {
        nickname = name;
    }

    public void JoinRoomButtonPressed() {
        Debug.Log("Connecting...");
        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);
        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
    
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("We're connected and in a room");
        roomCam.SetActive(false);
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Debug.Log("SpawnPlayer");
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
    }

    public void SetHashes() {
        try {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        } catch {
            // Nothing
        }
    }
}