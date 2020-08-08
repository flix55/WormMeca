using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float thrust = 1.0f;
    public Rigidbody rb;
    public bool redTeamBullet;
    public float TimeToDestryItslef = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
        StartCoroutine(DestroyOnTime(TimeToDestryItslef));
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            if(redTeamBullet)
            {
                other.gameObject.GetComponent<Player>().redTeam = true;
                ColorSwitch(Color.red,other.gameObject.GetComponent<Player>());
            }
            else
            {
                other.gameObject.GetComponent<Player>().redTeam = false;
                ColorSwitch(Color.blue,other.gameObject.GetComponent<Player>());
            }
            Destroy(gameObject);
        }
    }
    private void ColorSwitch(Color col,Player player)
    {
        Renderer rend = player.skin.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("HDRP/Lit");
        rend.material.SetColor("_BaseColor", col);
    }

    IEnumerator DestroyOnTime(float WaitForSecond)
    {
        yield return new WaitForSeconds(WaitForSecond);
        Destroy(gameObject);
    }
}
