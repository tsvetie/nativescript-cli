using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRStandardAssets.Utils;

namespace Assets.NVR.Core.Utils
{
    public class Clickable : MonoBehaviour
    {
        [SerializeField]
        private VRInteractiveItem vrInteractiveItem;

        public void OnEnable()
        {
            this.vrInteractiveItem.OnOver += this.OnGazeOver;
            this.vrInteractiveItem.OnOut += this.OnGazeOut;
            this.vrInteractiveItem.OnUp += this.OnGaseUp;
            this.vrInteractiveItem.OnDown += this.OnGaseDown;
        }

        public void AddClickHandler(Action onClick)
        {
            this.vrInteractiveItem.OnClick += onClick;
        }

        private void OnGazeOver()
        {
            LeanTween.cancel(this.transform.parent.gameObject);
            LeanTween.moveLocalZ(this.transform.parent.gameObject, -20f, 0.2f);
        }

        private void OnGazeOut()
        {
            LeanTween.cancel(this.transform.parent.gameObject);
            LeanTween.moveLocalZ(this.transform.parent.gameObject, 0f, 0.2f);
        }

        private void OnGaseUp()
        {
            LeanTween.cancel(this.transform.parent.gameObject);
            LeanTween.moveLocalZ(this.transform.parent.gameObject, 0f, 0.1f);
        }

        private void OnGaseDown()
        {
            LeanTween.cancel(this.transform.parent.gameObject);
            LeanTween.moveLocalZ(this.transform.parent.gameObject, 10f, 0.1f);
        }
    }
}
