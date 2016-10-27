using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Core.Elements
{
    public abstract class Element : IElement
    {
        protected GameObject element;

        public float Width
        {
            get
            {
                return this.element.GetComponent<Renderer>().bounds.size.y;
            }
        }

        public float Heigth
        {
            get
            {
                return this.element.GetComponent<Renderer>().bounds.size.z;
            }
        }

        public void SetParent(GameObject parent)
        {
            this.element.transform.SetParent(parent.transform, false);
        }

        public void SetColor(string color)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(float pos)
        {
            this.element.transform.position = new Vector3(0, 0, pos);
        }
    }
}
