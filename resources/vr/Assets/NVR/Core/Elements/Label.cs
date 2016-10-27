using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.NVR.Core.Elements
{
    public class Label : Element, ILabel
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
                return 20f;
            }
        }

        public float Heigth
        {
            get
            {
                return 30f;
                //return this.element.GetComponent<Renderer>().bounds.size.z;
            }
        }

        public Label(GameObject prefab, string text)
        {
            this.element = (GameObject)UnityEngine.Object.Instantiate(prefab);

            this.Text = text;
        }

        public void SetPosition(float pos)
        {
            this.element.transform.localPosition = new Vector3(0, pos, 0);
        }
    }
}
