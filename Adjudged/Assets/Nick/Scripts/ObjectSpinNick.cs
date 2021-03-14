using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class ObjectSpinNick : MonoBehaviour
    {

#pragma warning disable 0414

        public enum Axis { x, y, z };
        public Axis axis;

        public enum MotionType { Rotation, BackAndForth };
        public MotionType Motion;

        public float SpinSpeed = 5;
        public int RotationRange = 15;
        private Transform m_transform;

        private float m_time;
        private Vector3 m_prevPOS;
        private Vector3 m_initial_Rotation;
        private Vector3 m_initial_Position;
        private Color32 m_lightColor;
        private int frames = 0;

        public bool ontrigger;


        void Awake()
        {
            m_transform = transform;
            m_initial_Rotation = m_transform.rotation.eulerAngles;
            m_initial_Position = m_transform.position;

            Light light = GetComponent<Light>();
            m_lightColor = light != null ? light.color : Color.black;
        }


        // Update is called once per frame
        void Update()
        {
            if (axis == Axis.x)
            {
                if (Motion == MotionType.Rotation)
                {
                    m_transform.Rotate(SpinSpeed * Time.deltaTime, 0, 0 );
                }
                else
                {
                    m_time += SpinSpeed * Time.deltaTime;
                    m_transform.rotation = Quaternion.Euler(Mathf.Sin(m_time) * RotationRange + m_initial_Rotation.x, m_initial_Rotation.y, m_initial_Rotation.z);
                }
            }
            else if (axis == Axis.y)
            {
                if (Motion == MotionType.Rotation)
                {
                    m_transform.Rotate(0, SpinSpeed * Time.deltaTime, 0);
                }
                else
                {
                    m_time += SpinSpeed * Time.deltaTime;
                    m_transform.rotation = Quaternion.Euler(m_initial_Rotation.x, Mathf.Sin(m_time) * RotationRange + m_initial_Rotation.y, m_initial_Rotation.z);
                }
            }
            else
            {
                if (Motion == MotionType.Rotation)
                {
                    m_transform.Rotate(0, 0, SpinSpeed * Time.deltaTime);
                }
                else
                {
                    m_time += SpinSpeed * Time.deltaTime;
                    m_transform.rotation = Quaternion.Euler(m_initial_Rotation.x, m_initial_Rotation.y, Mathf.Sin(m_time) * RotationRange + m_initial_Rotation.z);
                }
            }
        }

    }
}