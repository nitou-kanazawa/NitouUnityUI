using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// [参考]
//  LIGHT11: マルチ解像度対応に必須なCanvas Scalerを目的別に理解する https://light11.hatenadiary.com/entry/2019/04/21/222337
//  LIGHT11: uGUIの描画順の制御方法をちゃんと理解する https://light11.hatenadiary.com/entry/2019/08/21/000713
//  ねこじゃらシティ: RectTransform.sizeDeltaの仕様と注意点 https://nekojara.city/unity-rect-transform-size-delta

namespace nitou.UI {
    using nitou.UI.Overlay;
    //using nitou.Settings;

    /// <summary>
    /// オーバーレイを操作するための
    /// </summary>
    public static class UIOverlay {

        private static OverlayContainer _container;

        public static bool IsInitialized { get; private set; } = false;


        /// ----------------------------------------------------------------------------
        // Public Method

        public static void RuntimeInitialize() {
            if (IsInitialized) return;
            
            CreateOverlayCanvas();

            _container.Load<OverlayBase>("Overlay/Prefab_SimplaFade_Overlay");

            IsInitialized = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Load(string resourceKey) {
            _container.Load<OverlayBase>(resourceKey);
        }

        /// <summary>
        /// 
        /// </summary>
        public static UniTask OpenAsync(float duration = 1f) {
            if (!_container.HasOverlay) return UniTask.CompletedTask;
            return _container.Current.OpenAsync(duration);
        }

        /// <summary>
        /// 
        /// </summary>
        public static UniTask CloseAsync(float duration = 1f) {
            if (!_container.HasOverlay) return UniTask.CompletedTask;
            return _container.Current.CloseAsync(duration);
        }


        /// ----------------------------------------------------------------------------
        // Private Method

        private static void CreateOverlayCanvas() {

            // Canvas
            //var sortingOrder = ProjectSettingsSO.Instance.OverlayCanvasSortingOrder;
            var sortingOrder = 100;
            var canvas = UIHelper.CreateCanvas("[Overlay Canvas]", sortingOrder);
            canvas.gameObject.DontDestroyOnLoad();

            // 
            var childRect = new GameObject("Container").AddComponent<RectTransform>();
            childRect.SetParent(canvas.transform, false);
            childRect.SetRectBasedOnParentEdiges(0, 0, 0, 0);

            // Container
            _container = childRect.AddComponent<OverlayContainer>();
        }
    }
}
