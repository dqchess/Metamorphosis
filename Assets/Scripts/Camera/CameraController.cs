using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CleverCamera {
    public class CameraController : MonoBehaviour {
        private const float angleChangeValue = 0.5f;

        public float CameraRotationAngle {
            get {
                return m_Angle;
            }
        }

        private float rotationMultipier = 1.0f;
        private float heightMultipier = .5f;
        private float distanceMultipier = 1.0f;
        #region vars
        public Transform m_Target;
        public float m_Height = 10f;
        public float m_Distance = 20f;
        public float m_Angle = 45f;
        public float m_SmoothSpeed = 0.5f;

        private Vector3 refVelocity;
        #endregion
        // Start is called before the first frame update
        void Start() {
            HandleCamera();
        }

        // Update is called once per frame
        void Update() {
            HandleCamera();
        }
        void FixedUpdate() {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) {
                if (Input.GetKey(KeyCode.Q)) {
                    m_Angle -= angleChangeValue * rotationMultipier;
                }
                else if (Input.GetKey(KeyCode.E)) {
                    m_Angle += angleChangeValue * rotationMultipier;
                }
                rotationMultipier += 0.15f;
            }
            else {
                rotationMultipier = 1.0f;
            }
            if (Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.H)) {
                if (Input.GetKey(KeyCode.Y)) {
                    m_Height += angleChangeValue * heightMultipier;
                }
                else if (Input.GetKey(KeyCode.H)) {
                    m_Height -= angleChangeValue * heightMultipier;
                }
                heightMultipier += 0.05f;
            }
            else {
                heightMultipier = 0.5f;
            }
            if (Input.mouseScrollDelta.y != 0) {
                if (Input.mouseScrollDelta.y < 0) {
                    m_Distance += angleChangeValue * distanceMultipier;
                }
                else if (Input.mouseScrollDelta.y > 0) {
                    m_Distance -= angleChangeValue * distanceMultipier;
                }
                distanceMultipier += 0.2f;
            }
            else {
                distanceMultipier = 1.0f;
            }
        }

        protected virtual void HandleCamera() {
            if (!m_Target) {
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
