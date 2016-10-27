using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Interfaces.Layouts
{
    public interface ILayout : IUnit
    {
        GameObject Container { get; }
        void AddElement(IElement element);
    }
}
