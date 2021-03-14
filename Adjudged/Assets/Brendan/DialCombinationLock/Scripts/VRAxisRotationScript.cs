using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Valve.VR.InteractionSystem;

public class VRAxisRotationScript : MonoBehaviour {

	bool m_beingHeld = false;
	
	private Vector3 m_anchorPosition;
	private Vector3 m_anchorRotation;
    private Vector3 m_anchorUp;
    private Vector3 m_anchorScale;

    public UnityEvent<float> m_angleUpdate;

	public GameObject m_parentObject = null;

	public float m_rotationOffset;
	public float m_controllerOffset;

    public enum AxisDir
    {
        AD_XPos,
        AD_XNeg,
        AD_YPos,
        AD_YNeg,
        AD_ZPos,
        AD_ZNeg
    }

    [Tooltip("GameObject used for relative axis")]
    public GameObject m_relativeAxisObject;

    [Tooltip("Axis that the dial will be projected on when rotated")]
    public AxisDir m_projectionAxis = AxisDir.AD_XPos;
    [Tooltip("The direction that will be used to measure rotation")]
    public AxisDir m_rotationDir = AxisDir.AD_ZPos;
    [Tooltip("The direction that will be used to differentiate rotation")]
    public AxisDir m_rotationDirPerpendicular = AxisDir.AD_YNeg;

    public bool m_externalFlipRotation = true;
    public float m_externalRotationOffset = -270;

    // debug variables

    // display variables

 //   public float m_dDialAngle = 0;
	//public float m_dParentAngle = 0;
 //   public float m_dExternalAngle = 0;



    void Awake()
	{
		m_anchorPosition = transform.position;
		m_anchorRotation = transform.localEulerAngles;
        m_anchorScale = transform.localScale;

    }

	void LateUpdate()
	{
		if (m_beingHeld)
		{
			transform.position = m_anchorPosition;
            transform.localScale = m_anchorScale;

            //Vector3 t = Vector3.ProjectOnPlane(transform.parent.up, AxisDirToVector(m_relativeAxisObject, m_projectionAxis));

            float angle = GetAngleOfObject();

			float angleOfParent = GetParentAngle() - m_controllerOffset;
            if (angleOfParent < 0) // make sure angle of Parent isn't a negative
            {
                //angleOfParent -= Mathf.Floor(-angleOfParent / 360) * 360;
            }
			float dialAngle = angleOfParent + m_rotationOffset;

			Quaternion qq = Quaternion.AngleAxis(dialAngle, AxisDirToVector(m_relativeAxisObject, m_projectionAxis));

            transform.rotation = qq;

            angle = GetAngleOfObject();

            //Quaternion q = Quaternion.LookRotation(
            //        (
            //           m_useOffset ?
            //Quaternion.AngleAxis(m_rotationOffset, AxisDirToVector(m_relativeAxisObject, m_OffsetAxis))
            //: Quaternion.identity
            //        )
            //        .eulerAngles, AxisDirToVector(m_relativeAxisObject, m_projectionAxis))
            //    * Quaternion.LookRotation(t, AxisDirToVector(m_relativeAxisObject, m_projectionAxis));



            // callback function
            if (m_angleUpdate != null)
            {
                m_angleUpdate.Invoke(GetAngleOfObject());
            }
		}
        else if (transform.parent != m_parentObject.transform)
        {
            transform.SetParent(m_parentObject.transform);
        }
  //      m_dDialAngle = GetAngleOfObject();
		//m_dParentAngle = GetParentAngle() - m_controllerOffset;
  //      m_dExternalAngle = ExternalGetAngleOfObject();
	}

	public float GetParentAngle()
	{
		return GetAngleFromVector(Vector3.ProjectOnPlane(transform.parent.up, AxisDirToVector(m_relativeAxisObject, m_projectionAxis)));
	}

	float GetAngleOfObject()
	{
		return GetAngleFromTransform(transform);
	}

    public float ExternalGetAngleOfObject()
    {
        float angle = GetAngleOfObject();
        angle += m_externalRotationOffset;
        if (angle < 0) // make sure angle of Parent isn't a negative
        {
            angle += Mathf.Ceil(-angle / 360) * 360;
        }
        if (angle > 360)
        {
            angle -= Mathf.Floor(angle / 360) * 360;
        }
        if (m_externalFlipRotation)
        {
            angle = -(angle - 360);
        }
        return angle;
    }

	public float GetAngleFromTransform(Transform t)
	{
		float angle = Vector3.Angle(AxisDirToVector(m_relativeAxisObject, m_rotationDir), t.forward);
		float diff = Vector3.Angle(AxisDirToVector(m_relativeAxisObject, m_rotationDirPerpendicular), t.forward);
        return (diff > 90 ? 360 - angle : angle);
    }

	public float GetAngleFromVector(Vector3 v)
	{
		float angle = Vector3.Angle(AxisDirToVector(m_relativeAxisObject, m_rotationDir), v);
		float diff = Vector3.Angle(AxisDirToVector(m_relativeAxisObject, m_rotationDirPerpendicular), v);
        return (diff > 90 ? 360 - angle : angle);
    }

	public Vector3 AxisDirToVector(Transform t, AxisDir dir)
	{
		switch (dir)
		{
			case AxisDir.AD_YPos:
				return t.up;
			case AxisDir.AD_YNeg:
				return -t.up;
			case AxisDir.AD_XPos:
				return t.right;
			case AxisDir.AD_XNeg:
				return -t.right;
			case AxisDir.AD_ZPos:
				return t.forward;
			case AxisDir.AD_ZNeg:
				return -t.forward;
			default:
				return new Vector3();
		}
	}

	public Vector3 AxisDirToVector(GameObject g, AxisDir dir)
	{
		return AxisDirToVector(g.transform, dir);
	}

	public void Detatch()
	{
		if (transform.parent.GetComponent<Hand>())
		{
			transform.parent.GetComponent<Hand>().DetachObject(gameObject);

			Destroy(GetComponent<Throwable>());
			Destroy(GetComponent<VelocityEstimator>());
			Destroy(GetComponent<Interactable>());
        }
    }

	public void Grabbed()
	{
		m_rotationOffset = GetAngleOfObject();
		m_controllerOffset = GetParentAngle();
		m_beingHeld = true;
	}

	public void Released()
	{
		m_beingHeld = false;
		transform.localScale = m_anchorScale;
	}
}
