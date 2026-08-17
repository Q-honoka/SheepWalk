using UnityEngine;

namespace SmoothigTransform
{
    public class RotateBaum : MonoBehaviour
    {
        // •Ï”éŒ¾
        public Quaternion TargetRotation;
        public float TimeFact { set; get; } = 0.15f;    // ‰ñ“]‚É‚©‚¯‚éŠÔ

        public void Start()
        {
            TargetRotation = transform.localRotation;
        }

        public void Update()
        {
            //TimeFact•b‚Å¡‚¢‚éêŠ‚©‚ç1/10‚Ü‚ÅŠÔ‚ğ‹l‚ß‚é‚½‚ß‚Ì’l
            var t = 1 - Mathf.Pow(0.1f, Time.deltaTime / TimeFact);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, TargetRotation, t);
        }

        /// <summary>
        /// ‰ñ“]‚ğİ’è‚·‚é
        /// </summary>
        /// <param name="dir">‰ñ‚·Šp“x</param>
        public void SetTargetRotation(int dir)
        {
            // ‰ñ“]•ûŒü‚ğİ’è
            TargetRotation *= Quaternion.Euler(0, 0, 90 * dir);

        }
    }
}