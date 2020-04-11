using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PheromoneDisplayOption
{
    public PheromoneType type;
    public int teamId;

    public bool enabled;
    public Color color;
}

public class PheromoneMapDisplayer : MonoBehaviour
{
    private static readonly Color NULL_COLOR = new Color(0, 128, 116);

    public PheromoneDisplayer displayerPrefab;

    public List<PheromoneDisplayOption> displayOptions;
    private List<PheromoneDisplayer>[][][] displayMaps;

    public void InitMap(List<Team> teams, int terrainWidth, int terrainHeight)
    {
        // Create all the pheromone maps
        displayMaps = new List<PheromoneDisplayer>[teams.Count][][];
        foreach (Team team in teams)
        {
            List<PheromoneDisplayer>[][] pheromoneMap = new List<PheromoneDisplayer>[terrainWidth][];
            displayMaps[team.teamId] = pheromoneMap;

            for (int i = 0; i < terrainWidth; i++)
            {
                pheromoneMap[i] = new List<PheromoneDisplayer>[terrainHeight];
                for (int j = 0; j < terrainHeight; j++)
                {
                    pheromoneMap[i][j] = new List<PheromoneDisplayer>();
                }
            }
        }
    }

    public void UpdateCell(List<PheromoneDescriptor>[][][] pheromoneMaps, int x, int y)
    {
        // Removing all the displayers from the cell
        foreach (List<PheromoneDisplayer>[][] displayMap in displayMaps)
        {
            foreach (PheromoneDisplayer displayer in displayMap[x][y])
            {
                Destroy(displayer.gameObject);
            }
            displayMap[x][y] = new List<PheromoneDisplayer>();
        }

        // Adding all the needed displayers to the cell
        for (int i = 0; i < pheromoneMaps.Length; i++)
        {
            List<PheromoneDescriptor>[][] pheromoneMap = pheromoneMaps[i];
            List<PheromoneDisplayer>[][] displayMap = displayMaps[i];

            foreach (PheromoneDescriptor pheromone in pheromoneMap[x][y])
            {
                Color validityColor = CheckValidity(pheromone, i);
                if (validityColor != NULL_COLOR)
                {
                    PheromoneDisplayer newDisplayer = Instantiate(
                        displayerPrefab,
                        CoordConverter.PlanToWorld(CoordConverter.HexToPos(new Vector2Int(x, y)), displayerPrefab.transform.position.y),
                        displayerPrefab.transform.rotation);

                    newDisplayer.displayDirection = pheromone.direction;
                    newDisplayer.transform.LookAt(CoordConverter.PlanToWorld(CoordConverter.HexToPos(CoordConverter.MoveHex(new Vector2Int(x, y), newDisplayer.displayDirection)), newDisplayer.transform.position.y));
                    newDisplayer.SetColor(validityColor);
                    
                    displayMap[x][y].Add(newDisplayer);
                }
            }

        }
    }

    private bool CheckValidity(PheromoneDisplayer displayer, int teamId)
    {
        foreach (PheromoneDisplayOption option in displayOptions)
        {
            if (option.enabled && displayer.type == option.type && teamId == option.teamId)
                return true;
        }
        return false;
    }

    private Color CheckValidity(PheromoneDescriptor pheromone, int teamId)
    {
        foreach (PheromoneDisplayOption option in displayOptions)
        {
            if (option.enabled && pheromone.type == option.type && teamId == option.teamId)
                return option.color;
        }
        return NULL_COLOR;
    }

    public void Die()
    {
        foreach (List<PheromoneDisplayer>[][] displayMap in displayMaps)
        {
            foreach (List<PheromoneDisplayer>[] displayLine in displayMap)
            {
                foreach (List<PheromoneDisplayer> displays in displayLine)
                {
                    foreach (PheromoneDisplayer display in displays)
                    {
                        display.Die();
                    }
                }
            }
        }
    }
}
