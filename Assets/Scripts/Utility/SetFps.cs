using UnityEngine;
using UnityEngine.Rendering;

namespace RevivalofNature.Core
{
    /// <summary>
    ///FPSセッティング用最初のシーンで使用
    /// </summary>
    public class SetFps : MonoBehaviour
    {
        public int fps = 60;

        /// <summary>
        /// Unity's Start method.
        /// </summary>
        private void Start()
        {
            #if UNITY_EDITOR
                fps = 60;
            #else
                fps = 60;
                OnDemandRendering.renderFrameInterval = 3;
            #endif
            Application.targetFrameRate = fps;
        }
    }
}