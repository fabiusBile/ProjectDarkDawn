using UnityEngine;
using System.Collections;

public class Moveable : MonoBehaviour {
	public float speed = 1f;
	Vector3 target;
	bool move;
	Animator[] animators;

	public void Start(){
		animators = transform.GetChild (0).GetComponentsInChildren<Animator> ();
	}

	public void Move(Vector3 target){
		this.target = target;
		Vector3 direction = Vector3.ClampMagnitude (new Vector3 (target.x - transform.position.x, target.y - transform.position.y,0),1f);
		Debug.Log (direction);

		foreach (Animator animator in animators) {
			Debug.Log (Mathf.Round (direction.x * 10f) / 10f);
			if (Mathf.Round(direction.x*10f)/10f==0 && direction.y > 0) {
				animator.SetFloat ("Direction", 0);
			}
			if (direction.x > 0 && Mathf.Round(direction.y*10f)/10f==0) {
				animator.SetFloat ("Direction", 1);
			}
			if (Mathf.Round(direction.x*100f)/100f==0 && direction.y < 0) {
				animator.SetFloat ("Direction", 2);
			}
			if (direction.x < 0 && Mathf.Round(direction.y*10f)/10f==0) {
				animator.SetFloat ("Direction", 3);
			}
			animator.SetFloat ("Speed", 1);

		}
		move = true;

	}
	public void Update(){
		if (move) {
			Vector3 direction = Vector3.ClampMagnitude (new Vector3 (target.x - transform.position.x, target.y - transform.position.y,0),1f);
			transform.Translate (direction * speed * Time.deltaTime);
			if (Vector3.Distance (transform.position, target) < 0.1) {
				move = false;
				foreach (Animator animator in animators) {
					animator.SetFloat ("Speed", 0);
				}
			}
		}
	}
}
