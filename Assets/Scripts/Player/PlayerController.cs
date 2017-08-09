using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using System.Linq;
using Level;
using UnityEngine.UI;

public class PlayerController : Entity, ILiving
{

	public float Speed = 10;
	public Animator[] animators;
	private IsoTransform isoTransform;

	[SerializeField]
	private IsoTransform crosshair;

	private Rigidbody rb;

	[SerializeField]
	private float MaxHp = 100;

	[SerializeField]
	private Slider hpBar;

	private float hp = 100;

	void Awake ()
	{
		hpBar.maxValue = MaxHp;
		isoTransform = this.GetOrAddComponent<IsoTransform> (); //avoids polling the IsoTransform component per frame
		animators = this.transform.GetChild (0).GetComponentsInChildren<Animator> ();
		rb = GetComponent<Rigidbody> ();
		//lvl = GameObject.Find ("Level").GetComponent<LevelGeneration> ();
	}

	void Start(){
        //pathfinder = new EnemyAStar();
       // pathfinder.LevelGenerator = lvl;
	}

	void Update ()
	{
		foreach (Animator animator in animators) {
			if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
				animator.SetFloat ("Speed", 1);
			} else {
				animator.SetFloat ("Speed", 0);
			}
			if (Input.GetAxis ("Fire1") != 0) {
				animator.SetBool ("Attack", true);
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
		//rb.AddForce (moveDirection * Speed);
		//rb.MovePosition (transform.position + moveDirection * Time.deltaTime * Speed);
		isoTransform.Position = Isometric.ScreenToIso (transform.position);

		//isoTransform.Translate(moveDirection * Time.deltaTime * Speed);
		//	rb.velocity = Isometric.IsoToScreen(direction*Speed);
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

	public void StopAttack ()
	{
		foreach (Animator animator in animators) {
			animator.SetBool ("Attack", false);
		}
	}

	#region ILiving implementation

	public void TakeDamage (float damageAmount)
	{
		if (hp > 0) {
			hp -= damageAmount;
		}

		hpBar.value = hp;

		if (hp <= 0) {
			Die ();
		}

		Debug.Log (hp);
	}

	public void Die ()
	{
		Debug.Log ("Player is dead!");
	}

	public float Hp {
		get {
			return hp;
		}
	}

	#endregion
}
