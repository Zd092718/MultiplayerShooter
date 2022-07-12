using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject playerPrefab;
    private GameObject player;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();

        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }

    public void Die()
    {
        PhotonNetwork.Instantiate(deathEffect.name, player.transform.position, Quaternion.identity);

        PhotonNetwork.Destroy(player);

        SpawnPlayer();
    }
}
