using UnityEngine;
using System.Collections;

namespace com.QH.QPGame.CX
{

    public class GameLoader : MonoBehaviour
    {

        // Use this for initialization
        IEnumerator Start()
        {
            string strSceneName = "";
            float fScreenRatio = (float)Screen.width / (float)Screen.height;
            if (fScreenRatio <= 1.45f)
            {
                //strSceneName	= "GameScene_ipad";
                strSceneName = "GameScene";
            }
            else if (fScreenRatio <= 1.55f)
            {
                //strSceneName	= "GameScene_iphone";
                strSceneName = "GameScene";
            }
            else
            {
                strSceneName = "GameScene";
            }


            AsyncOperation async = Application.LoadLevelAsync(strSceneName);
            yield return async;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}