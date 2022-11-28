using Overspace.GUI.View;
using Overspace.Pattern.Singleton;
using TMPro;
using UnityEngine.UI;

namespace Overspace.GUI
{
    public class GUIManager : MonoBehaviourSingleton<GUIManager>
    {
        public OrderView orderView;
        public Button backButton;
        public Button serveButton;
        public TextMeshProUGUI tipCountText;
        public TextMeshProUGUI helpText;
        public Button rightButton;
        public Button leftButton;
        public Image reaction;
    }
}