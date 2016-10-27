using Assets.NVR.Interfaces.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NVR.Interfaces
{
    public interface IPage
    {
        GameObject Container { get; }

        void AddLayout(ILayout layout);
    }
}
