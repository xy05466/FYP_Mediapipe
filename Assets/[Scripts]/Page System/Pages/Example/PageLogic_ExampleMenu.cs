using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe.Example
{
    public class PageLogic_ExampleMenu : PageLogic
    {
        public Button bu_next;
        private bool done = false;
        public override void Initialize()
        {
            bu_next.onClick.AddListener(() => done = true);
        }
        public override void StartPage()
        {
            done = false;
        }
        public override bool IsPageDone() => done;


    }
}