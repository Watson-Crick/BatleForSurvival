using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : EnvObstacleManager{

    protected override void FindAndInit()
    {
        M_Transform = gameObject.transform;
        Obstacles_Transform = M_Transform.Find("Stones");
        Point = M_Transform.Find("StonePoints").GetComponentsInChildren<Transform>();
        PrefabList = new List<GameObject>();
        PrefabList.Add(Resources.Load<GameObject>("Env/Rock_Normal"));
        PrefabList.Add(Resources.Load<GameObject>("Env/Rock_Metal"));
    }

    /// <summary>
    /// 创建障碍物
    /// </summary>
    protected override void CreateObstacle()
    {
        for (int i = 1; i < Point.Length; i++)
        {
            Point[i].GetComponent<MeshRenderer>().enabled = false;
            int random = Random.Range(0, 2);
            Vector3 rot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Transform stone;
            stone = Instantiate(PrefabList[random], Point[i].localPosition, Quaternion.Euler(rot), Obstacles_Transform).transform;
            stone.name = PrefabList[random].name;
            stone.localScale = stone.localScale * Random.Range(0.7f, 1.2f);
        }
    }
	
}
