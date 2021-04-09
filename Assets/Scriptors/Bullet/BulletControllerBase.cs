using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletControllerBase : MonoBehaviour {

    private Transform m_Transform;
    private Rigidbody m_Rigidbody;
    private RaycastHit hit;

    public Transform M_Transform { get { return m_Transform; } }
    public Rigidbody M_Rigidbody { get { return m_Rigidbody; } }
    public RaycastHit Hit { set { hit = value; } get { return hit; } }

    private int demage;

    public int Demage { set { demage = value; } get { return demage; } }

    void Awake()
    {
        m_Transform = gameObject.transform;
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        FindAndInit();
    }

    public abstract void FindAndInit();
    public abstract void Shoot(Vector3 dir, int force, int damage, RaycastHit hit);
    public abstract void CollisionEnter(Collision coll);

    /// <summary>
    /// 尾部颤动
    /// </summary>
    /// <returns></returns>
    public IEnumerator TailAnimation(Transform pivot)
    {
        float stopTime = Time.time + 1;
        float vel = 0;
        float range = 1;

        while (Time.time < stopTime)
        {
            pivot.localRotation = Quaternion.Euler(new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0));

            range = Mathf.SmoothDamp(range, 0, ref vel, stopTime - Time.time);

            yield return null;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
