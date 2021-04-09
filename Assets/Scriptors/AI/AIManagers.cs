using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManagers : MonoBehaviour {

    private Transform m_Transform;
    private Transform AI_1_Transform;
    private Transform AI_2_Transform;

	void Awake () {
        FindAndInit();
        CreateAIManager();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        AI_1_Transform = m_Transform.Find("AI_1");
        AI_2_Transform = m_Transform.Find("AI_2");
    }

    private void CreateAIManager()
    {
        AI_1_Transform.gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.CANNIBAL;
        AI_2_Transform.gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.BOAR;
    }
	

}
