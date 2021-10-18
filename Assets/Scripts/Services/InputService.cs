using UnityEngine;

namespace Services
{
    public static class InputService
    {
        public static bool IsTouchOn<T>(T component, Camera camera)
        {
            if (!Input.GetMouseButtonDown(0)) return false;

            var screenPosition = Input.mousePosition;
            screenPosition.z = camera.nearClipPlane;

            var position = camera.ScreenToWorldPoint(screenPosition);
            var collider = Physics2D.OverlapPoint(position);

            return !(collider is null) && collider.gameObject.GetComponent<T>().Equals(component);
        }
    }
}