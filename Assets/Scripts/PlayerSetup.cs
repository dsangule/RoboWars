using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour {
    public PlayerMovement movement;
    public GameObject camera;
    public string nickname;
    public TextMeshPro nicknameText;
    public Transform TPWeaponHolder;
    public Transform nameTag;
    public Transform playerEyes;

    public void IsLocalPlayer() {
        TPWeaponHolder.gameObject.SetActive(false);
        nameTag.gameObject.SetActive(false);
        playerEyes.gameObject.SetActive(false);

        movement.enabled = true;
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetTPWeapon(int _weaponIndex) {
        foreach (Transform _weapon in TPWeaponHolder) {
            _weapon.gameObject.SetActive(false);
        }
        TPWeaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name) {
        nickname = _name;
        nicknameText.text = nickname;
    }
}
