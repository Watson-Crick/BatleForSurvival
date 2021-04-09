using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvObstacleManager : MonoBehaviour {

    private Transform m_Transform;
    private Transform obstacles_Transform;
    private Transform[] point;
    private List<GameObject> prefabList;

    public Transform M_Transform { set { m_Transform = value; } get { return m_Transform; } }
    public Transform Obstacles_Transform { set { obstacles_Transform = value; } get { return obstacles_Transform; } }
    public Transform[] Point { set { point = value; } get { return point; } }
    public List<GameObject> PrefabList { set { prefabList = value; } get { return prefabList; } }

    void Start () {
        FindAndInit();
        CreateObstacle();
	}

    protected abstract void FindAndInit();

    protected abstract void CreateObstacle();
}
