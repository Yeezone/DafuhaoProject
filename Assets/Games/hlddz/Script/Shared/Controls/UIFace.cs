using UnityEngine;
using System.Collections;
using System.IO;

namespace com.QH.QPGame.DDZ
{
    [AddComponentMenu("Custom/Controls/Face")]

    public class UIFace : MonoBehaviour
    {
        private int _FaceID = 0;

        public GameObject Face = null;
        public GameObject Vip = null;


        private void Start()
        {

        }


        private void FixedUpdate()
        {


        }

        public void ShowFace(int dwFaceID, int nVipLevel)
        {

            _FaceID = dwFaceID;
            if (_FaceID <= 10000)
            {
                LoadNormalFace(_FaceID);
            }
            else
            {
                string strTempPath = Application.persistentDataPath + "/" + dwFaceID.ToString() + ".png";
                //
                if (File.Exists(strTempPath))
                {
                    StartCoroutine(LoadLocalFace(_FaceID));
                }
                else
                {
                    StartCoroutine(LoadCustomFace(_FaceID));
                }

                //if(Logic.GameEngine.Instance.MyUser.Userid==dwFaceID)
                //{

                //}
                //else
                //{
                //
                //    StartCoroutine(LoadCustomFace(_FaceID));
                //}
            }
            LoadVipFlag(nVipLevel);
        }

        private IEnumerator LoadCustomFace(int dwFaceID)
        {

            string strPath = "/" + dwFaceID.ToString() + ".png";



            WWW www = new WWW(strPath);
            yield return www;

            if (www.error == null)
            {
                if (Face != null)
                {
                    UITexture sprite = Face.GetComponent<UITexture>();
                    sprite.mainTexture = www.texture;

                    //if(dwFaceID==Logic.GameEngine.Instance.MyUser.Userid)
                    //{
                    File.WriteAllBytes(Application.persistentDataPath + "/" + dwFaceID.ToString() + ".png",
                                       www.texture.EncodeToPNG());
                    //}
                }

            }
            else
            {
                Debug.Log(www.error);
            }
        }

        private IEnumerator LoadLocalFace(int dwFaceID)
        {

            string path = "file://" + Application.persistentDataPath + "/" + dwFaceID.ToString() + ".png";

            WWW www = new WWW(path);
            yield return www;
            if (www.error == null)
            {
                if (Face != null)
                {
                    UITexture sprite = Face.GetComponent<UITexture>();
                    sprite.mainTexture = www.texture;
                }
            }

        }


        private void LoadNormalFace(int dwFaceID)
        {

            if (Face == null) return;


            UISprite sprite = Face.GetComponent<UISprite>();
			sprite.spriteName = "face_" + dwFaceID;

            /*UITexture sprite = Face.GetComponent<UITexture>();


            if(dwFaceID==-1)
            {
                sprite.mainTexture = (Texture2D)Resources.Load("Image/Face/blank");
            }
            else if(dwFaceID==-2)
            {
                sprite.mainTexture = (Texture2D) Resources.Load("Image/Face/face_robot");
            }
            else if(dwFaceID==-3)
            {
                sprite.mainTexture = (Texture2D) Resources.Load("Image/Face/face_offline");
            }
            else
            {
                //dwFaceID = dwFaceID;
               sprite.mainTexture = (Texture2D)Resources.Load("face_" + dwFaceID);
            }*/

        }

        private void LoadVipFlag(int nVipLevel)
        {
            if (Vip == null) return;

            UITexture sprite = Vip.GetComponent<UITexture>();
            if (nVipLevel == -1)
            {
                sprite.mainTexture = (Texture2D) Resources.Load("Image/Face/vip_level_0");
            }
            else
            {
                sprite.mainTexture = (Texture2D) Resources.Load("Image/Face/vip_level_" + nVipLevel.ToString());
            }
        }
    }
}