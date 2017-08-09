using UnityEngine;
using System.Collections;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

public class Moveable : MonoBehaviour {
	public float Speed = 1f;
	Vector3 target;
	bool move;
	Animator[] animators;
	IsoTransform isoTransform;
	Rigidbody rb;

	public void Start(){
		animators = transform.GetChild (0).GetComponentsInChildren<Animator> ();
		isoTransform = GetComponent<IsoTransform> ();
		rb = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 target){
		this.target = target;
		Vector3 direction = Vector3.ClampMagnitude (new Vector3 (target.x - isoTransform.Position.x, target.y - isoTransform.Position.y, target.z - isoTransform.Position.z),1f);
		Debug.Log (direction);

		foreach (Animator animator in animators) {
			Debug.Log (Mathf.Round (direction.x * 10f) / 10f);
			if (Mathf.Round(direction.x*10f)/10f>=0.5 && direction.z >=0.5) {
				animator.SetFloat ("Direction", 0);
			}
			if (direction.x >= 0.5 && Mathf.Round(direction.z*10f)/10f<=0.5) {
				animator.SetFloat ("Direction", 1);
			}
			if (Mathf.Round(direction.x*10f)/10f<=-0.5 && direction.z <=-0.5) {
				animator.SetFloat ("Direction", 2);
			}
			if (direction.x <= 0.5 && Mathf.Round(direction.z*10f)/10f>=0.5) {
				animator.SetFloat ("Direction", 3);
			}
			animator.SetFloat ("Speed", 1);

		}
		move = true;

	}
	public void FixedUpdate(){
		if (move) {
			Vector3 direction = target - isoTransform.Position;
			//isoTransform.Translate (direction * speed * Time.deltaTime);
			direction.Normalize();
			direction = Isometric.IsoToScreen(direction);
			rb.velocity = direction * Speed;
			isoTransform.Position = Isometric.ScreenToIso (transform.position);

			if (Vector3.Distance (isoTransform.Position, target) < 0.1) {
				move = false;
				foreach (Animator animator in animators) {
					animator.SetFloat ("Speed", 0);
				}
			}
		}
	}
}
