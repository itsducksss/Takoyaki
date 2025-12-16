using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> pathNodes = new List<Transform>();
    public float pathReachingRadius = 2f;

    private bool isPathValid()
    {
        return this && pathNodes.Count > 0;
    }

    public Vector3 GetPositionOfPathNodes(int NodeIndex)
    {
        if (NodeIndex < 0 || NodeIndex >= pathNodes.Count || pathNodes[NodeIndex] == null)
        { 
            return Vector3.zero;
        }

        return pathNodes[NodeIndex].position;
    }

    public Vector3 GetDestinationPath(Transform agent, int pathDestinationNodeIndex)
    {
        if (isPathValid())
        {
            return GetPositionOfPathNodes(pathDestinationNodeIndex);

        }
        else
        {
            return agent.position;
        }
    }

    public int UpdatePathDestination (Transform agent, int pathDestinationNodeIndex, bool inverseOrder = false)
    {
        if (isPathValid())
        {
            if ((agent.position - GetDestinationPath(agent, pathDestinationNodeIndex)).magnitude <= pathReachingRadius)
            {
                pathDestinationNodeIndex = inverseOrder ? (pathDestinationNodeIndex - 1) : (pathDestinationNodeIndex + 1);
                if (pathDestinationNodeIndex < 0)
                {
                    pathDestinationNodeIndex += pathNodes.Count;
                }
                if (pathDestinationNodeIndex >= pathNodes.Count)
                {
                    pathDestinationNodeIndex -= pathNodes.Count;
                }
            }
        }
        return pathDestinationNodeIndex;
    }
}
