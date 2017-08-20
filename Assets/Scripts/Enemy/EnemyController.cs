using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

using UnityEngine;
using UnityEngine.UI;
public class EnemyController : LivingEntity
{

    IsoTransform target;
    Moveable moveable;

    [SerializeField]
    Transform hpBarPivot = null;


    [SerializeField]
    Transform hpBarTransform;

    Entity targetEntity;

    [SerializeField]
    Weapon weapon = null;

    Animator[] animators;
    IsoTransform isoTransform;
    override public void Start()
    {
        moveable = GetComponent<Moveable>();
        isoTransform = GetComponent<IsoTransform>();
        animators = this.transform.GetChild(0).GetComponentsInChildren<Animator>();
        hpBarTransform = Instantiate(hpBarTransform) as Transform;
        hpBarTransform.parent = GameObject.Find("Canvas").transform;
        hpBar = hpBarTransform.GetComponent<Slider>();
        base.Start();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            target = col.GetComponent<IsoTransform>();
            targetEntity = col.GetComponent<Entity>();
        }
    }


    // Update is called once per frame
    void Update()
    {

        hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPivot.position);

        if (target != null)
        {
            if (Vector3.Distance(isoTransform.Position, target.Position) <= weapon.Range)
            {
                if (weapon.CanAttack)
                {
                    weapon.StartAttack(target.Position);
                }
                moveable.Stop();
                this.GetComponent<Rigidbody>().Sleep();
            }
            else
            {
                moveable.Move(target.Position);
                this.GetComponent<Rigidbody>().WakeUp();
            }
        }
    }

    public override void Die()
    {
        Debug.Log("Enemy is dead!");
        Object.Destroy(hpBar.gameObject);
        Object.Destroy(gameObject);
    }
}
