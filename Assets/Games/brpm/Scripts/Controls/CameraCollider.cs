using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.QH.QPGame.BRPM
{
	public class CameraCollider : MonoBehaviour {

		//普通地面
		public GameObject ground = null;
		//结束地面
		public GameObject lastGround = null;
		//地面父级
		public GameObject groundParent = null;
		//用于储存地面的更换
		public List<GameObject> Ground = new List<GameObject>();
		//生成地面的数量
		public int groundCound = 0;
		//地面的初始位置
		public Vector3 statrPosition = new Vector3(0, 0, 0);
		//地面的间隔
		public float interval = 0;
		//起点到终点的距离
		private float allDistance = 0;
        //已经铺好的地面
        public int beingGround = 5;

        void Awake()
        {
            allDistance = transform.parent.GetComponent<HorseControl>().allDistance;
        }

		//判断地面生成的时机
		void OnTriggerEnter(Collider other)
		{
			if(other.tag == "ground" && (groundCound + beingGround) < allDistance/600 + 1)
			{
				groundCound++;
				GameObject groundObj;

                if (groundCound == (allDistance / 600 - beingGround))
				{
					groundObj = Instantiate(lastGround, Vector3.one, Quaternion.identity) as GameObject;
				}
                else
				{
					groundObj = Instantiate(ground, Vector3.one, Quaternion.identity) as GameObject;
				}

                if ((groundCound + beingGround) / 3 > 0 && (groundCound + beingGround) % 3 == 0 && groundCound != (allDistance / 600 - 1))
				{
                    groundObj.transform.Find("km/km_value").GetComponent<label_number>().m_iNum = 300 - 50 * ((groundCound + beingGround) / 3);
				}
				else
				{
					groundObj.transform.Find("km").gameObject.SetActive(false);
				}

				Vector3 newPosition = new Vector3(interval*groundCound, 0, 0);
				groundObj.transform.parent = groundParent.transform;
				groundObj.transform.localScale = new Vector3(1, 1, 1);
				groundObj.transform.localPosition = newPosition + statrPosition;
				Ground.Add(groundObj);

                if (groundCound > 1)
				{
					Destroy(Ground[0]);
					Ground.Remove(Ground[0]);
				}

			}

		}

	}
}


