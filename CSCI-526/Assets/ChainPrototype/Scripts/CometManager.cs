using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometManager : MonoBehaviour
{

    [SerializeField] private GameObject comet;      // Spawn every so often on sides of arena

    private IEnumerator SpawnComet()        // Comet in charge of scaling itself -- just delay here
    {
        int delay = Random.Range(1, 5);

        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        Instantiate(comet, new Vector2(spawnX, spawnY), Quaternion.identity);

        yield return new WaitForSeconds(delay);

        StartCoroutine(SpawnComet());
    }

    void Awake()
    {
        StartCoroutine(SpawnComet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
