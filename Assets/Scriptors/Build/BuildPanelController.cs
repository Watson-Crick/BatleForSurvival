using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelController : MonoBehaviour {

    public static BuildPanelController Instance;
    private BuildPanelView m_BuildPanelView;

    private GameObject tempModel;           //临时模型
    private GameObject finalModel;          //最终创建模型
    private List<SectorUIController> sectorUIList;          //扇形UI管理器脚本
    private List<List<MaterialUIController>> materialUIList;            //材料UI管理器列表
    private List<List<GameObject>> materialModelList;           //材料模型列表
    private List<List<int>> materialUIIndexList;            //材料UI总索引列表

    private int sectorIndex;            //扇形UI索引
    private List<int> materialIndexList;            //各自材料的索引列表
    private float zRot;         //materialUI的rotation
    private BuildPanelState state;
    private bool isClick;           //是否点击过了
    private int layerNum;
    private int SectorIndex {
        set
        {
            sectorIndex = value;
            if (sectorIndex < 0)
                sectorIndex = m_BuildPanelView.SpriteList.Count - 1;
            if (sectorIndex > m_BuildPanelView.SpriteList.Count - 1)
                sectorIndex = 0;
        }
        get { return sectorIndex; }
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        FindAndInit();
        CreateAllSectorUI();
        ShowOrHide(false);
        ResetSectorIndex();
	}

    private void Update()
    {
        MouseControl();
    }

    private void FindAndInit()
    {
        m_BuildPanelView = gameObject.GetComponent<BuildPanelView>();

        tempModel = null;
        sectorUIList = new List<SectorUIController>();
        materialUIList = new List<List<MaterialUIController>>();
        materialModelList = new List<List<GameObject>>();
        materialUIIndexList = new List<List<int>>();
        materialIndexList = new List<int>();

        sectorIndex = 0;
        zRot = -20 + (40 / 3.0f) / 2;
        isClick = false;
    }

    /// <summary>
    /// 创建所有扇形UI
    /// </summary>
    private void CreateAllSectorUI()
    {
        List<Sprite> list = m_BuildPanelView.SpriteList;
        List<Sprite[]> materialList = m_BuildPanelView.MaterialList;
        List<GameObject[]> modelList = m_BuildPanelView.ModelList;
        for (int i = 0; i < list.Count; i++)
        {
            SectorUIController sectorController = Instantiate(m_BuildPanelView.Prefab_SectorBG, m_BuildPanelView.WheelBG_Transform).GetComponent<SectorUIController>();
            sectorController.InitUI(i * 40, list[i]);
            sectorUIList.Add(sectorController);
            List<MaterialUIController> tempList = new List<MaterialUIController>();
            List<GameObject> tempModelList = new List<GameObject>();
            List<int> tempIndexList = new List<int>();
            for (int j = 0; j < materialList[i].Length; j++)
            {
                if (materialList[i][j] != null)
                {
                    MaterialUIController materialController = Instantiate(m_BuildPanelView.Prefab_MaterialUI, m_BuildPanelView.WheelBG_Transform).GetComponent<MaterialUIController>();
                    materialController.InitUI(zRot, materialList[i][j]);
                    tempList.Add(materialController);
                    tempModelList.Add(modelList[i][j]);
                    tempIndexList.Add(j);
                }
                zRot += (40 / 3.0f);
            }
            materialUIList.Add(tempList);
            materialModelList.Add(tempModelList);
            materialUIIndexList.Add(tempIndexList);
            materialIndexList.Add(0);
        }
    }

    /// <summary>
    /// 隐藏自身
    /// </summary>
    public void ShowOrHide(bool show)
    {
        gameObject.SetActive(show);
    }
	
    /// <summary>
    /// 鼠标控制
    /// </summary>
    private void MouseControl()
    {
        MouseScroll();

        if (state == BuildPanelState.CREATE)
        {
            if (m_BuildPanelView.WheelBG_Transform.gameObject.activeSelf)
                m_BuildPanelView.WheelBG_Transform.gameObject.SetActive(false);
            SetTempModelPosition();
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseLeft();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MouseRight();
        }
        isClick = false;
    }

    /// <summary>
    /// 鼠标滚轮相关方法
    /// </summary>
    private void MouseScroll()
    {
        //鼠标未选中时切换扇形UI
        if (state == BuildPanelState.NORMAL)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeSectorIndex(SectorIndex - 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeSectorIndex(SectorIndex + 1);
            }
        }
        else if (state == BuildPanelState.SELECT)          //鼠标选中切换材料UI
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeMaterialIndex(materialIndexList[sectorIndex] + 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeMaterialIndex(materialIndexList[sectorIndex] - 1);
            }
        }
    }

    /// <summary>
    /// 鼠标右键相关方法
    /// </summary>
    private void MouseRight()
    {
        if (state == BuildPanelState.SELECT && !isClick)
        {
            isClick = true;
            state = BuildPanelState.NORMAL;
            materialUIList[sectorIndex][materialIndexList[sectorIndex]].LightMaterialBG(false);
            m_BuildPanelView.M_Text.text = m_BuildPanelView.TextArray[SectorIndex];
        }

        if (state == BuildPanelState.CREATE && !isClick)
        {
            isClick = true;
            if (!m_BuildPanelView.WheelBG_Transform.gameObject.activeSelf)
                m_BuildPanelView.WheelBG_Transform.gameObject.SetActive(true);
            Destroy(tempModel);
            tempModel = null;
            state = BuildPanelState.SELECT;
        }
    }

    /// <summary>
    /// 鼠标左键相关方法
    /// </summary>
    private void MouseLeft()
    {
        if (state == BuildPanelState.NORMAL && sectorIndex != 0 && !isClick)       //扇形UI索引不为0(即非默认状态)且未点击过
        {
            isClick = true;
            state = BuildPanelState.SELECT;
            materialUIList[sectorIndex][materialIndexList[sectorIndex]].LightMaterialBG(true);
            m_BuildPanelView.M_Text.text = materialUIList[sectorIndex][materialIndexList[sectorIndex]].name;
        }

        if (state == BuildPanelState.SELECT && !isClick)           //进入二级选单且未点击过
        {
            isClick = true;
            state = BuildPanelState.CREATE;
            tempModel = CreateModel(m_BuildPanelView.Player_Transform.localPosition + new Vector3(0, 0, 5));
        }

        if (state == BuildPanelState.CREATE && !isClick)            //进入建造状态且未点击过
        {
            if (tempModel != null && tempModel.GetComponent<MaterialModelBase>().State == BuildItemState.CANPUT)
            {
                finalModel = CreateModel(tempModel.transform.position, m_BuildPanelView.Parent_Transform);
                finalModel.name = materialModelList[sectorIndex][materialIndexList[sectorIndex]].name;
                if (finalModel.name == "Door")
                {
                    finalModel.transform.SetParent(tempModel.GetComponent<Door>().Wall_Transform);
                }
                else if (finalModel.name == "Window")
                {
                    finalModel.transform.SetParent(tempModel.GetComponent<Window>().Wall_Transform);
                }else if (finalModel.name == "Ceiling_Light")
                {
                    finalModel.transform.SetParent(tempModel.GetComponent<CeilingLight>().Roof_Transform);
                }
                finalModel.GetComponent<MaterialModelBase>().AttachObject = tempModel.GetComponent<MaterialModelBase>().AttachObject;
                finalModel.GetComponent<MaterialModelBase>().Normal();
                finalModel.transform.rotation = tempModel.transform.rotation;
            }
            isClick = true;
        }
    }

    /// <summary>
    /// 重置sectorIndex
    /// </summary>
    public void ResetSectorIndex()
    {
        ChangeSectorIndex(0);
        state = BuildPanelState.NORMAL;
    }

    /// <summary>
    /// 改变sectorIndex
    /// </summary>
    /// <param name="i"></param>
    private void ChangeSectorIndex(int i)
    {
        sectorUIList[SectorIndex].ShowSectorBG(false);
        SectorIndex = i;
        sectorUIList[SectorIndex].ShowSectorBG(true);
        m_BuildPanelView.M_Text.text = m_BuildPanelView.TextArray[SectorIndex];
    }

    /// <summary>
    /// 改变materialIndex
    /// </summary>
    /// <param name="i"></param>
    private void ChangeMaterialIndex(int i)
    {
        if (i > materialUIList[sectorIndex].Count - 1)
        {
            i = 0;
        }else if (i < 0)
        {
            i = materialUIList[sectorIndex].Count - 1;
        }
        materialUIList[sectorIndex][materialIndexList[sectorIndex]].LightMaterialBG(false);
        materialIndexList[sectorIndex] = i;
        materialUIList[sectorIndex][materialIndexList[sectorIndex]].LightMaterialBG(true);
        m_BuildPanelView.M_Text.text = m_BuildPanelView.MaterialList[SectorIndex][materialUIIndexList[sectorIndex][i]].name;
    }

    private GameObject CreateModel(Vector3 pos, Transform parent = null)
    {
        return Instantiate(materialModelList[sectorIndex][materialIndexList[sectorIndex]], pos, Quaternion.identity, parent);
    }

    /// <summary>
    /// 设置临时模型位置
    /// </summary>
    private void SetTempModelPosition()
    {
        Ray ray = m_BuildPanelView.Env_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (tempModel != null)
        {
            if (tempModel.name == "Roof(Clone)" || tempModel.name == "Ceiling_Light(Clone)")
            {
                layerNum = ~(1 << 13);
            }else
            {
                layerNum = (1 << 0);
            }
            if (Physics.Raycast(ray, out hit, 15, layerNum))
            {
                LineRenderer line = gameObject.GetComponent<LineRenderer>();
                line.SetPosition(0, m_BuildPanelView.Player_Transform.position);
                line.SetPosition(1, hit.point);
                if (!tempModel.GetComponent<MaterialModelBase>().IsAttach)
                {
                    tempModel.transform.position = hit.point;
                }
                if (Vector3.Distance(tempModel.transform.position, hit.point) > 1.5f)
                {
                    tempModel.GetComponent<MaterialModelBase>().IsAttach = false;
                }
            }
        }
    }

    public void ClosePanel()
    {
        if (state == BuildPanelState.CREATE)
        {
            Destroy(tempModel);
            tempModel = null;
            m_BuildPanelView.WheelBG_Transform.gameObject.SetActive(true);
            state = BuildPanelState.NORMAL;
        }
        ResetSectorIndex();
    }
}
