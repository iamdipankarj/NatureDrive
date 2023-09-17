using UnityEngine;
using UnityEngine.UI;

namespace NSVehicle
{
    public class DemoWelcomeMessage : MonoBehaviour
    {
        public GameObject welcomeMessageGO;
        public Button closeButton;


        private void Start()
        {
            if (!Application.isEditor)
            {
                welcomeMessageGO.SetActive(true);
            }

            closeButton.onClick.AddListener(Close);
        }


        private void Close()
        {
            welcomeMessageGO.SetActive(false);
        }
    }
}