using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public bool isAttacking;
	public bool isDashing;
	public bool cannotAttack;
	public bool cannoMove;
	public bool isAtackingWithMouse;
	public bool canDash;

	// DO TILT ACELERATION
	// CANCEL COMBO WHEN DASHING

	void Start(){

		originalCountDown = countDownStart;
		//attacking.SetActive (false);

	}

	void Update () 
	{
		Mouvement ();
		DashTwo ();
		Timer ();
        ShootLaser();
        Cstick();
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
		moveInput = new Vector3 (Input.GetAxis ("Horizontal") * 100000, 0, Input.GetAxis ("Vertical") * 100000);
		preciseInput = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
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

    void Cstick()
    {
        Vector3 cStickVec = new Vector3 (Input.GetAxis ("StickX"), 0, Input.GetAxis ("StickY"));
        Debug.Log(cStickVec);
    }
	void Timer ()
    {

		if (startCombo == true) {

			countDownStart -= Time.deltaTime;
			cannoMove = true;
			isAttacking = true;
			attacking.SetActive (true);

		}

		if (countDownStart <= 0) 
        {

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

		if (Input.GetButtonDown ("X") || Input.GetKeyDown(KeyCode.Space)) {

			if (isDashing == false) {
				//DASH
				skin.transform.rotation = Quaternion.LookRotation (lastDirection);
				StartCoroutine(Dash2());
				Push(dashDistance,lastDirection);
			}

		}
	}
    void ShootLaser()
    {
        if(Input.GetButtonDown("Y"))
        {
            Debug.Log("ShootLaser");
        }
    }
	// REDO THIS WITH TIMMER
	IEnumerator Trow(){

		cannoMove = true;
		moveWithSpeed = Vector3.zero;
		yield return new WaitForSeconds (0.1f);
		cannoMove = false;

	}
	IEnumerator Dash2(){

		cannoMove = true;
		cannotAttack = true;
		isDashing = true;
		yield return new WaitForSeconds (0.3f);
		cannotAttack = false;
		cannoMove = false;
		isDashing = false;

	}
	IEnumerator AttackWait(){

		isAttacking = true;
		cannoMove = true;
		yield return new WaitForSeconds (0.3f);
		isAttacking = false;
		cannoMove = false;

	}
}
