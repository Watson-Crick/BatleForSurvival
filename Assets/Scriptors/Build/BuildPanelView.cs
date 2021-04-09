using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelView : MonoBehaviour {

    private Transform m_Transform;
    private Transform wheelBG_Transform;
    private Transform player_Transform;
    private Transform parent_Transform;
    private Camera env_Camera;
    private GameObject prefab_SectorBG;
    private GameObject prefab_MaterialUI;
    private Text m_Text;

    private List<Sprite> spriteList;
    private List<Sprite[]> materialList;
    private List<GameObject[]> modelList;
    private string[] textArray;

    public Transform M_Transform { get { return m_Transform; } }
    public Transform WheelBG_Transform { get { return wheelBG_Transform; } }
    public Transform Player_Transform { get { return player_Transform; } }
    public Transform Parent_Transform { get { return parent_Transform; } }
    public Camera Env_Camera { get { return env_Camera; } }
    public GameObject Prefab_SectorBG { get { return prefab_SectorBG; } }
    public GameObject Prefab_MaterialUI { get { return prefab_MaterialUI; } }
    public Text M_Text { get { return m_Text; } }

    public List<Sprite> SpriteList { get { return spriteList; } }
    public List<Sprite[]> MaterialList { get { return materialList; } }
    public List<GameObject[]> ModelList { get { return modelList; } }
    public string[] TextArray { get { return textArray; } }

	void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        prefab_SectorBG = Resources.Load<GameObject>("Build/Prefab/SectorBG");
        prefab_MaterialUI = Resources.Load<GameObject>("Build/Prefab/MaterialUI");
        m_Text = m_Transform.Find("WheelBG/Text").GetComponent<Text>();
        wheelBG_Transform = m_Transform.Find("WheelBG");
        player_Transform = GameObject.Find("FPSController").transform;
        parent_Transform = GameObject.Find("BuildModel").transform;
        env_Camera = player_Transform.Find("PersonCamera/EnvCamera").GetComponent<Camera>();

        InitSpriteList();
        InitMaterialList();
        InitModelList();

        textArray = new string[] { "", "杂项", "屋顶", "楼梯", "窗户", "门", "墙面", "地板", "地基" };
    }

    private void InitSpriteList()
    {
        spriteList = new List<Sprite>();
        spriteList.Add(null);
        spriteList.Add(LoadSprite("Icon", "Question Mark"));
        spriteList.Add(LoadSprite("Icon", "Roof_Category"));
        spriteList.Add(LoadSprite("Icon", "Stairs_Category"));
        spriteList.Add(LoadSprite("Icon", "Window_Category"));
        spriteList.Add(LoadSprite("Icon", "Door_Category"));
        spriteList.Add(LoadSprite("Icon", "Wall_Category"));
        spriteList.Add(LoadSprite("Icon", "Floor_Category"));
        spriteList.Add(LoadSprite("Icon", "Foundation_Category"));
    }

    private void InitMaterialList()
    {
        materialList = new List<Sprite[]>();
        materialList.Add(new Sprite[] { null, null, null });
        materialList.Add(new Sprite[] { LoadSprite("Material", "Ceiling Light"), LoadSprite("Material", "Pillar_Wood"), LoadSprite("Material", "Wooden Ladder") });
        materialList.Add(new Sprite[] { null, LoadSprite("Material", "Roof_Metal"), null });
        materialList.Add(new Sprite[] { LoadSprite("Material", "Stairs_Wood"), LoadSprite("Material", "L Shaped Stairs_Wood"), null });
        materialList.Add(new Sprite[] { null, LoadSprite("Material", "Window_Wood"), null });
        materialList.Add(new Sprite[] { null, LoadSprite("Material", "Wooden Door"), null });
        materialList.Add(new Sprite[] { LoadSprite("Material", "Wall_Wood"), LoadSprite("Material", "Doorway_Wood"), LoadSprite("Material", "Window Frame_Wood") });
        materialList.Add(new Sprite[] { null, LoadSprite("Material", "Floor_Wood"), null });
        materialList.Add(new Sprite[] { null, LoadSprite("Material", "Platform_Wood"), null });
    }

    private void InitModelList()
    {
        modelList = new List<GameObject[]>();
        modelList.Add(new GameObject[] { null, null, null });
        modelList.Add(new GameObject[] { LoadGameObject("Model", "Ceiling_Light"), LoadGameObject("Model", "Pillar"), LoadGameObject("Model", "Ladder") });
        modelList.Add(new GameObject[] { null, LoadGameObject("Model", "Roof"), null });
        modelList.Add(new GameObject[] { LoadGameObject("Model", "Stairs"), LoadGameObject("Model", "L_Shaped_Stairs"), null });
        modelList.Add(new GameObject[] { null, LoadGameObject("Model", "Window"), null });
        modelList.Add(new GameObject[] { null, LoadGameObject("Model", "Door"), null });
        modelList.Add(new GameObject[] { LoadGameObject("Model", "Wall"), LoadGameObject("Model", "Doorway"), LoadGameObject("Model", "Window_Frame") });
        modelList.Add(new GameObject[] { null, LoadGameObject("Model", "Floor"), null });
        modelList.Add(new GameObject[] { null, LoadGameObject("Model", "Platform"), null });
    }

    private Sprite LoadSprite(string floder, string name)
    {
        return Resources.Load<Sprite>("Build/" + floder  + "/" + name);
    }

    private GameObject LoadGameObject(string floder, string name)
    {
        return Resources.Load<GameObject>("Build/" + floder + "/" + name);
    }
}
