using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class ColorfulPlatform : MonoBehaviour
{
        [SerializeField]
        private Color color;
        [SerializeField]
        private ColorPalette palette;
        [SerializeField]
        static float increasedSpeed = 10f;
        [SerializeField]
        static float increasedJumpforce = 15f;

        private List<Color> colors;

        private void Start()
        {
            colors = palette.colors;
            color = GetComponent<Tilemap>().color;
        }

        void AAAAA(Collision2D other)
        {
        }
}
