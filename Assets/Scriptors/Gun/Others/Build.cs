using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour {

    /// <summary>
    /// 动画事件绑定隐藏自身方法
    /// </summary>
    private void HideSelf()
    {
        InputManager.Instance.BuildPanelFreeze();
        gameObject.SetActive(false);
    }
}
