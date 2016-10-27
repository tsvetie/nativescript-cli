using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.NVR.Interfaces.Elements
{
    public interface ILabel : IElement
    {
        string Text { get; set; }
    }
}
