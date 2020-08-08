using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float speed = 0.1f;
	public float turnSpeed = 0.15f;
	public float dashDistance = 4;
	public float attackDistance = 2;
	public float deadZone = 0.25f;
	public float holding;

	[Space(10)]

	public float countDownStart = 2;
	private float originalCountDown;
	public int combo;
	public bool startCombo;
	public GameObject attacking;

	[Space(10)]

	public Rigidbody rb;

	public GameObject skin;
	public GameObject dropShadowObject;

	public Vector3 moveInput;
	public Vector3 moveWithSpeed;
	public Vector3 lastDirection;
	public Vector3 preciseInput;
	public Vector3 dashVelocity;

	public bool isDashing;
	public bool cannotAttack;
	public bool cannoMove;
	public bool isAtackingWithMouse;
	public bool coolDownFinish;

    public bool redTeam;
    float XAsix;
    float YAsix;
    bool Attackk;
    bool Dash;
    [Space(30)]
    public Image cerleMask;

	// DO TILT ACELERATION
	// CANCEL COMBO WHEN DASHING

	void Start(){

		originalCountDown = countDownStart;
		//attacking.SetActive (false);
        SwitchTeams();
        coolDownFinish = true;
        ChangeColor();
	}
    public void SwitchTeams()
    {
        if(redTeam)
        {
            XAsix = Input.GetAxis ("Horizontal");
            YAsix = -Input.GetAxis ("Vertical");
            Attackk = Input.GetButtonDown ("Y");
            Dash = Input.GetButtonDown ("X");
        }
        else
        {
            XAsix = Input.GetAxis ("Mouse X");
            YAsix = -Input.GetAxis ("Mouse Y");
            Attackk = Input.GetButtonDown ("Right Bumber");
            Dash = Input.GetButtonDown ("X Button");
        }
    }
	void Update () 
	{
		Mouvement ();
		DashTwo ();
		TimerCoolDown ();
        ShootLaser();
        SwitchTeams();
	}
    void ChangeColor()
    {
        if(redTeam)
        {
            ColorSwitch(Color.red);
        }
        else
        {
            ColorSwitch(Color.blue);
        }
    }

    void ColorSwitch(Color col)
    {
        Renderer rend = skin.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("HDRP/Lit");
        rend.material.SetColor("_BaseColor", col);
    }
	void FixedUpdate()
	{
		if (!cannoMove) 
		{
			rb.velocity = Vector3.ClampMagnitude (moveWithSpeed, speed);
		}
	}
	void Mouvement()
	{
		moveInput = new Vector3 (XAsix * 100000, 0, YAsix * 100000);
		preciseInput = new Vector3 (XAsix, 0, YAsix);
		if (preciseInput.magnitude < deadZone) 
		{
			// DEAD ZONE if the player have a really bad controller like me 
			moveInput = Vector3.zero;
		}
			//to not get Hight value because of the *
			moveInput = Vector3.ClampMagnitude (moveInput, 1);
			moveWithSpeed = moveInput * speed;

		if (!cannoMove) 
		{
			if (moveInput != Vector3.zero) 
			{
				// you can also dash with that if you find the last position
				dropShadowObject.transform.rotation = Quaternion.LookRotation (moveInput);
			}
			// follow drop shadow
			skin.transform.rotation = Quaternion.Slerp (skin.transform.rotation, dropShadowObject.transform.rotation, turnSpeed * Time.deltaTime);
			//skin.transform.rotation = Quaternion.LookRotation (lastDirection) * new Quaternion (skin.transform.rotation.y,skin.transform.rotation.x,skin.transform.rotation.z, 60f * 10f);
			lastDirection = dropShadowObject.transform.rotation * Vector3.forward;
		}
	}
    float fillAmount = 0;
	void TimerCoolDown ()
    {
		if (startCombo == true) 
        {
			countDownStart -= Time.deltaTime;
            coolDownFinish = false;
            cerleMask.fillAmount = countDownStart;
		}
		if (countDownStart <= 0) 
        {
            coolDownFinish = true;
            startCombo = false;
            countDownStart = originalCountDown;
            cerleMask.fillAmount = 2;
		}
	}
	void Push(float distance, Vector3 direction){

		dashVelocity = Vector3.Scale (direction, distance * new Vector3 ((Mathf.Log (1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log (1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
		rb.AddForce (dashVelocity, ForceMode.VelocityChange);
	}
	void lookDirection(int controleNumber2){

		if (moveInput.magnitude > 0 && controleNumber2 == 0) {
			skin.transform.rotation = Quaternion.LookRotation (moveInput);
			dropShadowObject.transform.rotation = skin.transform.rotation;
		}

	}
	void DashTwo(){

		if (Dash || Input.GetKeyDown(KeyCode.Space)) {

			if (isDashing == false) {
				skin.transform.rotation = Quaternion.LookRotation (lastDirection);
				StartCoroutine(Dash2());
				Push(dashDistance,lastDirection);
			}
		}
	}
    void ShootLaser()
    {
        if(Attackk)
        {
            startCombo = true;
            if(coolDownFinish)
                transform.GetComponent<Trow>().InstatiateTheBullet();
        }
    }
	IEnumerator Dash2(){

		cannoMove = true;
		cannotAttack = true;
		isDashing = true;
		yield return new WaitForSeconds (0.2f);
		cannotAttack = false;
		cannoMove = false;
		isDashing = false;

	}
}
