using Assets.NVR.Core.Elements;
using Assets.NVR.Interfaces;
using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Core
{
    public class ElementsFactory : MonoBehaviour, IElementsFactory
    {
        public GameObject ButtonPrefab;
        public GameObject LabelPrefab;

        public IButton CreateButton(string text)
        {
            return new Button(this.ButtonPrefab, text);
        }

        public ILabel CreateLabel(string Text)
        {
            return new Label(this.LabelPrefab, Text);
        }
    }
}
