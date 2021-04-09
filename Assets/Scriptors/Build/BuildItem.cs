using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildItem {

    private string name;

    private string posX;
    private string posY;
    private string posZ;

    private string rotX;
    private string rotY;
    private string rotZ;
    private string rotW;

    public string Name { set { name = value; } get { return name; } }

    public string PosX { set { posX = value; } get { return posX; } }
    public string PosY { set { posY = value; } get { return posY; } }
    public string PosZ { set { posZ = value; } get { return posZ; } }

    public string RotX { set { rotX = value; } get { return rotX; } }
    public string RotY { set { rotY = value; } get { return rotY; } }
    public string RotZ { set { rotZ = value; } get { return rotZ; } }
    public string RotW { set { rotW = value; } get { return rotW; } }

    public BuildItem() { }
    public BuildItem(string n, string px, string py, string pz, string rx, string ry, string rz, string rw)
    {
        name = n;

        posX = px;
        posY = py;
        posZ = pz;

        rotX = rx;
        rotY = ry;
        rotZ = rz;
        rotW = rw;
    }
}
