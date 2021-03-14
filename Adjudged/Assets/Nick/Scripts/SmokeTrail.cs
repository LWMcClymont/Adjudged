using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(LineRenderer))]
public class SmokeTrail : MonoBehaviour {


	private LineRenderer line;
	private Transform tr;
	private Vector3[] positions;
	private Vector3[] directions;
	private float timeSinceUpdate;
	private Material lineMaterial;
	private float lineSegment;
	private int currentNumberOfPoints;
	private bool allPointsAdded = false;
	private Vector3 tempVec;

	private bool smoking = true;
	public int numberOfPoints = 10;
	public float updateSpeed = 0.25f;
	public float riseSpeed = 0.25f;
	public float spread = 0.2f;

	private float alpha;
	public float fadeOut = 0.05f;

	void Start()
	{
        Debug.Log("Initialise");
        tr = transform;
        line = GetComponent<LineRenderer>();

        lineMaterial = line.material;
        alpha = lineMaterial.color.a;
        lineSegment = 1.0f / numberOfPoints;

        positions = new Vector3[numberOfPoints];
        directions = new Vector3[numberOfPoints];
        timeSinceUpdate = 0;
        lineSegment = 0.0f;
        currentNumberOfPoints = 2;
        line.positionCount = currentNumberOfPoints;

        for (int i = 0; i < currentNumberOfPoints; i++)
        {
            tempVec = getSmokeVec();
            directions[i] = tempVec;
            positions[i] = tr.position;
            line.SetPosition(i, positions[i]);
        }
    }
	
	void Update ()
	{

		timeSinceUpdate += Time.deltaTime;

		if (timeSinceUpdate > updateSpeed)
		{
			timeSinceUpdate -= updateSpeed;

			// Add points until the target number is reached.
			if (!allPointsAdded)
			{
				currentNumberOfPoints++;
				line.positionCount = currentNumberOfPoints;
				tempVec = getSmokeVec();
				directions[0] = tempVec;
				positions[0] = tr.position;
				line.SetPosition(0, positions[0]);
			}

			if (!allPointsAdded && (currentNumberOfPoints == numberOfPoints))
			{
				allPointsAdded = true;
			}

			// Make each point in the line take the position and direction of the one before it (effectively removing the last point from the line and adding a new one at transform position).
			for (int i = currentNumberOfPoints - 1; i > 0; i--)
			{
				tempVec = positions[i - 1];
				positions[i] = tempVec;
				tempVec = directions[i - 1];
				directions[i] = tempVec;
			}

			tempVec = getSmokeVec();
			directions[0] = tempVec; // Remember and give 0th point a direction for when it gets pulled up the chain in the next line update.
		}

		// Update the line...
		for (int i = 1; i < currentNumberOfPoints; i++)
		{
			tempVec = positions[i];
			tempVec += directions[i] * Time.deltaTime;
			positions[i] = tempVec;

			line.SetPosition(i, positions[i]);
		}
		positions[0] = tr.position; // 0th point is a special case, always follows the transform directly.
		line.SetPosition(0, tr.position);

		// If we're at the maximum number of points, tweak the offset so that the last line segment is "invisible" (i.e. off the top of the texture) when it disappears.
		// Makes the change less jarring and ensures the texture doesn't jump.
		if (allPointsAdded)
		{
            //Debug.Log("updateing material");
			Vector2 texOffset = lineMaterial.mainTextureOffset;
			texOffset.x = lineSegment * (timeSinceUpdate / updateSpeed);
			lineMaterial.mainTextureOffset = texOffset;
		}

		if (!smoking)
			FadeOut();
	}

	Vector3 getSmokeVec()
	{
		Vector3 smokeVec;

		smokeVec.x = Random.Range(-0.5f, 0.5f);
		smokeVec.y = Random.Range( 0.0f, 0.5f);
		smokeVec.z = Random.Range(-0.5f, 0.5f);
		smokeVec.Normalize();
		smokeVec *= spread;
		smokeVec.y += riseSpeed;

		return smokeVec;
	}

	void FadeOut()
	{
        if (alpha > 0)
        {
            alpha -= fadeOut;
            Color colour = lineMaterial.color;
            colour.a = alpha;
            lineMaterial.color = colour;
        }
        else
        {
            alpha = 0;
            Color colour = lineMaterial.color;
            colour.a = alpha;
            lineMaterial.color = colour;
        }
	}

    public void StopSmoking()
    {
        smoking = false;
    }

    public bool ReadyToDestroy()
    {
        if (alpha == 0)
            return true;
        else
            return false;
    }

    public void DestroySmoke()
    {
        Destroy(gameObject);
    }
}
