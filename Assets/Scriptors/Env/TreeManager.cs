using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : EnvObstacleManager {

    protected override void FindAndInit()
    {
        M_Transform = gameObject.transform;
        Obstacles_Transform = M_Transform.Find("Trees");
        Point = M_Transform.Find("TreePoints").GetComponentsInChildren<Transform>();
        PrefabList = new List<GameObject>();
        PrefabList.Add(Resources.Load<GameObject>("Env/Broadleaf"));
        PrefabList.Add(Resources.Load<GameObject>("Env/Conifer"));
        PrefabList.Add(Resources.Load<GameObject>("Env/Palm"));
    }

    /// <summary>
    /// 创建障碍物
    /// </summary>
    protected override void CreateObstacle()
    {
        for (int i = 1; i < Point.Length; i++)
        {
            Point[i].GetComponent<MeshRenderer>().enabled = false;
            int random = Random.Range(0, 3);
            Vector3 rot = new Vector3(0, Random.Range(0, 360), 0);
            Transform tree;
            tree = Instantiate(PrefabList[random], Point[i].localPosition, Quaternion.Euler(rot), Obstacles_Transform).transform;
            tree.name = PrefabList[random].name;
            tree.localScale = tree.localScale * Random.Range(0.5f, 1.0f);
        }
    }
}
