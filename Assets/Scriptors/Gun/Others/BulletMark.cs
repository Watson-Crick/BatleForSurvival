using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour {

    private Texture2D main_Texture;                              //主贴图
    private Texture2D bulletMark_Texture;                    //弹痕贴图
    private Texture2D main_TextureBackUp;                 //主贴图备份

    private Transform effectSetTransform;                      //特效集合

    private Queue<Vector2> uvQueue;                           //弹痕uv坐标队列

    [SerializeField]
    private MaterialType materialType;                          //材质类型枚举

    public MaterialType MaterialType { get { return materialType; } }

    private GameObject effect;                                       //特效
    private GameObject prefab_DropMaterial;

    private ObjectPool pool;

    [SerializeField] private int materialLife;          //作为材料的生命值
    [SerializeField] private int obstacleLife;          //作为障碍物的生命值

	void Start () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        if (gameObject.name == "Broadleaf")
        {
            main_Texture = Instantiate<Texture2D>(gameObject.GetComponent<MeshRenderer>().materials[1].mainTexture as Texture2D);
        } else if (gameObject.name == "Conifer")
        {
            main_Texture = Instantiate<Texture2D>(gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture as Texture2D);
        } else if (gameObject.name == "Palm")
        {
            main_Texture = Instantiate<Texture2D>(gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture as Texture2D);
        }
        else
        {
            main_Texture = Instantiate<Texture2D>(gameObject.GetComponent<MeshRenderer>().material.mainTexture as Texture2D);
        }
        switch(materialType)
        {
            case MaterialType.Wood:
                ResourcesLoad("Bullet Decal_Wood", "Bullet Impact FX_Wood", "Effect_Wood_Set");
                materialLife = 100;
                obstacleLife = 150;
                break;
            case MaterialType.Stone:
                ResourcesLoad("Bullet Decal_Stone", "Bullet Impact FX_Stone", "Effect_Stone_Set");
                materialLife = 150;
                obstacleLife = 300;
                prefab_DropMaterial = Resources.Load<GameObject>("Env/Rock_Normal_Material");
                break;
            case MaterialType.Flesh:
                ResourcesLoad("Bullet Decal_Flesh", "Bullet Impact FX_Flesh", "Effect_Flesh_Set");
                break;
            case MaterialType.Metal:
                ResourcesLoad("Bullet Decal_Metal(1)", "Bullet Impact FX_Metal", "Effect_Metal_Set");
                materialLife = 200;
                obstacleLife = 400;
                prefab_DropMaterial = Resources.Load<GameObject>("Env/Rock_Metal_Material");
                break;
        }
        main_TextureBackUp = Instantiate<Texture2D>(main_Texture);

        uvQueue = new Queue<Vector2>();

        pool = new ObjectPool();
    }

    /// <summary>
    /// 生成弹痕
    /// </summary>
    public void CreateBulletMark(RaycastHit hit)
    {
        PlayAudio();
        Vector2 uv = hit.textureCoord;
        uvQueue.Enqueue(uv);

        for (int i = 0; i < bulletMark_Texture.width; i++)
        {
            for (int j = 0; j < bulletMark_Texture.height; j++)
            {
                float x = uv.x * main_Texture.width - bulletMark_Texture.width / 2 + i;
                float y = uv.y * main_Texture.height - bulletMark_Texture.height / 2 + j;

                Color color = bulletMark_Texture.GetPixel(i, j);

                if (color.a > 0.2f)
                {
                    main_Texture.SetPixel((int)x, (int)y, color);
                }
            }
        }
        main_Texture.Apply();

        gameObject.GetComponent<MeshRenderer>().material.mainTexture = main_Texture;

        PlayEffect(hit);

        Invoke("ClearBulletMark", 3);
    }

    /// <summary>
    /// 清除弹痕
    /// </summary>
    private void ClearBulletMark()
    {
        Vector2 uv = uvQueue.Dequeue();

        for (int i = 0; i < bulletMark_Texture.width; i++)
        {
            for (int j = 0; j < bulletMark_Texture.height; j++)
            {
                float x = uv.x * main_Texture.width - bulletMark_Texture.width / 2 + i;
                float y = uv.y * main_Texture.height - bulletMark_Texture.height / 2 + j;

                Color color = main_TextureBackUp.GetPixel((int)x, (int)y);
                main_Texture.SetPixel((int)x, (int)y, color);
            }
        }
        main_Texture.Apply();
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    private void PlayEffect(RaycastHit hit)
    {
        GameObject temp = null;
        if (pool.Data())
        {
            //对象池不为空,提取对象
            temp = pool.GetObject();
            temp.transform.position = hit.point;
            temp.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
        else
        {
            //对象池为空,新建对象
            temp = Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal), effectSetTransform);
            temp.name = "Effect_" + materialType;
        }
        StartCoroutine(DelayAddObjectPool(temp, 3.0f));
    }

    /// <summary>
    /// 延迟一段时间后将物体加入对象池
    /// </summary>
    IEnumerator DelayAddObjectPool(GameObject temp, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(temp);
    }

    /// <summary>
    /// 贴图,特效等资源读取
    /// </summary>
    private void ResourcesLoad(string bulletMarkTexturePath, string effectPath, string effectSetPath)
    {
        bulletMark_Texture = Resources.Load<Texture2D>("Textures/BulletMarks/" + bulletMarkTexturePath);
        effect = Resources.Load<GameObject>("Effects/Gun/" + effectPath);
        effectSetTransform = GameObject.Find("TempObject/" + effectSetPath).transform;
    }

    private void PlayAudio()
    {
        switch (materialType)
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

    public void HatchetHit(RaycastHit hit, int damege)
    {
        PlayAudio();
        PlayEffect(hit);
        materialLife -= damege;
        if (materialLife <= 0)
        {
            DropItem();
            Invoke("DestroySelf", 0.5f);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void DropItem()
    {
        GameObject go = Instantiate(prefab_DropMaterial, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        go.name = "2";
    }
}
