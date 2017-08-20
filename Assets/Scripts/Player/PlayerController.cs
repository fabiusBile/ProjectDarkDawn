using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using System.Linq;
using Level;
using UnityEngine.UI;

public class PlayerController : LivingEntity
{

	public float Speed = 10;
	public Animator[] animators;
	private IsoTransform isoTransform;

	[SerializeField]
    private IsoTransform crosshair = null;

	private Rigidbody rb;

	//public Vector3 gpos = new Vector3 (0, -0.9f, 0);
	[SerializeField]
	private Weapon weapon;

	void Awake ()
	{
		hpBar.maxValue = MaxHp;
		isoTransform = this.GetOrAddComponent<IsoTransform> (); //avoids polling the IsoTransform component per frame
		animators = this.transform.GetChild (0).GetComponentsInChildren<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		//transform.GetChild (0).transform.localPosition =  Isometric.IsoToScreen(gpos);

		if (Input.GetAxis ("Fire1") != 0 ) {
			if (weapon.CanAttack) {
				weapon.StartAttack (crosshair.Position);
			} else {
				Debug.Log ("I cant attack right now!");
			}
		}

		foreach (Animator animator in animators) {
			if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
				animator.SetFloat ("Speed", 1);
			} else {
				animator.SetFloat ("Speed", 0);
			}

		}
	}

	void OnTriggerStay (Object collision)
	{
		
//			//if (collision.GetType() == System.Type.GetType("Collider")) {
//			Debug.Log (collision.name);
//			if (Input.GetAxis ("Fire1") != 0) {
//				IsoTransform transform = ((Collider)collision).GetComponent<IsoTransform> ();
//				curPos = new Point (transform.Position);
//				Point goal = new Point (crosshair.Position);
//                IEnumerable<Tile> path = pathfinder.Search(curPos, goal, lvl.TileMap);
//            Tile[] p = path.ToArray();
//			Transform prev = null;
//			}

	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector3 moveDirection = new Vector3 (Input.GetAxis ("Horizontal") + Input.GetAxis ("Vertical"), 0, -Input.GetAxis ("Horizontal") + Input.GetAxis ("Vertical"));
		moveDirection.Normalize ();

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layer = LayerMask.NameToLayer ("floor");
		if (Physics.Raycast (ray, out hit,10000,1 << layer)) {
			crosshair.Position = Isometric.ScreenToIso (hit.point);
		}
		crosshair.Position.Set (crosshair.Position.x, isoTransform.Position.y, crosshair.Position.z);
		Vector3 lookDirection = crosshair.Position - isoTransform.Position;
		lookDirection.Normalize ();
		moveDirection = Isometric.IsoToScreen (moveDirection);
		rb.velocity = moveDirection * Speed;
		isoTransform.Position = Isometric.ScreenToIso (transform.position);


		foreach (Animator animator in animators) {
			if (Mathf.Round (lookDirection.x * 10f) / 10f >= 0.5 && lookDirection.z >= 0.5) {
				animator.SetFloat ("Direction", 0);
			}
			if (lookDirection.x >= 0.5 && Mathf.Round (lookDirection.z * 10f) / 10f <= 0.5) {
				animator.SetFloat ("Direction", 1);
			}
			if (Mathf.Round (lookDirection.x * 10f) / 10f <= -0.5 && lookDirection.z <= -0.5) {
				animator.SetFloat ("Direction", 2);
			}
			if (lookDirection.x <= 0.5 && Mathf.Round (lookDirection.z * 10f) / 10f >= 0.5) {
				animator.SetFloat ("Direction", 3);
			}

		}
	}

//	public void StopAttack ()
//	{
//		weapon.EndAttack ();
//		foreach (Animator animator in animators) {
//			animator.SetBool ("Attack", false);
//		}
//	}		

	override public void Die ()
	{
		Debug.Log ("Player is dead!");
	}


}
