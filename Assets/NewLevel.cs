using UnityEngine;
using System.Collections.Generic;

public class Cell
{
    public GameObject cell;
    public bool constructed;
    public int x, y;

    public bool north_wall;
    public bool south_wall;
    public bool east_wall;
    public bool west_wall;

    public Cell()
    {
        this.constructed = false;
        this.x = 0;
        this.y = 0;
        this.north_wall = true;
        this.south_wall = true;
        this.east_wall = true;
        this.west_wall = true;
    }


}


public class NewLevel : MonoBehaviour {

    // Use this for initialization
    private int width;// =  GameObject.Find("Values");
    private int height;// = 10;
    private int cell_size = 5;

    private Cell[,] floorplan;

    void Start() {
        // get the global values of what we should set as the height and width
        GameObject gamevalues = GameObject.Find("GameValues");
        Values vals = gamevalues.GetComponent("Values") as Values;
        width = vals.width;
        height = vals.height;

        // reset the player's position
        GameObject player = GameObject.Find("FPSController");
        player.transform.position = new Vector3((float)0.7, 1, (float)0.506);
        floorplan = new Cell[width, height];
        GenerateMaze();

    }

    // Update is called once per frame
    void Update() {

    }

    void GenerateMaze()
    {
        // start at 0, 0 - creating all of the cells we need
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // create a new cell
                GameObject next_cell = Instantiate(Resources.Load("objects/cell/Cell Template", typeof(GameObject)), new Vector3(i*cell_size, 0, j*cell_size), Quaternion.identity) as GameObject;
                Cell cell = new Cell();
                cell.cell = next_cell;
                cell.x = i;
                cell.y = j;
                this.floorplan[i, j] = cell;
            }
        }

        // start at 0, 0
        CreateMazeStep(ref this.floorplan[0, 0]);

        // at this point, we have our maze built
        // ...but neighboring cells have two walls and two corners in the same location...so run through and delete them
        // !!remember!!  we only need to remove ONE, so for sanity's sake, let's target the north and west walls; plus the NE/SW corners
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                // check the walls for this dude
                if(this.floorplan[i, j].north_wall && this.floorplan[i, j].y > 0)
                {
                    // remove it
                    Destroy(this.floorplan[i, j].cell.transform.FindChild("NorthWall").gameObject);
                }

                if (this.floorplan[i, j].west_wall && this.floorplan[i, j].x > 0)
                {
                    // remove it
                    Destroy(this.floorplan[i, j].cell.transform.FindChild("WestWall").gameObject);
                }
            }
        }

        // an artifact of the prefab'd cells are that if a path is created around a right-turn, the corner on the inside
        // of that turn is missing.  So let's round off those corners...
        RoundCorners();


        //-------------- KEY PLACEMENT ---------------------//
        // choose a random cell to place our key in
        int rand_x = Random.Range(0, width-1);
        int rand_y = Random.Range(0, height-1);
        GameObject random_cell = this.floorplan[rand_x, rand_y].cell;
        float pos_x = random_cell.transform.position.x;// + (float)2.5;
        float pos_z = random_cell.transform.position.z;// + (float)2.5;
        //Debug.Log("Placing key at: " + pos_x + "," + pos_z);

        // now that the maze is generated, we can add our key
        GameObject key = Instantiate(Resources.Load("key", typeof(GameObject)), new Vector3(pos_x, (float)0.1, pos_z), Quaternion.identity) as GameObject;
        key.AddComponent<ItemInView>();
        Debug.Log("Loaded key");
        //------------------ END KEY PLACEMENT ----------------------//

        //--------------- ANCIENT ARTIFACTS ---------------//
        GameObject mapgen = GameObject.Find("GameValues");
        Values val = mapgen.GetComponent("Values") as Values;
        //check if we've generated a scarab yet
        if (!val.scarab_generated)
        {
            // then we have a chance
            if(Random.Range(1, 100) <= val.scarab_chance)
            {
              // then generate the scarab!
              Debug.Log("Scarab generated!");
              rand_x = Random.Range(0, width - 1);
              rand_y = Random.Range(0, height - 1);
              random_cell = this.floorplan[rand_x, rand_y].cell;
              pos_x = random_cell.transform.position.x;// + (float)2.5;
              pos_z = random_cell.transform.position.z;// + (float)2.5;
              GameObject scarab = Instantiate(Resources.Load("objects/stone_scarab/scarab", typeof(GameObject)), new Vector3(pos_x, (float)0.1, pos_z), Quaternion.identity) as GameObject;
              //GameObject scarab = Instantiate(Resources.Load("key", typeof(GameObject)), new Vector3(pos_x, (float)1.0, pos_z), Quaternion.identity) as GameObject;
              scarab.transform.Find("Body").gameObject.AddComponent<ScarabScript>();
              val.scarab_generated = true;
            }
        }

        //------------- DOORS ---------------//

        // let's also add some doors
        GameObject exit_cell = this.floorplan[width - 1, height - 1].cell;
        pos_x = exit_cell.transform.position.x;// - (float)0.555;
        //float pos_y = exit_cell.transform.position.y + (float)0.691;
        float pos_y = 0;
        pos_z = exit_cell.transform.position.z + (float)2.25;
        GameObject exit_door = Instantiate(Resources.Load("objects/door/exit_door", typeof(GameObject)), new Vector3(pos_x, pos_y, pos_z), Quaternion.Euler(0, 270, 0)) as GameObject;
        exit_door.AddComponent<DoorInteraction>();
        exit_door.transform.localScale = new Vector3((float)1, (float)1, (float)1);

        // also do an entrance door
        GameObject entrance_cell = this.floorplan[0, 0].cell;
        pos_x = entrance_cell.transform.position.x;// - (float)0.555;
        pos_y = 0;
        pos_z = entrance_cell.transform.position.z - (float)2.25;
        GameObject entrance_door = Instantiate(Resources.Load("objects/door/entrance_door", typeof(GameObject)), new Vector3(pos_x, pos_y, pos_z), Quaternion.Euler(0, 90, 0)) as GameObject;
        Destroy(entrance_door.transform.Find("Key").gameObject);
        //Destroy(entrance_door.transform.Find("Plane").gameObject);
        //entrance_door.AddComponent<DoorInteraction>();
        entrance_door.transform.localScale = new Vector3(1, (float)0.45, (float)0.45);

        //----------------- END DOORS ------------------//




    }

    void JoinCells(ref Cell cell1, ref Cell cell2)
    {
        if(cell1.x > cell2.x && cell1.y == cell2.y)
        {
            KnockWall(ref cell1, "west");
            KnockWall(ref cell2, "east");
        }
        if (cell1.x < cell2.x && cell1.y == cell2.y)
        {
            KnockWall(ref cell1, "east");
            KnockWall(ref cell2, "west");
        }
        if (cell1.y > cell2.y && cell1.x == cell2.x)
        {
            KnockWall(ref cell1, "north");
            KnockWall(ref cell2, "south");
        }
        if (cell1.y < cell2.y && cell1.x == cell2.x)
        {
            KnockWall(ref cell1, "south");
            KnockWall(ref cell2, "north");
        }
    }

    void CreateMazeStep(ref Cell current_cell)
    {
        //Debug.Log("Working on: " + current_cell.x + ", " + current_cell.y);
        // mark this cell as being visited
        current_cell.constructed = true;

        // get a list of neighbors that have not been constructed
        List<Cell> neighbors = new List<Cell>();

        // check to the west
        if (current_cell.x != 0 && !this.floorplan[current_cell.x - 1, current_cell.y].constructed)
        {
            neighbors.Add(this.floorplan[(current_cell.x - 1), current_cell.y]);
        }

        // check to the east
        if (current_cell.x < width - 1 && !this.floorplan[current_cell.x + 1, current_cell.y].constructed)
        {
            neighbors.Add(this.floorplan[(current_cell.x + 1), current_cell.y]);
        }

        // check to the north
        if (current_cell.y != 0 && !this.floorplan[current_cell.x, (current_cell.y - 1)].constructed)
        {
            neighbors.Add(this.floorplan[current_cell.x, current_cell.y - 1]);
        }

        // check to the south
        if (current_cell.y < height - 1 && !this.floorplan[current_cell.x, current_cell.y + 1].constructed)
        {
            neighbors.Add(this.floorplan[current_cell.x, (current_cell.y + 1)]);
        }

        // shuffle the neighbors
        for(int z = 0; z < neighbors.Count; z++)
        {
            Cell temp = neighbors[z];
            int randomIndex = Random.Range(z, neighbors.Count);
            neighbors[z] = neighbors[randomIndex];
            neighbors[randomIndex] = temp;
        }

        // for each neighbor, see if it's been constructed yet.  If not, join the two cells and call this function again
        for(int z = 0; z < neighbors.Count; z++)
        {
            if (!neighbors[z].constructed)
            {
                // join the two cells
                Cell this_cell = current_cell;
                Cell next_cell = neighbors[z];
                //Debug.Log("Joining: " + this_cell.x + ", " + this_cell.y + " and " + next_cell.x + ", " + next_cell.y);
                JoinCells(ref this_cell, ref next_cell);
                CreateMazeStep(ref next_cell);
            }
        }

    }

    public void KnockWall(ref Cell current_cell, string wall)
    {
        switch (wall)
        {
            case "north":
                //Debug.Log("Removing north wall");
                Destroy(current_cell.cell.transform.FindChild("NorthWall").gameObject);
                current_cell.north_wall = false;
                break;
            case "south":
                //Debug.Log("Removing south wall");
                Destroy(current_cell.cell.transform.FindChild("SouthWall").gameObject);
                current_cell.south_wall = false;
                break;
            case "east":
                //Debug.Log("Removing east wall");
                Destroy(current_cell.cell.transform.FindChild("EastWall").gameObject);
                current_cell.east_wall = false;
                break;
            case "west":
                //Debug.Log("Removing west wall");
                Destroy(current_cell.cell.transform.FindChild("WestWall").gameObject);
                current_cell.west_wall = false;
                break;
        }
    }

    public void RoundCorners()
    {
        // loop through each cell looking for right-turns
        for(var x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                // check for NE corner
                if(y > 0 && x < (width-1) && !floorplan[x,y].north_wall && !floorplan[x,y].east_wall && floorplan[x,y-1].east_wall && floorplan[x+1, y].north_wall)
                {
                    // then we have a NE corner to fill
                    Debug.Log("NE Corner");
                    float pos_x = floorplan[x, y].cell.gameObject.transform.position.x + (float)2.375;
                    float pos_z = floorplan[x, y].cell.gameObject.transform.position.z - (float)2.375;
                    GameObject corner = Instantiate(Resources.Load("objects/cell/Corner", typeof(GameObject)), new Vector3(pos_x, (float)2.5, pos_z), Quaternion.identity) as GameObject;
                }

                // check for SE corner
                if (y < (height-1) && x < (width-1) && !floorplan[x, y].south_wall && !floorplan[x, y].east_wall && floorplan[x, y + 1].east_wall && floorplan[x + 1, y].south_wall)
                {
                    Debug.Log("SE Corner");
                    // then we have a SE corner to fill
                    float pos_x = floorplan[x, y].cell.gameObject.transform.position.x + (float)2.375;
                    float pos_z = floorplan[x, y].cell.gameObject.transform.position.z + (float)2.375;
                    GameObject corner = Instantiate(Resources.Load("objects/cell/Corner", typeof(GameObject)), new Vector3(pos_x, (float)2.5, pos_z), Quaternion.identity) as GameObject;
                }

                // check for NW corner
                if (y > 0 && x > 0 && !floorplan[x, y].north_wall && !floorplan[x, y].west_wall && floorplan[x, y - 1].west_wall && floorplan[x - 1, y].north_wall)
                {
                    Debug.Log("NW Corner");
                    // then we have a NW corner to fill
                    float pos_x = floorplan[x, y].cell.gameObject.transform.position.x - (float)2.375;
                    float pos_z = floorplan[x, y].cell.gameObject.transform.position.z - (float)2.375;
                    GameObject corner = Instantiate(Resources.Load("objects/cell/Corner", typeof(GameObject)), new Vector3(pos_x, (float)2.5, pos_z), Quaternion.identity) as GameObject;
                }

                // check for SW corner
                if (y < (height-1) && x > 0 && !floorplan[x, y].south_wall && !floorplan[x, y].west_wall && floorplan[x, y + 1].west_wall && floorplan[x - 1, y].south_wall)
                {
                    Debug.Log("SW Corner");
                    // then we have a NE corner to fill
                    float pos_x = floorplan[x, y].cell.gameObject.transform.position.x - (float)2.375;
                    float pos_z = floorplan[x, y].cell.gameObject.transform.position.z + (float)2.375;
                    GameObject corner = Instantiate(Resources.Load("objects/cell/Corner", typeof(GameObject)), new Vector3(pos_x, (float)2.5, pos_z), Quaternion.identity) as GameObject;
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width / 2) - 2, (Screen.height / 2) - 2, 4, 4), new Texture2D(4, 4));
    }

}
