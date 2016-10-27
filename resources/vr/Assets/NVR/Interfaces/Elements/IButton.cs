using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Assets.NVR.Interfaces.Elements
{
    public interface IButton : IElement
    {
        string Text { get; set; }

        void AddOnClick(Action onClicked);
    }
}
