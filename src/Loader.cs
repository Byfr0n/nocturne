using UnityEngine;
using nocturne;

namespace nocturne
{
    public class Loader : MonoBehaviour
    {
        public static GameObject load;

        public static string version = "1.0";
        public static void Init()
        {
            if (load == null)
            {
                load = new GameObject();

                load.AddComponent<MenuManager>();

                load.AddComponent<ExampleModule>();

                DontDestroyOnLoad(load);
            }
        }

        public static void Unload()
        {
            if (load != null)
            {
                Destroy(load);
            }
        }
    }
}
