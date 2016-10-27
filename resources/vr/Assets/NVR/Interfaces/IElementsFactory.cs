using Assets.NVR.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.NVR.Interfaces
{
    public interface IElementsFactory
    {
        IButton CreateButton(string Text);
        ILabel CreateLabel(string Text);
    }
}
