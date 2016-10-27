//using Assets.NVR.Core;
//using Assets.NVR.Core.Elements;
//using Assets.NVR.Core.Layouts;
//using Assets.NVR.Interfaces;
//using Assets.NVR.Interfaces.Elements;
//using Assets.NVR.Interfaces.Layouts;
//using Assets.Scripts.Pages;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UniRx;
//using UnityEngine.VR;

//namespace Assets.Scripts
//{
//    public class Bootstrap : MonoBehaviour
//    {
//        public GameObject MainPage;

//        void Start()
//        {
//            VRSettings.renderScale = 1.5f;
//            IElementsFactory elementsFactory =  this.GetComponent<ElementsFactory>();

//            HelloWorldModel vm = new HelloWorldModel();

//            IPage page = new Page(this.MainPage);
//            IStackLayout layout = new StackLayout(this.MainPage);

//            ILabel label = elementsFactory.CreateLabel("Tap here to continue");
//            layout.AddElement(label);

//            IButton button = elementsFactory.CreateButton("Some text 2");

//            // text binding
//            vm.message.Subscribe((x) =>
//            {
//                label.Text = vm.message.Value;
//            });

//            // tap binding
//            button.AddOnClick(vm.onTap);

//            layout.AddElement(button);
//        }
//    }
//}
using Assets.NVR.Core;
using Assets.NVR.Core.Elements;
using Assets.NVR.Core.Layouts;
using Assets.NVR.Interfaces;
using Assets.NVR.Interfaces.Elements;
using Assets.NVR.Interfaces.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using UnityEngine.VR;
using Assets.Scripts.Pages;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        public GameObject MainPage;

        void Start()
        {
            VRSettings.renderScale = 1.5f;
            IElementsFactory elementsFactory = this.GetComponent<ElementsFactory>();

            HelloWorldModel vm = new HelloWorldModel();
            IPage page173 = new Page(this.MainPage);
            IStackLayout stackLayout614 = new StackLayout(this.MainPage);
            ILabel label900 = elementsFactory.CreateLabel("Tap the button");

            stackLayout614.AddElement(label900);
            ILabel label151 = elementsFactory.CreateLabel("{{ message }}");

            vm.message.Subscribe((x) =>
            {
                label151.Text = vm.message.Value;
            });
            stackLayout614.AddElement(label151);

            IButton button342 = elementsFactory.CreateButton("TAP");

            button342.AddOnClick(vm.onTap);
            stackLayout614.AddElement(button342);


        }
    }
}
