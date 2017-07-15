using UnityEngine;

namespace com.QH.QPGame.Fishing
{
    public class SingleFish : BaseFish
    {		

        void OnEnable()
        {
            // 开启碰撞体.
            cur_Collider.enabled = true;

            // 显示层降回原来.
			UISprite[] renderers = this.GetComponentsInChildren<UISprite>();

            // 初始化鱼动画的帧率
            if (gameObject.GetComponent<UISpriteAnimation>() != null)
            {
                gameObject.GetComponent<UISpriteAnimation>().framesPerSecond = fishAnimFPS;
            }

            for (int i = 0; i < renderers.Length; i++)
            {
                var item = renderers[i];
                item.depth = orgOrderInlayer;
                // 把组合鱼的底图设低层
//                 if (item.gameObject.tag == "fishs_bg")
//                 {
//                     item.depth = orgOrderInlayer - 2;
//                 }
            }
//            cur_SpriteRenderer.depth = orgOrderInlayer;

            // 大小为0.
            cur_Transform.localScale = Vector3.zero;

            removeFromFishList = false;

            lastMap2ClearFish = false;

            killByBlackHall = false;
            blackHall = null;
            lerp2BlackHallPercent = 0f;

            locking = false;
            dead = false;
            recycleTargetTime = 0f;
            fishDeadValue = 0;

            playCoinAudio = true;
            startHurt = false;

            // 鱼的颜色回到初始值
            for (int i = 0; i < hurtRends.Length; i++)
            {
                hurtRends[i].color = orgColor;
            }

            switch (fishType)
            {
                case FishType.ywdj:
                    {
                        specialFishDeadState = SpecialFishDeadState.none;
                        break;
                    }
                case FishType.quanping:
                    {
                        specialFishHaveAddValue = false;
                        break;
                    }
                case FishType.blackHall:
                    {
                        specialFishDeadState = SpecialFishDeadState.none;
                        bh_need2AbsorbMulti = 0;
                        specialFishHaveAddValue = false;
                        // 重置吸鱼的分值.
                        if (blackHallNum != null)
                        {
                            blackHallNum.ApplyValue(0, 0);
                        }
                        break;
                    }
                case FishType.likui:
                    {
                        if (LikuiNumPrefab != null && (LikuiNumCreated == null || !LikuiNumCreated.gameObject.activeSelf))
                        {
                            LikuiNumCreated = Factory.Create(LikuiNumPrefab, Vector3.zero, Quaternion.identity);
                            LikuiNumCreated.GetComponent<UILabel>().text = multi.ToString();
                            LikuiNumCreated.parent = cur_Transform;
                            LikuiNumCreated.localPosition = new Vector3(0f, 2 * LikuiHeight, 0f);
                            LikuiNumCreated.localEulerAngles = Vector3.zero;
                            LikuiNumCreated.localScale = LikuiNumScale;
                            LiKuiCurrentBeilv = LikuiNumCreated.GetComponent<UILabel>();
                            LiKuiCurrentBeilv.text = multi.ToString();
                        }
                        break;
                    }
            }
        }

        // 初始化鱼的参数.
        public void InitFishParam(NavPath _nav, int _fishServerID, SingleShadow _ss, int _fishPool)
        {
            // 变大.
            iTween.ScaleTo(gameObject, localScale, 1f);
            // 这条鱼所对应的影子.
            singleShadow = _ss;
            // 鱼的id.
            fishServerID = _fishServerID;
            // 鱼所在的池，方便同类炸弹使用.
            fishPool = _fishPool;
            // 初始化路径.
            navigation.init(cur_Transform, _nav);
            if (fishType == FishType.likui)
            {
                multi = LikuiBeginBeilv;
            }
        }

        protected override void Update()
        {

            DoHurtEff();

            // 游动.
            navigation.update();

            // 鱼还没死亡.
            if (!dead)
            {
                // 李逵每隔一定时间增加倍率
                LikuiAddRote();

                // 每隔一段时间检查一次 .
                if (Time.time > targetTime2CheckRecycle)
                {
                    IsNpcNavigationOver();
                    targetTime2CheckRecycle = Time.time + 0.5f;
                }
            }
            // 鱼死了.
            else
            {
                // 1, 如果是被黑洞杀死的，那就只要做黑洞吸鱼的效果.
                if (killByBlackHall)
                {
                    DidBlackHallAbsorbEffAndRecycle();
                    return;
                }

                // 2, 全屏炸弹.
                if (fishType == FishType.quanping)
                {
                    quanpingDead();
                    return;
                }

                // 3, 如果是黑洞.
                if (fishType == FishType.blackHall)
                {
                    blackHallDead();
                    return;
                }

                //4.回收李逵数字
                if (fishType == FishType.likui)
                {
                    // coin.
                    CoinCtrl.Instance.CreateCoins(canonID, coinType, coinNum, cur_Transform.position, playCoinAudio);
                    // 喇叭.
//                    TrumpetCtrl.Instance.CreateOneTrumpet(canonID, serverMulti, fishDeadValue);
                    RecycleFish();
                    return;
                }

                // 5, 普通鱼.
                if (Time.time > recycleTargetTime)
                {
                    // coin.
                    CoinCtrl.Instance.CreateCoins(canonID, coinType, coinNum, cur_Transform.position, playCoinAudio);
                    RecycleFish();
                }

            }
        }

        // 李逵倍率增加
        private void LikuiAddRote()
        {
            if (fishType == FishType.likui)
            {
                likuiCurrent += Time.deltaTime;
                if (likuiCurrent > LiKuiBeilvTime && multi < LiKuibeilvHigh)
                {
                    multi += LiKuiAddBeilv;
                    //LiKuiCurrentBeilv.ApplyValue(multi, 0);
                    LiKuiCurrentBeilv.text = multi.ToString();
                    likuiCurrent = 0f;
                }
                if (LikuiNumCreated != null && LikuiNumCreated.gameObject.activeSelf)
                {
                    LikuiNumCreated.localPosition = new Vector3(0f, 2 * LikuiHeight, 0f);
                }
            }
        }

        // NPC路径游完
        private void IsNpcNavigationOver()
        {
            // 路径已经游完了，回收鱼.
            if (navigation.timeStamp >= 1f)
            {
                RecycleFish();
            }

            // 如果不在屏幕之内了，如果这条鱼在锁定状态，就取消这条鱼的锁定状态.
            if (CanonCtrl.Instance.turn_screen && CanonCtrl.Instance.turn_screen_on_of)
            {
                if (cur_Transform.position.x > viewportRect.x)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.y < viewportRect.y)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.x < viewportRect.z)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.y > viewportRect.w)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
            }
            else
            {
                if (cur_Transform.position.x < viewportRect.x)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.y > viewportRect.y)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.x > viewportRect.z)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
                if (cur_Transform.position.y < viewportRect.w)
                {
                    if (locking)
                    {
                        locking = false;
                    }
                }
            }
        }

        //全屏死亡
        private void quanpingDead()
        {
            switch (specialFishDeadState)
            {
                // 移动这条鱼到屏幕中心.
                case SpecialFishDeadState.drag2Center:
                    {
                        specialFishMovePercent += Time.deltaTime * move2CenterSpeed;
                        cur_Transform.position = Vector3.Lerp(move2CenterStartPos, Vector3.zero, specialFishMovePercent);
                        if (specialFishMovePercent > 1f)
                        {
                            // 如果有死亡特效，就做特效.
                            if (deadEffList.Count > 0)
                            {
                                FishDeadEffHandler.CreatedFishDeadEffCtrlItem(this, deadEffList);
                            }

                            // 还剩多长时间就回收鱼 = 鱼死亡时间 - 移动到屏幕的时间.
                            float _leftTimeLength = deadTimeLength - move2CenterTimeLength;
                            if (_leftTimeLength < 0f)
                            {
                                _leftTimeLength = 0f;
                            }
                            // 移动的速度.
                            move2CenterSpeed = 1f / _leftTimeLength;
                            specialFishMovePercent = 0f;
                            specialFishDeadState = SpecialFishDeadState.wait;
                        }
                        break;
                    }
                case SpecialFishDeadState.wait:
                    {
                        specialFishMovePercent += Time.deltaTime * move2CenterSpeed;
                        if (specialFishMovePercent > 1f)
                        {
                            if (!specialFishHaveAddValue)
                            {
                                // 把分数加给玩家.
                                CanonCtrl.Instance.singleCanonList[canonID].AddValueList(fishDeadValue, 0f);
                                specialFishHaveAddValue = true;
                            }

                            // 处理全屏鱼杀死其他鱼的逻辑.
                            SpecialFishDeadHandler.QuanPing_Kill_Fish(this, canonID, bulletCost);

                            // 鱼身上的分数.
                            // NumCtrl.Instance.CreateFishDeadNum(_value, m_traCache.position, Quaternion.identity);

                            // 鱼的 bubble 分数.
                            NumCtrl.Instance.AdddBubble2List(serverMulti, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].bubbleShowUpPos.position, Quaternion.identity, CanonCtrl.Instance.singleCanonList[canonID].upsideDown);

                            // 鱼的金币.
                            CoinCtrl.Instance.CreateCoins(canonID, coinType, coinNum, cur_Transform.position, playCoinAudio);

                            // 如果不是当鱼死亡后就播放 high score， 就在这里播放.
                            if (!showHighScoreWhenFishDead)
                            {
                                // 有high score 预设，并且分数不为0.
                                if (highScorePrefab != null && fishDeadValue > 0)
                                {
                                    HighScoreCtrl.Instance.Create(canonID, highScorePrefab, CanonCtrl.Instance.singleCanonList[canonID].highScorePos, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].dir);
                                }
                            }
                            // 创建 喇叭.
//                            TrumpetCtrl.Instance.CreateOneTrumpet(canonID, serverMulti, fishDeadValue);

                            // 创建金币柱.
                            if (TotalCylinderCtrl.Instance.showCylider)
                            {
                                TotalCylinderCtrl.Instance.singleCylinderList[canonID].Add2Need2CreateList(serverMulti, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].cylinderShowUpPos.position);
                            }

                            // 恢复玩家发炮状态.
                            if (canNotPlayUntilRecycle)
                            {
                                CanonCtrl.Instance.AllowFire(true);
                            }

                            // 回收鱼.
                            RecycleFish();
                        }
                        break;
                    }
            }
        }

        // 黑洞死亡
        private void blackHallDead()
        {
            switch (specialFishDeadState)
            {
                // 移动到中间.
                case SpecialFishDeadState.drag2Center:
                    {
                        specialFishMovePercent += Time.deltaTime * move2CenterSpeed;
                        cur_Transform.position = Vector3.Lerp(move2CenterStartPos, Vector3.zero, specialFishMovePercent);
                        cur_Transform.localEulerAngles = Vector3.Lerp(move2CenterStartEuler, Vector3.zero, specialFishMovePercent);
                        if (specialFishMovePercent > 1f)
                        {
                            specialFishMovePercent = 0f;
                            specialFishDeadState = SpecialFishDeadState.wait;
                        }

                        // 连接玩家到黑洞的线.
                        if (blackHallLineRend != null)
                        {
                            //					Vector3 _pos0_InLockCam = LockCtrl.Instance.uiCamPos_to_LockCamPos(cur_Transform.position);
                            Vector3 _pos0_InLockCam = cur_Transform.position;
                            blackHallLineRend.SetPosition(0, _pos0_InLockCam);
                        }
                        break;
                    }
                // 吸鱼等待状态.
                case SpecialFishDeadState.wait:
                    break;

                // 回收.
                case SpecialFishDeadState.recycle:
                    {
                        // 播放死亡效果.
                        if (deadEffList.Count > 0)
                        {
                            FishDeadEffHandler.CreatedFishDeadEffCtrlItem(this, deadEffList);
                        }

                        // 鱼身上的数字.
                        // NumCtrl.Instance.CreateFishDeadNum(_value, m_traCache.position, Quaternion.identity);

                        // 金币.
                        CoinCtrl.Instance.CreateCoins(canonID, coinType, coinNum, cur_Transform.position, playCoinAudio);

                        // 如果不是当鱼死亡后就播放 high score， 就在这里播放.
                        if (!showHighScoreWhenFishDead)
                        {
                            if (highScorePrefab != null && fishDeadValue > 0)
                            {
                                HighScoreCtrl.Instance.Create(canonID, highScorePrefab, CanonCtrl.Instance.singleCanonList[canonID].highScorePos, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].dir);
                            }
                        }
                        // 喇叭.
//                        TrumpetCtrl.Instance.CreateOneTrumpet(canonID, serverMulti, fishDeadValue);

                        // 金币柱.
                        if (TotalCylinderCtrl.Instance.showCylider)
                        {
                            TotalCylinderCtrl.Instance.singleCylinderList[canonID].Add2Need2CreateList(multi, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].cylinderShowUpPos.position);
                        }

                        // 恢复玩家发炮状态.
                        if (canNotPlayUntilRecycle)
                        {
                            CanonCtrl.Instance.AllowFire(true);
                        }

                        // 回收黑洞的数字.
                        if (blackHallNumCreated != null)
                        {
                            Factory.Recycle(blackHallNumCreated);
                        }
                        // 回收黑洞的线.
                        if (blackHallLineCreated != null)
                        {
                            Factory.Recycle(blackHallLineCreated);
                        }
                        // 回收黑洞.
                        RecycleFish();
                        break;
                    }

            }

            // 黑洞的数字跟随着黑洞移动.
            if (blackHallNumCreated != null && blackHallNumCreated.gameObject.activeSelf)
            {
                blackHallNumCreated.localPosition = cur_Transform.localPosition + new Vector3(0f, blackHallHeight, 0f);
            }
        }

        // 打中鱼，更改颜色.
        public void ChangeColor()
        {
            if (!startHurt)
            {
                for (int i = 0; i < hurtRends.Length; i++)
                {
                    hurtRends[i].color = hurtColor;
                }
            }
            startHurt = true;
            hurtColorCancelTime = Time.time + hurtTimeLength;
        }

        // 鱼颜色更改后，过了一段时间恢复.
        private void DoHurtEff()
        {
            if (startHurt)
            {
                if (Time.time > hurtColorCancelTime)
                {
                    for (int i = 0; i < hurtRends.Length; i++)
                    {
                        hurtRends[i].color = orgColor;
                    }
                    startHurt = false;
                }
            }
        }

        // 鱼死亡.
        public override void KillByBullet(int _canonID, int _multi, int _power, int _npcType)
        {
            if (dead)
            {
                Debug.Log("Fish is dead, but someone still try to kill it. singleFish.cs - killByBullet() _canonID = " + _canonID);

                return;
            }

            // 关闭碰撞体，子弹就不会打到.
            cur_Collider.enabled = false;

            // 播放死亡动画.
			FishDeadAnim(cur_Animator.Length);
//            for (int i = 0; i < cur_Animator.Length; i++)
//            {
//                if (gameObject.activeSelf)
//                {
//                    //cur_Animator[i].SetTrigger(deadHash);
//                    cur_Animator[i].framesPerSecond = cur_Animator[i].framesPerSecond * 3;
//                }
//            }
            // 显示到最顶层.
            UISprite[] renderers = this.GetComponentsInChildren<UISprite>();
            for (int i = 0; i < renderers.Length; i++)
            {
                var item = renderers[i];
                item.depth = deadOrderInLayer;
                // 把组合鱼的底图设低层
//                 if (item.gameObject.tag == "fishs_bg")
//                 {
//                     item.depth = deadOrderInLayer - 2;
//                 }
            }
//            cur_SpriteRenderer.depth = deadOrderInLayer - 1;
            transform.localScale *= 1.2f;

            // 停止游动.
            StopMove();
            // 处理影子，影子也要停止游动，播放死亡动画等.
            HandleShadow();

            // 标志死亡.
            dead = true;
            // 取消锁定状态.
            locking = false;

            // 打死这条鱼的 玩家id.
            canonID = _canonID;
            power = _power;
            // 从服务器接收到这条鱼的倍率.
            serverMulti = _multi;
            // 鱼死亡的分值.
            fishDeadValue = _power * _multi;

            // 回收的时间.
            recycleTargetTime = Time.time + deadTimeLength;


            // 把这条鱼从记录鱼的缓存 list 中移除.
            if (!removeFromFishList)
            {
                FishCtrl.Instance.RemoveFishAndShadowInList(this);
                removeFromFishList = true;
            }

            // 死亡效果.
            NPCDeadEffect(_multi, _power);


            //创建线.
            switch (fishType)
            {
                // 一网打尽创建杀死鱼的线.
                case FishType.ywdj:
                    // 每次执行一网打尽时候,第一条鱼并没有累计进计数,所以强制累计一条鱼.(和其他方法无关)
                    //				CanonCtrl.Instance.shuliang +=1;
                    SpecialFishDeadHandler.Show_YWDJ_Lines(this, ywdjLine, canonID, ywdjKillFishPool, bulletCost, cur_Transform.position, deadTimeLength);
                    break;

                // 记录要杀死的全屏鱼.
                case FishType.quanping:
                    SpecialFishDeadHandler.Record_quanPing_KillFish(this);
                    break;

                // 黑洞创建连接到炮台的线.
                case FishType.blackHall:
                    if (blackHallLine != null)
                    {
                        blackHallLineCreated = Factory.Create(blackHallLine, Vector3.zero, Quaternion.identity);
                        blackHallLineRend = blackHallLineCreated.GetComponent<LineRenderer>();
                        // 炮台这一端.
                        //					Vector3 _pos0_InLockCam = LockCtrl.Instance.uiCamPos_to_LockCamPos(CanonCtrl.Instance.singleCanonList[_canonID].canonBarrelTrans.position);
                        Vector3 _pos0_InLockCam = CanonCtrl.Instance.singleCanonList[_canonID].canonBarrelTrans.position;
                        blackHallLineRend.SetPosition(1, _pos0_InLockCam);
                    }
                    break;
            }

            // 播放死亡音效.
            if (deadAudio != null)
            {
                if (Random.Range(0, 100) >= deadProbability)
                {
					//AudioSource.PlayClipAtPoint(deadAudio, Vector3.zero);
					if(gameObject.GetComponent<UIPlaySound>()!=null)
					{
						gameObject.GetComponent<UIPlaySound>().enabled = true;
					}
                }
            }

            // 如果是当鱼死亡后就播放 high score， 就在这里播放.
            if (showHighScoreWhenFishDead)
            {
                if (highScorePrefab != null && fishDeadValue > 0)
                {
                    HighScoreCtrl.Instance.Create(canonID, highScorePrefab, CanonCtrl.Instance.singleCanonList[canonID].highScorePos, fishDeadValue, CanonCtrl.Instance.singleCanonList[canonID].dir);
                }
            }

            // 如果是普通鱼，那么，当鱼死亡后就播放喇叭，high score 和 喇叭是同时出现的.
            if (fishDeadValue > 0)
            {
                switch (fishType)
                {
                    case FishType.normal:
                    case FishType.ywdj:
//                        TrumpetCtrl.Instance.CreateOneTrumpet(canonID, serverMulti, fishDeadValue);
                        break;
                }
            }

            // 记录杀死的鱼的类型. 显示到结算面板上.
            if (_npcType >= 1)
            {
                if (_canonID == CanonCtrl.Instance.realCanonID)
                {
                    CanonCtrl.Instance.KillOneFish(_canonID, _npcType - 1);
                    // 杀死鱼的分数乘以房间倍率除以币值,得到玩家真实赢到的金币数量.
                    fishDeadValue = fishDeadValue * CanonCtrl.Instance.roomMulti / CanonCtrl.Instance.oneGoldScore;
                    // 统计杀死鱼的数量和金币,并显示到account面板上
                    CanonCtrl.Instance.account_zongfen += fishDeadValue;
                    CanonCtrl.Instance.SetTempGoldAndBeansValue(CanonCtrl.Instance.account_zongfen, CanonCtrl.Instance.account_shuliang);
                }
            }

            // 锁定玩家发炮状态.
            if (canNotPlayUntilRecycle)
            {
                CanonCtrl.Instance.AllowFire(false);
            }

            // 震屏.
            if (shakeCam)
            {
                WaveCtrl.Instance.ShakeCamera();
				//15.12.14 增加手机震屏 ADa

//#if UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
				#if !UNITY_STANDALONE_WIN
				if(AudioCtrl.Instance.m_bIsOpen) Handheld.Vibrate();
#endif
            }
        }

        //NPC死亡特效
        private void NPCDeadEffect(int _multi, int _power)
        {
            // 死亡效果.
            switch (fishType)
            {
                // 全屏炸弹就初始化移动到屏幕中心的参数.
                case FishType.quanping:
                    {
                        specialFishMovePercent = 0f;
                        move2CenterStartPos = cur_Transform.position;
                        move2CenterStartEuler = cur_Transform.localEulerAngles;
                        move2CenterSpeed = 1f / move2CenterTimeLength;
                        specialFishDeadState = SpecialFishDeadState.drag2Center;
                        break;
                    }
                // 黑洞就初始化移动到屏幕中心的参数.
                case FishType.blackHall:
                    {
                        // 黑洞需要吸的倍率.
                        bh_need2AbsorbMulti = _multi;
                        // 黑洞需要吸的分值.
                        bh_need2AbsorbValue = _multi * _power;

                        specialFishMovePercent = 0f;
                        move2CenterStartPos = cur_Transform.position;
                        move2CenterStartEuler = cur_Transform.localEulerAngles;
                        move2CenterSpeed = 1f / move2CenterTimeLength;
                        specialFishDeadState = SpecialFishDeadState.drag2Center;
                        break;
                    }
                // 普通鱼就只要在这里播放死亡特效就可以了.
                default:
                    if (deadEffList.Count > 0)
                    {
                        FishDeadEffHandler.CreatedFishDeadEffCtrlItem(this, deadEffList);
                    }
                    break;
            }
        }

        // 被全屏炸弹炸死的鱼： 只能关闭碰撞体, 还不能设置dead = true, 如果设置为true了就会开始计时回收这条鱼, 必须等到处理全屏鱼杀死其他鱼的逻辑的时候才开始计时回收.
        public void KillByQuanping()
        {
            // 碰撞体关闭，不能被子弹打到.
            cur_Collider.enabled = false;

            // 播放死亡动画.
			FishDeadAnim(cur_Animator.Length);
//            for (int i = 0; i < cur_Animator.Length; i++)
//            {
//                //cur_Animator[i].SetTrigger(deadHash);
//                cur_Animator[i].framesPerSecond = cur_Animator[i].framesPerSecond * 3;
//            }
            // 显示到最高层.
            cur_SpriteRenderer.depth = deadOrderInLayer;

            // 停止移动.
            StopMove();

            // 处理影子.
            HandleShadow();
        }


        // 当波浪回收鱼的时候，如果是全屏或黑洞，要把剩余的分数加给玩家. 
        public void WaveForceRecycle()
        {
            if (fishType == FishType.quanping)
            {
                // 已经死亡 && 还没给分.
                if (dead && !specialFishHaveAddValue)
                {
                    CanonCtrl.Instance.singleCanonList[canonID].AddValueList(fishDeadValue, 0f);
                    specialFishHaveAddValue = true;
                    Debug.Log("ForceRecycle quanping  : canonID = " + canonID + " / dead = " + dead + " / specialFishHaveAddValue = " + specialFishHaveAddValue);
                }
            }
            else if (fishType == FishType.blackHall)
            {
                // 已经死亡 && 还没给分.
                if (dead && !specialFishHaveAddValue)
                {
                    // 剩余的分数.
                    int _leftValue = bh_need2AbsorbValue - bh_haveAbsorbValue;
                    CanonCtrl.Instance.singleCanonList[canonID].AddValueList(_leftValue, 0f);
                    specialFishHaveAddValue = true;
                    Debug.Log("ForceRecycle blackHall : canonID = " + canonID + " / dead = " + dead + " / specialFishHaveAddValue = " + specialFishHaveAddValue);
                }
            }
            else if (fishType == FishType.likui)
            {
                //李逵回收头上数字显示
                if (LikuiNumCreated != null)
                {
                    Factory.Recycle(LikuiNumCreated);
                }
            }
        }


        // 这条鱼死亡后是否需要播放金币声音。因为比如全屏或者一网打尽死亡后，会杀死其他鱼，如果这个时候其他鱼也播放死亡声音，那就很乱，所以，只要播放金币最多的那条鱼的金币声音就可以了.
        public void PlayCoinAudio(bool _play)
        {
            playCoinAudio = _play;
        }


        // 处理背影.
        void HandleShadow()
        {
            switch (fishType)
            {
                case FishType.quanping:
                case FishType.blackHall:
                    if (singleShadow != null)
                    {
                        singleShadow.StartLerpShadow2Center(move2CenterTimeLength);
                    }
                    break;
                default:
                    if (singleShadow != null)
                    {
                        singleShadow.KillByBullet();
                    }
                    break;
            }
        }


        // 被黑洞吸收的鱼：关闭碰撞体，播放死亡动画，以及告诉这条鱼，是被哪个黑洞杀死. 
        public void AbosorbByBlackHall(bool _blackHallIsDead, Transform _blackHall, int _canonID)
        {
            canonID = _canonID;

            locking = false;
            dead = true;

            // 标志是被 black hall 杀死的.
            killByBlackHall = true;
            blackHall = _blackHall;

            // 关闭碰撞体，播放死亡动画.
            cur_Collider.enabled = false;
			FishDeadAnim(cur_Animator.Length);
//            for (int i = 0; i < cur_Animator.Length; i++)
//            {
//                //cur_Animator[i].SetTrigger(deadHash);
//                cur_Animator[i].framesPerSecond = cur_Animator[i].framesPerSecond * 3;
//            }
            cur_SpriteRenderer.depth = deadOrderInLayer;

            // 停止移动.
            StopMove();

            // 影子.
            if (singleShadow)
            {
                singleShadow.AbosorbByBlackHall(blackHall);
            }

            // 从缓存list 移除这条鱼.
            if (!removeFromFishList)
            {
                FishCtrl.Instance.RemoveFishAndShadowInList(this);
                removeFromFishList = true;
            }

            // 播放声音.
            if (deadAudio != null)
            {
                if (Random.Range(0, 100) >= deadProbability)
                {
//                    AudioSource.PlayClipAtPoint(deadAudio, Vector3.zero);
					if(gameObject.GetComponent<UIPlaySound>()!=null)
					{
						gameObject.GetComponent<UIPlaySound>().enabled = true;
					}
                }
            }

            // 黑洞是死的，才创建金币.
            if (_blackHallIsDead)
            {
                CoinCtrl.Instance.CreateCoins(canonID, coinType, coinNum, cur_Transform.position, playCoinAudio);
            }
        }

        // 被黑洞吸的鱼： 被黑洞吸的效果.
        void DidBlackHallAbsorbEffAndRecycle()
        {
            if (killByBlackHall)
            {
                lerp2BlackHallPercent += Time.deltaTime * 0.125f;
                cur_Transform.localEulerAngles += new Vector3(0f, 0f, 5f);
                cur_Transform.RotateAround(blackHall.position, Vector3.forward, -4f);
                cur_Transform.localScale = Vector3.Lerp(cur_Transform.localScale, Vector3.zero, lerp2BlackHallPercent);
                if (blackHall != null)
                {
                    cur_Transform.position = Vector3.Lerp(cur_Transform.position, blackHall.position, lerp2BlackHallPercent);
                }

                // 被吸到最小了，就回收.
                if (lerp2BlackHallPercent >= 1f)
                {
                    RecycleFish();
                }
            }
        }

        // 黑洞：每吸收一条鱼就给玩家加分.
        public void bh_AbosorbOneFish(int _blackHallPower, int _absorbMulti, Vector3 _pos)
        {
            bh_need2AbsorbMulti -= _absorbMulti;
            int _tempMulti = _absorbMulti;
            if (bh_need2AbsorbMulti <= 0)
            {
                // 判断是否已经吸够了.
                if (bh_need2AbsorbMulti < 0)
                {
                    // 吸收够了，就吐出多余的部分.
                    _tempMulti += bh_need2AbsorbMulti;
                }
                // 标示已经吸够分数了.
                specialFishDeadState = SpecialFishDeadState.recycle;
            }

            // 当前吸收这条鱼的分数.
            int _value = _blackHallPower * _tempMulti;

            // 给玩家吸收了这条鱼的分数.
            CanonCtrl.Instance.singleCanonList[canonID].AddValueList(_value, 0f);

            // 黑洞头顶上的分数.
            if (blackHallNumPrefab != null && (blackHallNumCreated == null || !blackHallNumCreated.gameObject.activeSelf))
            {
                blackHallNumCreated = Factory.Create(blackHallNumPrefab, Vector3.zero, Quaternion.identity);
                blackHallNumCreated.parent = cur_Transform.parent;
                blackHallNumCreated.localPosition = cur_Transform.localPosition + new Vector3(0f, blackHallHeight, 0f);
                blackHallNumCreated.localEulerAngles = Vector3.zero;
                blackHallNumCreated.localScale = blackHallScale;
                blackHallNum = blackHallNumCreated.GetComponent<NumItem>();
                bh_haveAbsorbValue = 0;
            }
            bh_haveAbsorbValue += _value;
            if (blackHallNum != null)
            {
                blackHallNum.ApplyValue(bh_haveAbsorbValue, 0);
            }

            // 鱼身上的分数.
            NumCtrl.Instance.CreateFishDeadNum(_value, _pos, Quaternion.identity);

            // bubble.
            NumCtrl.Instance.AdddBubble2List(_tempMulti,
                                             _value,
                                             CanonCtrl.Instance.singleCanonList[canonID].bubbleShowUpPos.position,
                                             Quaternion.identity, CanonCtrl.Instance.singleCanonList[canonID].upsideDown
                                             );
            // 开始回收黑洞，不再吸鱼.
            if (specialFishDeadState == SpecialFishDeadState.recycle)
            {
                specialFishHaveAddValue = true;
            }
        }

        // 停止移动.
        void StopMove()
        {
            navigation.forcePathEnd();
        }

        // 回收 fish.
        void RecycleFish()
        {
			// 回收鱼的时候,把声效关闭.
			if(gameObject.GetComponent<UIPlaySound>() != null) gameObject.GetComponent<UIPlaySound>().enabled = false;
            //李逵回收头上数字显示
            if (fishType == FishType.likui)
            {
                if (LikuiNumCreated != null)
                {
                    Factory.Recycle(LikuiNumCreated);
                }
            }
            locking = false;

            if (!removeFromFishList)
            {
                FishCtrl.Instance.RemoveFishAndShadowInList(this);
                removeFromFishList = true;
            }

            // recycle shadow.
            if (singleShadow != null)
            {
                Factory.Recycle(singleShadow.transform);
            }

            // recycle fish.
            Factory.Recycle(cur_Transform);

            cur_Transform.position = new Vector3(100f, 100f, 0f);
        }

        void Start()
        {
            //RaycastHit2D hit = Physics2D.Raycast(UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }

		void FishDeadAnim(int num){
			for (int i = 0; i < num; i++)
			{
				if (gameObject.activeSelf)
				{
					cur_Animator[i].framesPerSecond = cur_Animator[i].framesPerSecond * 8;
				}
			}
		}

    }
}