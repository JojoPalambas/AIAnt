using System.Collections.Generic;

public class PheromoneDigest
{
    public readonly PheromoneType type;
    public readonly HexDirection direction;

    public PheromoneDigest(PheromoneType type, HexDirection direction)
    {
        this.type = type;
        this.direction = direction;
    }

    public static PheromoneDigest FromDescriptor(PheromoneDescriptor descriptor)
    {
        if (descriptor == null)
            return null;

        return new PheromoneDigest(descriptor.type, descriptor.direction);
    }

    public static List<PheromoneDigest> ListFromDescriptorList(List<PheromoneDescriptor> descriptors)
    {
        List<PheromoneDigest> ret = new List<PheromoneDigest>();
        for (int i = 0; i < Const.MAX_PHEROMONE_BY_CELL; i++)
        {
            if (descriptors != null && i < descriptors.Count)
            {
                if (descriptors[i] != null)
                    ret.Add(FromDescriptor(descriptors[i]));
            }
            else
                break;
        }

        return ret;
    }
}
