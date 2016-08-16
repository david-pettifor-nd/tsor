using System;
using UnityEngine;

public class Cell
{
    public bool visitied = false;
    public bool constructed = false;

    public bool north_wall = true;
    public bool east_wall = true;
    public bool west_wall = true;
    public bool south_wall = true;

    public Vector3 location;

    public Cell()
	{

	}

    public void KnockWall(string wall)
    {
        switch (wall)
        {
            case "north":
                this.north_wall = false;
                break;
            case "south":
                this.south_wall = false;
                break;
            case "east":
                this.east_wall = false;
                break;
            case "west":
                this.west_wall = false;
                break;
        }
    }

    public void RenderCell(Vector3 location)
    {

    }
}
