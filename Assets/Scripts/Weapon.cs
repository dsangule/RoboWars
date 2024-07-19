using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;

public class Weapon : MonoBehaviour {
    public new Camera camera;
    public int damage;
    public float fireRate;

    [Header("VFX")]
    public GameObject hitVFX;

    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;
    public Image ammoCircle;

    [Header("Animation")]
    public new Animation animation;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    [Range(0, 2)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 0.01f;
    public float recoilBack = 0.04f;

    private float nextFire;
    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;
    private float recoilLength;
    private float recoverLength;
    private bool isRecoiling;
    private bool isRecovering;

    void SetAmmo() {
        ammoCircle.fillAmount = (float)ammo / magAmmo;
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Start() {
        SetAmmo();
        originalPosition = transform.localPosition;
        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    void Update() {
        if (nextFire > 0) {
            nextFire -= Time.deltaTime;
        }
        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && animation.isPlaying == false) {
            nextFire = 1 / fireRate;
            ammo--;
            SetAmmo();
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && mag > 0) {
            Reload();
        }
        if (isRecoiling) {
            Recoil();
        }
        if (isRecovering) {
            Recover();
        }
    }

    void Reload() {
        animation.Play(reload.name);
        if (mag > 0) {
            mag--;
            ammo = magAmmo;
        }
        SetAmmo();
    }

    void Fire() {
        isRecoiling = true;
        isRecovering = false;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f)) {
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            if (hit.transform.gameObject.GetComponent<PlayerHealth>()) {
                if (damage >= hit.transform.gameObject.GetComponent<PlayerHealth>().health) {
                    RoomManager.instance.kills++;
                    RoomManager.instance.SetHashes();
                    PhotonNetwork.LocalPlayer.AddScore(100); // Kill Score
                }
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }

    void Recoil() {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);
        if (transform.localPosition == finalPosition) {
            isRecoiling = false;
            isRecovering = true;
        }
    }

    void Recover() {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);
        if (transform.localPosition == finalPosition) {
            isRecoiling = false;
            isRecovering = false;
        }
    }
}