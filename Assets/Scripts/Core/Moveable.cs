using UnityEngine;
using System.Collections;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class Moveable : MonoBehaviour {
	public float speed = 1f;
	Vector3 target;
	bool move;
	Animator[] animators;
	IsoTransform isoTransform;

	public void Start(){
		animators = transform.GetChild (0).GetComponentsInChildren<Animator> ();
		isoTransform = GetComponent<IsoTransform> ();
	}

	public void Move(Vector3 target){
		this.target = target;
		Vector3 direction = Vector3.ClampMagnitude (new Vector3 (target.x - isoTransform.Position.x, 0, target.z - isoTransform.Position.z),1f);
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
	public void Update(){
		if (move) {
			Vector3 direction = Vector3.ClampMagnitude (new Vector3 (target.x - isoTransform.Position.x,0,target.z - isoTransform.Position.z),1f);
			isoTransform.Translate (direction * speed * Time.deltaTime);
			if (Vector3.Distance (isoTransform.Position, target) < 0.1) {
				move = false;
				foreach (Animator animator in animators) {
					animator.SetFloat ("Speed", 0);
				}
			}
		}
	}
}
