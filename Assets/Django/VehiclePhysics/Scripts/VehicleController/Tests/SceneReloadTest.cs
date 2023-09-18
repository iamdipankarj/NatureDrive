using UnityEngine;
using UnityEngine.SceneManagement;

namespace Django.VehiclePhysics.Tests
{
    /// <summary>
    ///     Reloads scene each 10 seconds.
    /// </summary>
    public partial class SceneReloadTest : MonoBehaviour
    {
        private void Update()
        {
            if (Time.timeSinceLevelLoad > 10)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}