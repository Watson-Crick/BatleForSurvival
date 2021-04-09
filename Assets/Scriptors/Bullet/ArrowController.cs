using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : BulletControllerBase {

    private Transform m_Pivot;

    public override void FindAndInit()
    {
        m_Pivot = gameObject.transform.Find("Pivot");
    }

    public override void Shoot(Vector3 dir, int force, int damage, RaycastHit hit)
    {
        M_Rigidbody.AddForce(dir * force);
        Demage = damage;
        Hit = hit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }

    public override void CollisionEnter(Collision coll)
    {
        M_Rigidbody.Sleep();
        if (coll.collider.gameObject.layer == LayerMask.NameToLayer("Env"))
        {
            Destroy(M_Rigidbody);
            Destroy(gameObject.GetComponent<BoxCollider>());
            PlayAudio(coll);
            gameObject.transform.SetParent(coll.transform);
            StartCoroutine(TailAnimation(m_Pivot));
        }
        else if (coll.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            Destroy(M_Rigidbody);
            Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.SetParent(coll.transform);
            StartCoroutine(TailAnimation(m_Pivot));
            if (coll.collider.transform.parent.name == "Head")
            {
                coll.collider.GetComponentInParent<AIController>().HitHard( 2 *Demage);
            }else
            {
                coll.collider.GetComponentInParent<AIController>().HitNormal(Demage);
            }
            coll.collider.GetComponentInParent<AIController>().PlayEffect(Hit);
        }
    }

    private void PlayAudio(Collision coll)
    {
        switch (coll.gameObject.GetComponent<BulletMark>().MaterialType)
        {
            case MaterialType.Wood:
                AudioManager.Instance.PlayAudioClipByEnum(ClipName.BulletImpactWood, gameObject.transform.position);
                break;
            case MaterialType.Stone:
                AudioManager.Instance.PlayAudioClipByEnum(ClipName.BulletImpactStone, gameObject.transform.position);
                break;
            case MaterialType.Flesh:
                AudioManager.Instance.PlayAudioClipByEnum(ClipName.BulletImpactFlesh, gameObject.transform.position);
                break;
            case MaterialType.Metal:
                AudioManager.Instance.PlayAudioClipByEnum(ClipName.BulletImpactMetal, gameObject.transform.position);
                break;
        }
    }
}
