using UnityEngine;

namespace nocturne
{
    public class Loader : MonoBehaviour
    {
        public static GameObject load;

        public static void Init()
        {
            if (load == null)
            {
                load = new GameObject();

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
