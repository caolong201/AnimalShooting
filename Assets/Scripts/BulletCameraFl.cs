using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IE.RSB
{
    public class BulletCameraFl : MonoBehaviour
    {
        [SerializeField] private Transform m_bullet;           // The bullet to follow
        [SerializeField] private Transform m_cameraTransform;   // The camera following the bullet
        [SerializeField] private float m_cameraDistance = 2.5f; // Increased distance to avoid being too close to bullet
        [SerializeField] private float m_cameraHeightOffset = 0.3f; // Raise camera to avoid grass and ground

        private void LateUpdate()
        {
            if (m_bullet == null || m_cameraTransform == null)
                return;

            // Calculate the offset from the bullet position
            Vector3 offset = -m_bullet.forward * m_cameraDistance + Vector3.up * m_cameraHeightOffset;

            // Set the camera position behind and slightly above the bullet
            m_cameraTransform.position = m_bullet.position + offset;

            // Make the camera look forward in the direction the bullet is moving
            m_cameraTransform.LookAt(m_bullet.position + m_bullet.forward * 5f);
        }

        public void SetBullet(Transform bullet)
        {
            m_bullet = bullet;
        }

    }
}
