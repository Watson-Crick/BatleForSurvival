using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletController : BulletControllerBase {

    private Ray ray;
    private RaycastHit selfHit;

    public override void FindAndInit()
    {
        Invoke("DestroySelf", 5);
    }
	
	public override void Shoot(Vector3 dir, int force, int demage, RaycastHit hit)
    {
        M_Rigidbody.AddForce(dir * force);
        ray = new Ray(M_Transform.position, dir);
        Demage = demage;
        Hit = hit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }

    public override void CollisionEnter(Collision coll)
    {
        if (coll.collider.GetComponent<BulletMark>() != null)
        {
            Physics.Raycast(ray, out selfHit, 1000, 1 << 11);
            coll.gameObject.GetComponent<BulletMark>().CreateBulletMark(selfHit);
        }
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            Physics.Raycast(ray, out selfHit, 1000, 1 << 12);
            if (coll.collider.transform.parent.name == "Head")
            {
                coll.collider.GetComponentInParent<AIController>().HitHard(2 * Demage);
            }
            else
            {
                coll.collider.GetComponentInParent<AIController>().HitNormal(Demage);
            }
            coll.collider.GetComponentInParent<AIController>().PlayEffect(selfHit);
        }
        Destroy(gameObject);
    }
}
