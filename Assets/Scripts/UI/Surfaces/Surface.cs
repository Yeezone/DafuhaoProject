using System.Collections.Generic;
using UnityEngine;
using com.QH.QPGame.Services.Data;

namespace com.QH.QPGame.Lobby.Surfaces
{
    /// <summary>
    /// TODO 扩展为基于引用技术来管理
    /// </summary>
    public class Surface : MonoBehaviour
    {
        public string SurfaceName;

        public List<GameObject> ConfigUIList;

        public virtual void ApplyConfig(GameUIConfig[] configs)
        {
            foreach (var config in configs)
            {
                var obj = ConfigUIList.Find(item => item.name == config.Name);
                if (obj != null)
                {
                    obj.SetActive(config.Activation);
                }
            }
        }

        public virtual void Show()
        {
            NGUITools.SetActive(gameObject, true);
        }

        public virtual void Hide()
        {
            NGUITools.SetActive(gameObject, false);
        }

    }

}

