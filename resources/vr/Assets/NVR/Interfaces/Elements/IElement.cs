using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Interfaces.Elements
{
    public interface IElement : IUnit
    {
        void SetColor(string color);
        void SetPosition(float pos);
    }
}
