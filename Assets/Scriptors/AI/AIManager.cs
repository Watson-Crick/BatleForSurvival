using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    private Transform m_Transform;
    private Transform AIparent_Transform;
    private Transform[] BirthPos_Transform;
    private Transform[] PatrolPos_Transform;

    private List<GameObject> prefabList;
    private List<GameObject> AIList;

    private AIManagerType aiManagerType = AIManagerType.NULL;
    private ObjectPool pool;

    public AIManagerType AIManagerType { set { aiManagerType = value; } get { return aiManagerType; } }
    public ObjectPool Pool { get { return pool; } }

	void Start () {
        FindAndInit();
        CreateAIByEnum();
	}
	
    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        AIparent_Transform = m_Transform.Find("AIParent");
        BirthPos_Transform = m_Transform.Find("BirthPos").GetComponentsInChildren<Transform>(true);
        PatrolPos_Transform = m_Transform.Find("PatrolPos").GetComponentsInChildren<Transform>(true);

        prefabList = new List<GameObject>();
        AIList = new List<GameObject>();
        prefabList.Add(Resources.Load<GameObject>("AI/Boar"));
        prefabList.Add(Resources.Load<GameObject>("AI/Cannibal"));

        pool = new ObjectPool();
    }

    /// <summary>
    /// 通过枚举创建UI角色
    /// </summary>
    private void CreateAIByEnum()
    {
        GameObject temp = null;
        int demage, life;
        Func(out temp, out demage, out life);
        CreateAI(temp, demage, life);
    }

    /// <summary>
    /// 创建UI角色
    /// </summary>
    private void CreateAI(GameObject prefab, int demage, int life)
    {
        for (int i = 1; i < BirthPos_Transform.Length; i++)
        {
            GameObject temp = Instantiate(prefab, BirthPos_Transform[i].position, Quaternion.identity, AIparent_Transform);
            AIController controller = temp.GetComponent<AIController>();
            controller.PatrolPos = PatrolPos_Transform;
            controller.Demage = demage;
            controller.Life = life;
            controller.Parent_AIManager = this;
            
            AIList.Add(temp);
        }
    }
    
    /// <summary>
    /// 通过枚举判断该创建哪一预制体
    /// </summary>
    private void Func(out GameObject temp, out int demage, out int life)
    {
        if (aiManagerType == AIManagerType.BOAR)
        {
            temp = prefabList[0];
            demage = 50;
            life = 200;
        }
        else if (aiManagerType == AIManagerType.CANNIBAL)
        {
            temp = prefabList[1];
            demage = 20;
            life = 100;
        }else
        {
            temp = null;
            demage = 0;
            life = 0;
        }
    }

    /// <summary>
    /// AI角色死亡
    /// </summary>
    private void AIDeath(GameObject go)
    {
        AIController controller = go.GetComponent<AIController>();
        AIList.Remove(go);
        StartCoroutine(ReCreateAI(controller.StartPos));
    }

    /// <summary>
    /// AI角色重生
    /// </summary>
    IEnumerator ReCreateAI(Vector3 startPos)
    {
        yield return new WaitForSeconds(3);

        GameObject temp = null;
        int demage, life;
        Func(out temp, out demage, out life);
        GameObject go = Instantiate(temp, startPos, Quaternion.identity, AIparent_Transform);
        AIController controller = go.GetComponent<AIController>();
        controller.PatrolPos = PatrolPos_Transform;
        controller.Demage = demage;
        controller.Life = life;
        controller.Parent_AIManager = this;

        AIList.Add(go);
    }
}
