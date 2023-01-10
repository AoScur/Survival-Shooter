using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawner : MonoBehaviour
{
    public Zom[] zomPrefabs;
    public Transform[] spawnPoints;
    
    private List<Zom> zoms = new List<Zom>();

    void Update()
    {
        //Invoke("CreateZom", 5f);
    }

    private void CreateZom()
    {
        var zomData = zomPrefabs[Random.Range(0, zomPrefabs.Length)];
        var spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Zom zom = Instantiate(zomData, spawnPoint.position, spawnPoint.rotation);
        zoms.Add(zom);
    }
}
