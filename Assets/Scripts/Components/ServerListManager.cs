using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.GameUtils;
using com.QH.QPGame.Utility;
using UnityEngine;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 服务器列表管理器
    /// 向cdn WEB服务器请求服务器列表
    /// 策略1，随机连接一个域名，当连接失败，则再次随机直到连接成功或者全部失败
    /// 策略2，同时发起返回的ip列表ping测试延迟，在最长等待时间内获取延迟最低的域名并连接
    /// @Author: guofeng
    /// </summary>
    public class ServerListManager : MonoBehaviour
    {
        private class ServerNode
        {
            public ServerNode(string addr)
            {
                Address = addr;
            }

            public string Address;
            public float PingStart;
            public float PingEnd;
            public float Delay;
            //public int Weight;
            public Ping Ping;
        }

        private List<ServerNode> mNodes;
        private int mRetryCnt = 0;

        public bool Ready = false;
        public bool Error = false;

        private string _statusStr = null;
        public string StatusStr
        {
            get { return _statusStr; }
            private set
            {
                _statusStr = value;
            }
        }

        public ServerListManager()
        {
            mNodes = new List<ServerNode>();
        }

        public void Clear()
        {
            StopAllCoroutines();
            Ready = false;
            Error = false;
            _statusStr = null;
            mRetryCnt = 0;
            mNodes.Clear();
        }

        public void Initialize()
        {
            
        }

        public void LoadServerList(string url)
        {
            StopAllCoroutines();
            StartCoroutine(StartLoadServerList(url));
        }

        private IEnumerator StartLoadServerList(string url)
        {
            StatusStr = string.Format("开始加载服务器列表 {0}/{1}", mRetryCnt, GlobalConst.Update.RetryTimes);
            Logger.Net.Log(StatusStr);
            
            bool timeout = false;
            float elapsedTime = 0.0f;
            var www = new WWW(url);
            yield return www;
            while (!www.isDone)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= GlobalConst.Update.Timeout)
                {
                    timeout = true;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }

            if (!string.IsNullOrEmpty(www.error) || timeout)
            {
                if (mRetryCnt++ < GlobalConst.Update.RetryTimes)
                {
                    float fWaitTime = mRetryCnt * 2 + 1;

                    StatusStr = string.Format("加载列表失败,Err:{0}，{1}秒后重试", 
                        timeout?"超时":www.error,
                        (int)fWaitTime);
                    Logger.Net.Log(StatusStr);

                    yield return new WaitForSeconds(fWaitTime);
                    yield return StartCoroutine(StartLoadServerList(url));
                }
                else
                {
                    Error = true;
                    StatusStr = string.Format("加载列表失败,Err:{0}", timeout ? "超时" : www.error);
                    Logger.Net.LogError(StatusStr);
                }
                yield break;
            }

            var decText = www.text;

#if !DISABLE_ENCRYPT_FILES && !UNITY_EDITOR
            decText = DESEncryption.Decrypt(decText, GlobalConst.Res.EncryptPassword);
#endif
            var strs = decText.Split(',');
            if (strs.Length > 1)
            {
                StatusStr = string.Format("列表获取成功，可用节点数:{0}，开始检测网络", strs.Length);
                Logger.Net.Log(StatusStr);

                foreach (var str in strs)
                {
                    mNodes.Add(new ServerNode(str));
                }

                PingAll();
            }
        }

        private void Update()
        {
            if (!Ready)
            {
                CheckPing();
            }
        }

        private void PingAll()
        {
            foreach (var node in mNodes)
            {
                node.Ping = new Ping(node.Address);
            }
        }

        private void CheckPing()
        {
            int doneCnt = 0;
            foreach (var serverNode in mNodes)
            {
                if (serverNode.Ping != null && serverNode.Ping.isDone)
                {
                    if (++doneCnt == mNodes.Count)
                    {
                        foreach (var serverNode2 in mNodes)
                        {
                            if (serverNode2.Ping != null)
                            {
                                serverNode2.Delay = serverNode2.Ping.time < 0 ? 999999 : serverNode2.Ping.time;
                                serverNode2.Ping.DestroyPing();
                                serverNode2.Ping = null;
                            }
                        }
                        mNodes.Sort((x, y) => { return (int)(x.Delay - y.Delay); });
                        Ready = true;
                        StatusStr = string.Format("检测网络完成，准备连接");
                        Logger.Net.Log(StatusStr);
                        break;
                    }
                }
            }
        }

        public string PickALowestDelayServer()
        {
            if (mNodes.Count == 0 || !Ready)
            {
                return "";
            }

            var item = mNodes[0];
            return item.Address;
        }

    }
}

