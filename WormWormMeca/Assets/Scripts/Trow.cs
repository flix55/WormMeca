using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trow : MonoBehaviour
{
    public GameObject redBall;
    public GameObject blueBall;
    public GameObject spawnPointBullet;
    public void InstatiateTheBullet()
    {
        if(transform.GetComponent<Player>().redTeam == true)
        {
            Instantiate(redBall, spawnPointBullet.transform.position + spawnPointBullet.transform.forward, spawnPointBullet.transform.rotation);
        }
        if(transform.GetComponent<Player>().redTeam == false)
        {
            Instantiate(blueBall, spawnPointBullet.transform.position + spawnPointBullet.transform.forward, spawnPointBullet.transform.rotation);
        }
    }
}
