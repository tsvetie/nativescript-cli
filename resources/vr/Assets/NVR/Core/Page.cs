using Assets.NVR.Interfaces;
using Assets.NVR.Interfaces.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Core
{
    public class Page : IPage
    {
        public GameObject Container { get; private set; }

        private IList<ILayout> Layouts { get; set; }

        public Page(GameObject container)
        {
            this.Container = container;
            this.Layouts = new List<ILayout>();
        }

        public void AddLayout(ILayout layout)
        {
            layout.SetParent(this.Container);
            this.Layouts.Add(layout);
        }
    }
}
