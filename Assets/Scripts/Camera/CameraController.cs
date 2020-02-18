using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CleverCamera
{
    public class CameraController : MonoBehaviour
    {
        private const float angleChangeValue = 0.5f;

        public float CameraRotationAngle {
            get {
                return m_Angle;
            }
        }


        #region vars
        public Transform m_Target;
        public float m_Height = 10f;
        public float m_Distance = 20f;
        public float m_Angle = 45f;
        public float m_SmoothSpeed = 0.5f;

        private Vector3 refVelocity;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }
        void FixedUpdate() {
            if (Input.GetKey(KeyCode.Q)) {
                m_Angle -= angleChangeValue;
            }
            else if (Input.GetKey(KeyCode.E)) {
                m_Angle += angleChangeValue;
            }
        }

        protected virtual void HandleCamera()
        {
            if (!m_Target)
            {
                return;
            }

            Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
            Debug.DrawLine(m_Target.position, worldPosition, Color.red);

            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
            Debug.DrawLine(m_Target.position, rotatedVector, Color.green);

            Vector3 flatTargetPosition = m_Target.position;
            flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            Debug.DrawLine(m_Target.position, finalPosition, Color.blue);

            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, m_SmoothSpeed);
            transform.LookAt(flatTargetPosition);


        }
    }
}
