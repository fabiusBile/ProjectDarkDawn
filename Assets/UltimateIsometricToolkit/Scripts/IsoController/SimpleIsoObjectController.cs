using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

namespace UltimateIsometricToolkit.controller { 
/// <summary>
/// Simple continuous movement with WSAD/Arrow Keys movement.
/// Note: This is an exemplary implementation. You may vary inputs, speeds, etc.
/// </summary>

	public class SimpleIsoObjectController : MonoBehaviour {

		public float Speed = 10;
		Rigidbody rb;
		private IsoTransform _isoTransform;
		
		void Awake() {
			_isoTransform = this.GetOrAddComponent<IsoTransform>(); //avoids polling the IsoTransform component per frame
			rb = this.GetComponent<Rigidbody>();
		}

		void Update() {
			
			//translate on isotransform
			//Vector3 direction = new Vector3 (Input.GetAxis("Vertical")+Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
			//_isoTransform.Translate(Isometric.ScreenToIso(direction) * Time.deltaTime * Speed);
			Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			dir = Isometric.IsoToScreen (dir);
			rb.velocity = dir*Speed;
		}
	}
}
