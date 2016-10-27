using Assets.NVR.Core.Utils;
using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRStandardAssets.Utils;

namespace Assets.NVR.Core.Elements
{
    class Button : Element, IButton
    {
        public string Text
        {
            get
            {
                return this.element.GetComponentInChildren<Text>().text;
            }
            set
            {
                this.element.GetComponentInChildren<Text>().text = value;
            }
        }

        public float Width
        {
            get
            {
                return this.button.GetComponent<Renderer>().bounds.size.y;
            }
        }

        public float Heigth
        {
            get
            {
                return 20f;
                //return this.element.GetComponent<Renderer>().bounds.size.z;
            }
        }

        private Transform button;

        public Button(GameObject prefab, string text)
        {
            this.element = (GameObject)UnityEngine.Object.Instantiate(prefab);
            this.button = this.element.transform.FindChild("El");

            this.Text = text;
        }

        public void SetColor(string color)
        {
        }

        public void AddOnClick(Action onClicked)
        {
            this.button.GetComponent<Clickable>().AddClickHandler(onClicked);
        }

        public void SetPosition(float pos)
        {
            this.element.transform.localPosition = new Vector3(0, pos, 0);
        }
    }
}
