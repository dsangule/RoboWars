using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponSwitcher : MonoBehaviour {
    public PhotonView playerSetupView;
    public Animation _animation;
    public AnimationClip draw;
    private int selectedWeapon = 0;
    void Start() {
        SelectWeapon();
    }

    void Update() {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedWeapon = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            selectedWeapon = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            selectedWeapon = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            selectedWeapon = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            selectedWeapon = 8;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (selectedWeapon >= transform.childCount - 1) {
                selectedWeapon = 0;
            } else {
                selectedWeapon += 1;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (selectedWeapon <= 0) {
                selectedWeapon = transform.childCount - 1;
            } else {
                selectedWeapon -= 1;
            }
        }

        if (previousSelectedWeapon != selectedWeapon) {
            SelectWeapon();
        }
    }

    void SelectWeapon() {
        if (selectedWeapon >= transform.childCount) {
            selectedWeapon = transform.childCount - 1;
        }

        playerSetupView.RPC("SetTPWeapon", RpcTarget.All, selectedWeapon);
        // _animation.Stop();
        _animation.Play(draw.name);
        int i = 0;
        foreach (Transform _weapon in transform) {
            if (i == selectedWeapon) {
                _weapon.gameObject.SetActive(true);
            } else {
                _weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
