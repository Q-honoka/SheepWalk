using UnityEngine;

namespace SmoothigTransform
{
    public class RotateBaum : MonoBehaviour
    {
        // •Ï”éŒ¾
        public Vector3 TargetPosition;
        public Vector3 TargetScale;
        public Quaternion TargetRotation;
        public float TimeFact { set; get; } = 0.15f;

        public void Start()
        {
            // ‰Šú‰»ˆ—
            TargetPosition = transform.localPosition;
            TargetScale = transform.localScale;
            TargetRotation = transform.localRotation;
        }

        public void Update()
        {
            //TimeFact•b‚Å¡‚¢‚éêŠ‚©‚ç1/10‚Ü‚ÅŠÔ‚ğ‹l‚ß‚é‚½‚ß‚Ì’l
            var t = 1 - Mathf.Pow(0.1f, Time.deltaTime / TimeFact);
            transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, t);
            transform.localScale = Vector3.Lerp(transform.localScale, TargetScale, t);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, TargetRotation, t);
        }

        public void SetTargetRotation(int dir)
        {
            // ‰ñ“]•ûŒü‚ğİ’è
            TargetRotation *= Quaternion.Euler(0, 0, 90 * dir);

        }
    }
}