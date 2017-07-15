using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Shared;

namespace com.QH.QPGame.SH
{
    [AddComponentMenu("Custom/Controls/Face")]

    public class UIFace : MonoBehaviour
    {
        int _FaceID = 0;

        public GameObject Face = null;
        public GameObject Vip = null;


        void Start()
        {

        }


        void FixedUpdate()
        {


        }

        public void ShowFace(int dwFaceID, int nVipLevel)
        {

            _FaceID = dwFaceID;
            if (_FaceID <= 10000)
            {
//				if(_FaceID > 12)
//				{
//					_FaceID = UnityEngine.Random.Range(0,1);
//				}
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

        IEnumerator LoadCustomFace(int dwFaceID)
        {

            string strPath = dwFaceID.ToString() + ".png";



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
                    File.WriteAllBytes(Application.persistentDataPath + "/" + dwFaceID.ToString() + ".png", www.texture.EncodeToPNG());
                    //}
                }

            }
            else
            {
                Debug.Log(www.error);
            }
        }

        IEnumerator LoadLocalFace(int dwFaceID)
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


        void LoadNormalFace(int dwFaceID)
        {
            if (Face == null) return;


            UISprite sprite = Face.GetComponent<UISprite>();
			sprite.spriteName = "face_" + dwFaceID;

        }
        void LoadVipFlag(int nVipLevel)
        {
            if (Vip == null) return;

            UITexture sprite = Vip.GetComponent<UITexture>();
            if (nVipLevel == -1)
            {
                sprite.mainTexture = (Texture2D)Resources.Load("Image/Face/vip_level_0");
            }
            else
            {
                sprite.mainTexture = (Texture2D)Resources.Load("Image/Face/vip_level_" + nVipLevel.ToString());
            }
        }
    }
}