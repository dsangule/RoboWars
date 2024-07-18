using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerHealth : MonoBehaviour  {
    public int health;
    public bool isLocalPlayer;
    
    public RectTransform healthBar;
    private float originalHealthBarSize;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start() {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }

    [PunRPC]
    public void TakeDamage(int _damage) {
        health -= _damage;
        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);
        healthText.text = health.ToString();
        if (health <= 0) {
            if (isLocalPlayer) {
                RoomManager.instance.SpawnPlayer();
                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }
            Destroy(gameObject);
        }
    }
}