using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Interfaces
{
    public interface IUnit
    {
        float Width { get; }
        float Heigth { get; }

        void SetParent(GameObject parent);
    }
}
