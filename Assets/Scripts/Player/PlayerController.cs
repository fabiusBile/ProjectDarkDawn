using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class PlayerController : MonoBehaviour {

	public float Speed = 10;
	public Animator[] animators;
	private IsoTransform isoTransform;
	public IsoTransform crosshair;
	private Rigidbody rb;
	void Awake() {
		isoTransform = this.GetOrAddComponent<IsoTransform>(); //avoids polling the IsoTransform component per frame
		animators = this.transform.GetChild (0).GetComponentsInChildren<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update(){
		foreach (Animator animator in animators) {
			if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
				animator.SetFloat ("Speed", 1);
			} else {
				animator.SetFloat ("Speed", 0);
			}
			if (Input.GetAxis("Fire1") != 0){
				animator.SetBool("Attack",true);
			}
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log (collision.gameObject.name);
	}
	void OnTriggerEnter(Collision collision)
	{
		Debug.Log (collision.gameObject.name);
	}
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 moveDirection = new Vector3 (Input.GetAxis("Horizontal")+Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")+Input.GetAxis("Vertical"));
		moveDirection.Normalize();

		RaycastHit hit;
		Ray ray =  Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			crosshair.Position = Isometric.ScreenToIso(hit.point);
		}
		crosshair.Position.Set (crosshair.Position.x, isoTransform.Position.y, crosshair.Position.z);
		Vector3 lookDirection = crosshair.Position - isoTransform.Position;
		lookDirection.Normalize ();
		moveDirection = Isometric.IsoToScreen (moveDirection);
		rb.velocity = moveDirection * Speed;
		//rb.AddForce (moveDirection * Speed);
		//rb.MovePosition (transform.position + moveDirection * Time.deltaTime * Speed);
		isoTransform.Position = Isometric.ScreenToIso (transform.position);

		//isoTransform.Translate(moveDirection * Time.deltaTime * Speed);
	//	rb.velocity = Isometric.IsoToScreen(direction*Speed);
		foreach (Animator animator in animators) {
			if (Mathf.Round(lookDirection.x*10f)/10f>=0.5 && lookDirection.z >=0.5) {
				animator.SetFloat ("Direction", 0);
			}
			if (lookDirection.x >= 0.5 && Mathf.Round(lookDirection.z*10f)/10f<=0.5) {
				animator.SetFloat ("Direction", 1);
			}
			if (Mathf.Round(lookDirection.x*10f)/10f<=-0.5 && lookDirection.z <=-0.5) {
				animator.SetFloat ("Direction", 2);
			}
			if (lookDirection.x <= 0.5 && Mathf.Round(lookDirection.z*10f)/10f>=0.5) {
				animator.SetFloat ("Direction", 3);
			}

		}
	}
	public void StopAttack(){
		foreach (Animator animator in animators) {
			animator.SetBool ("Attack", false);
		}
	}
}
