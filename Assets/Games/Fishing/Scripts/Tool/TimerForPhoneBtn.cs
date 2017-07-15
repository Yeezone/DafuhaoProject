using UnityEngine;
using com.QH.QPGame.Fishing;

public class TimerForPhoneBtn : MonoBehaviour {

    public static TimerForPhoneBtn Instance;

    public float TimeLength = 60f;

    public float CurTime;

    // 手机版打开两侧按钮的时间周期
    public float waitTime_allButton = 5f;
    private float CurTime_allButton;

    void Start()
    {
        CurTime = TimeLength;
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    void Update()
    {

        CurTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            CurTime = TimeLength;
        }

        if (TimeLength - CurTime >= waitTime_allButton)
        {
            CanonCtrl.Instance.m_bTimeOut = true;
        }
        else
        {
            CanonCtrl.Instance.m_bTimeOut = false;
        }
    }
}
