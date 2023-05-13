using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshBaker : MonoBehaviour
{
	public NavMeshSurface Surface2D;

	void Start()
	{
		Surface2D.BuildNavMeshAsync();
	}
}
