using System;
using com.QH.QPGame.Lobby.Surfaces;

namespace com.QH.QPGame.Lobby
{
    /// <summary>
    /// 弹出窗口管理器,提供全局统一的几种模态对话框,减少累人的与UI层来回通信
    /// TODO 层次管理
    /// @Author: guofeng
    /// </summary>
    public class PopupManager
    {
        public void ShowFormatTips(string msg, params object[] args)
        {
            string str = string.Format(msg, args);
            ShowTips(str, 0.0f, null);
        }

        public void ShowTips(string message)
        {
            ShowTips(message, 0.0f, null);
        }

        public void ShowTips(string message, float _time, Action _callback)
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var tips = container.GetSurface<SurfaceTips>();
            tips.Show(message, _time, _callback);
        }

        public bool IsTipsShown()
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var tips = container.GetSurface<SurfaceTips>();
            return tips.IsShown();
        }

        public void HideTips()
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var tips = container.GetSurface<SurfaceTips>();
            tips.Hide();
        }

        public bool IsMsgBoxShown()
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var msgBox = container.GetSurface<MessageBoxPopup>();
            return msgBox.IsShown();
        }

        public void MsgBox(string title, string message, ButtonStyle _ButtonStyle, MessageBoxCallback2 _callback = null,
                           float _fWaitTime = 5.0f)
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var msgBox = container.GetSurface<MessageBoxPopup>();
            msgBox.Show(title, message, _ButtonStyle, _callback, _fWaitTime);
        }

        public void Confirm(string title, string message, MessageBoxCallback2 _callback = null, float _fWaitTime = 5.0f)
        {
            var container = UIRoot.list[0].gameObject.GetComponent<SurfaceContainer>();
            var msgBox = container.GetSurface<MessageBoxPopup>();
            msgBox.Confirm(title, message, _callback, _fWaitTime);
        }
    }
}