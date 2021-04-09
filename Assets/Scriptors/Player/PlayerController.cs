using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    private float life;
    private int vit;
    private float maxLife;
    private float maxVit;
    private int index;          //计时器
    private float maxWalkSpeed;
    private float maxRunSpeed;
    private bool isBreath;          //用于控制是否正在呼吸
    private bool ifDeath;           //玩家是否死亡

    public float Life { set { life = value; } get { return life; } }
    public int Vit { set { vit = value; } get { return vit; } }
    public bool IfDeath { set { ifDeath = value; } get { return ifDeath; } }

    private Transform m_Transform;
    private FirstPersonController FPS;
    private BloodScreenEffect bloodScrecEffect;
    private Transform playerPanel;
    private Image hpImage;
    private Image vitImage;
    private Text hpText;
    private Text vitText;
    private AudioSource audioSource;
    private GameObject m_BuildingPlan;
    private GameObject currentItem;
    private GameObject targetItem;

    public GameObject CurrentItem
    {
        set
        {
            if (currentItem != null)
            {
                currentItem.GetComponent<Animator>().SetTrigger("Holster");
            }
            currentItem = value;
            if (currentItem != null)
            {
                currentItem.SetActive(true);
                if (currentItem.tag == "Build")
                {
                    InputManager.Instance.BuildPanelActive();
                    ToolBarPanelController.Instance.CanMouseScroll = false;
                }else
                {
                    ToolBarPanelController.Instance.CanMouseScroll = true;
                }
            }
            StartCoroutine(ChangeItem());
        }
    }

    private static WaitForSeconds wait;

    void Awake()
    {
        Instance = this;
    }

    void Start () {
        FindAndInit();
        UpdatePlayerHpUI();
        UpdatePlayerVitUI();
        StartCoroutine(RestoreVit());
	}
	
	void Update () {
        ReduceVit();
        InputControl();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        FPS = gameObject.GetComponent<FirstPersonController>();
        bloodScrecEffect = m_Transform.Find("PersonCamera/EnvCamera").GetComponent<BloodScreenEffect>();
        playerPanel = GameObject.Find("Canvas/PlayerPanel").GetComponent<Transform>();
        hpImage = playerPanel.Find("HP/Bar").GetComponent<Image>();
        vitImage = playerPanel.Find("VIT/Bar").GetComponent<Image>();
        hpText = playerPanel.Find("HP/Bar/HPValue").GetComponent<Text>();
        vitText = playerPanel.Find("VIT/Bar/VITValue").GetComponent<Text>();
        audioSource = AudioManager.Instance.AddAudioSourceComponent(ClipName.PlayerBreathingHeavy, gameObject, false);

        life = 300;
        maxLife = life;
        vit = 100;
        maxVit = vit;
        maxWalkSpeed = FPS.M_WalkSpeed;
        maxRunSpeed = FPS.M_RunSpeed;
        isBreath = false;
        ifDeath = false;

        wait = new WaitForSeconds(1);

        m_BuildingPlan = GameObject.Find("FPSController/PersonCamera/Building Plan");
        m_BuildingPlan.SetActive(false);
        currentItem = null;
        targetItem = null;
    }

    /// <summary>
    /// 体力值恢复
    /// </summary>
    IEnumerator RestoreVit()
    {
        while (true)
        {
            if (vit <= 95)
            {
                switch (FPS.playerSportState)
                {
                    case PlayerSportState.IDLE:
                        vit += 5;
                        break;
                    case PlayerSportState.WALK:
                        vit += 3;
                        break;
                    case PlayerSportState.RUN:
                        vit += 1;
                        break;
                }
                UpdatePlayerSpeed();
                UpdatePlayerVitUI();
            }
            if (vit >= 90)
            {
                audioSource.Stop();
                isBreath = false;
            }
            yield return wait;
        }
    }

    /// <summary>
    /// 体力值减少
    /// </summary>
    private void ReduceVit()
    {
        if (FPS.playerSportState == PlayerSportState.WALK && index <=19)
        {
            index++;
        }else if (FPS.playerSportState == PlayerSportState.RUN && index <= 18)
        {
            index += 2;
        }
        if (index >= 20 && vit >= 2)
        {
            vit -= 2;
            UpdatePlayerSpeed();
            UpdatePlayerVitUI();
            index -= 20;
        }

        if (vit < 90 && !isBreath)
        {
            audioSource.Play();
            isBreath = true;
        }
    }

    /// <summary>
    /// 更新玩家速度
    /// </summary>
    private void UpdatePlayerSpeed()
    {
        FPS.M_WalkSpeed = maxWalkSpeed * (vit / maxVit);
        FPS.M_RunSpeed = maxRunSpeed * (vit / maxVit);
    }

    /// <summary>
    /// 更新玩家体力UI信息
    /// </summary>
    private void UpdatePlayerVitUI()
    {
        vitImage.fillAmount = vit / maxVit;
        vitText.text = vit.ToString() + "/" + maxVit.ToString();
    }

    /// <summary>
    /// 削减角色生命
    /// </summary>
    /// <param name="demage"></param>
    public void CutPlayerLife(float demage)
    {
        if (ifDeath == false)
        {
            life -= demage;
            if (life <= 0)
            {
                PlayDeathAudio();
                Dead();
            }
            else
            {
                UpdatePlayerHpUI();
                BloodScreen();
                PlayHitAudio();
            }
        }
        
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    private void Dead()
    {
        ifDeath = true;
        FPS.enabled = false;
        GameObject.Find("Manager").GetComponent<InputManager>().enabled = false;
        ChangeScene();
    }

    /// <summary>
    /// 转变场景
    /// </summary>
    private void ChangeScene()
    {
        SceneManager.LoadScene("Reset");
    }

    /// <summary>
    /// 血屏效果
    /// </summary>
    private void BloodScreen()
    {
        bloodScrecEffect.transparency = 1 - life / maxLife;
    }

    private void PlayHitAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.PlayerHurt, m_Transform.position);
    }

    private void PlayDeathAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.PlayerDeath, m_Transform.position);
    }

    /// <summary>
    /// 更新玩家生命UI信息
    /// </summary>
    private void UpdatePlayerHpUI()
    {
        hpImage.fillAmount = life / maxLife;
        hpText.text = life.ToString() + "/" + maxLife.ToString();
    }

    private void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (currentItem != m_BuildingPlan)
            {
                CurrentItem = m_BuildingPlan;
            }
            else
            {
                CurrentItem = ToolBarPanelController.Instance.CurrentToolModel;
            }
        }
    }

    IEnumerator ChangeItem()
    {
        yield return new WaitForSeconds(1);
        if (currentItem != null)
        {
            currentItem.SetActive(true);
        }
    }

    public void BuildEnd(GameObject currItem)
    {
        if (currentItem != null)
        {
            //这里是为了防止与Build无关的currItem切换,比如两把枪之间互切
            //必须是现在处于Build状态然后再切换才有效
            if (currentItem.tag == "Build")
            {
                if (currentItem == m_BuildingPlan)
                {
                    InputManager.Instance.BuildPanelFreeze();
                }
                CurrentItem = currItem;
            }
        }else
        {
            //这里是为了在一开始currentItem为null时安全的设置currItem
            CurrentItem = currItem;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BaseMaterial")
        {
            InventoryPanelController.Instance.AddItemFromWorld(other.name);
            Destroy(other.gameObject);
        }
    }
}
