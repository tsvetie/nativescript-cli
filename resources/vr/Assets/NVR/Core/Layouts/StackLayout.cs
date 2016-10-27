using Assets.NVR.Interfaces.Elements;
using Assets.NVR.Interfaces.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Core.Layouts
{
    public class StackLayout : Layout, IStackLayout
    {
        public float topPosition = 350f;

        public StackLayout(GameObject container)
            : base(container)
        {

        }

        public void AddElement(IElement element)
        {
            element.SetParent(this.Container);
            element.SetPosition(this.GetNextElementPosition(element.Heigth));

            this.elements.Add(element);
        }


        public void SetParent(GameObject parent)
        {

        }

        private float GetNextElementPosition(float elementHeigth)
        {
            var currentHeigth = this.topPosition - this.Heigth;

            return currentHeigth - elementHeigth * 2;
        }
    }
}
