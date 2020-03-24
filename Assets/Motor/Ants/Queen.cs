using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Ant
{
    public override AntType Type
    {
        get { return AntType.QUEEN; }
    }

    // Start is called before the first frame update
    void Start()
    {
        SuperStart();

        carriedFood = 100;
    }

    // Update is called once per frame
    void Update()
    {
        SuperUpdate();
    }
}
