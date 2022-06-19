using UnityEngine;
using UnityEngine.Tilemaps;

class ColorfulPlatform : MonoBehaviour
{
        [SerializeField]
        private Color color;
        
        private static float increasedSpeed = 10F;
        private static float increasedJumpforce = 15F;

        private Witch witch;

        private void Update()
        {
            color = GetComponent<Tilemap>().color;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                witch = other.GetComponent<Witch>();
                witch.leftColorfulPlatform = false;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                witch = other.GetComponent<Witch>();
                witch.ResetStats();
                if (color.Equals(ColorPalette.colors[1]))
                {
                    witch.speed = increasedSpeed;
                }
                else if (color.Equals(ColorPalette.colors[2]))
                {
                    witch.jumpforce = increasedJumpforce;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                witch = other.GetComponent<Witch>();
                witch.leftColorfulPlatform = true;
            }
        }
}
