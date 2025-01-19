using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace nocturne
{
    public class Utils : MonoBehaviour
    {
        public static Dictionary<Color, Texture2D> colorTextures = new Dictionary<Color, Texture2D>(); // cached

        public static Texture2D GetColorTexture(Color color)
        {
            if (colorTextures.ContainsKey(color))
                return colorTextures[color];
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            colorTextures[color] = texture;
            return texture;
        }
    }
}
