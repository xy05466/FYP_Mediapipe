using UnityEngine;
using UnityEngine.UI;

namespace SDM_FythmPipe
{
    public class PageLogic_StartMenu : PageLogic
    {
        public GameObject panel;
        public Button bu_start;
        bool pageEnd = false;


        public override void Initialize()
        {
            bu_start.onClick.AddListener(() => { pageEnd = true; });
        }
        public override void StartPage()
        {
            panel.gameObject.SetActive(true);
            pageEnd = false;
        }
        public override void EndPage()
        {
            panel.gameObject.SetActive(false);
        }

        public override bool IsPageDone() => pageEnd;

    }
}