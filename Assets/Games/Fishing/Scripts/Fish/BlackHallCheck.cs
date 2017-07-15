using UnityEngine;

namespace com.QH.QPGame.Fishing
{
	[RequireComponent(typeof(Rigidbody2D))]
	[ExecuteInEditMode]
	public class BlackHallCheck : MonoBehaviour 
	{
		Rigidbody2D rigid2D;
		SingleFish blackHall;
		int killByCanonID;

		void Awake () 
		{
			rigid2D = GetComponent<Rigidbody2D>();
			if(rigid2D!=null)
			{
				rigid2D.gravityScale = 0f;
				rigid2D.isKinematic = true;
			}
			blackHall = transform.parent.GetComponent<SingleFish>();
		}

		void OnTriggerEnter2D(Collider2D _collider)
		{
			SingleFish _sf = _collider.GetComponent<SingleFish>();

			// 9 是鱼在场景中所在的层（layers）.
			if(_collider.gameObject.layer==9)
			{
				if(_sf !=null && _sf!=blackHall && _sf.fishType!=BaseFish.FishType.blackHall && _sf.fishType!=BaseFish.FishType.quanping)
				{
					// 如果黑洞死了.
					if(!blackHall.dead)
					{
						// can be absorbed. wont absorb deading fish(including the fish killed by quanping).
						if(_sf.beAbsorbed && _sf.cur_Collider.enabled)
						{
							_sf.AbosorbByBlackHall(blackHall.dead, transform, blackHall.canonID);
						}
					}
					else
					{
						// 如果黑洞已经吸够分数了，开始回收这个黑洞, 就不要吸鱼了.
						if(blackHall.specialFishDeadState != BaseFish.SpecialFishDeadState.recycle)
						{
							_sf.AbosorbByBlackHall(blackHall.dead, transform, blackHall.canonID);

							blackHall.bh_AbosorbOneFish(blackHall.power, blackHall.blackHallAbsorbMulti, _sf.transform.position);
						}
					}
				}
			}
		}
	}
}
