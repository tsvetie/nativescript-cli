using Assets.NVR.Interfaces.Elements;
using Assets.NVR.Interfaces.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Core.Layouts
{
    public abstract class Layout : ILayout
    {
        public GameObject Container { get; private set; }
        protected IList<IElement> elements;

        public float Width
        {
            get
            {
                return this.elements.Select(x => x.Width).Sum();
            }
        }

        public float Heigth
        {
            get
            {
                return this.elements.Select(x => x.Heigth).Sum();
            }
        }


        public Layout(GameObject container)
        {
            this.Container = container;
            this.elements = new List<IElement>();
        }


        public void AddElement(IElement element)
        {
            throw new NotImplementedException();
        }


        public void SetParent(GameObject parent)
        {
            throw new NotImplementedException();
        }
    }
}
