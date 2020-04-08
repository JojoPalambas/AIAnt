using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneDescriptor
{
    public readonly PheromoneType type;
    public readonly HexDirection direction;

    public PheromoneDescriptor(PheromoneType type, HexDirection direction)
    {
        this.type = type;
        this.direction = direction;
    }

    public static PheromoneDescriptor FromDigest(PheromoneDigest digest)
    {
        if (digest == null)
            return null;

        return new PheromoneDescriptor(digest.type, digest.direction);
    }

    public static List<PheromoneDescriptor> ListFromDigestList(List<PheromoneDigest> digests)
    {
        List<PheromoneDescriptor> ret = new List<PheromoneDescriptor>();
        for (int i = 0; i < Const.MAX_PHEROMONE_BY_CELL; i++)
        {
            if (digests != null && i < digests.Count)
            {
                if (digests[i] != null)
                    ret.Add(FromDigest(digests[i]));
            }
            else
                break;
        }

        return ret;
    }
}
