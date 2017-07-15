using UnityEngine;

namespace com.QH.QPGame.DDZ
{
    public class GameLoader : MonoBehaviour
    {

        // Use this for initialization
        /*IEnumerator Start () 
        {
            string strSceneName = "";
            float fScreenRatio = (float)Screen.width / (float)Screen.height;
            if(fScreenRatio<=1.45f)
            {
                Debug.Log("ipad:"+Screen.width+","+Screen.height);
                //strSceneName	= "GameScene_ipad";
                strSceneName	= "GameScene";
            }
            else if(fScreenRatio<=1.55f)
            {
                Debug.Log("iphone:"+Screen.width+","+Screen.height);
                //strSceneName	= "GameScene_iphone";
                strSceneName	= "GameScene";
            }
            else
            {
                Debug.Log("others:"+Screen.width+","+Screen.height);
                strSceneName	= "GameScene";
            }
		
	
            AsyncOperation async = Application.LoadLevelAsync(strSceneName);
            yield return async;
        }*/

        // Update is called once per frame
        private void Update()
        {

        }
    }
}