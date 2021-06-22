using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectVector
{
    public static Vector3 BlueNorth = new Vector3(90, 180, 180);
    public static Vector3 BlueEast = new Vector3(90, 180, 90);
    public static Vector3 BlueSouth = new Vector3(90, 180, 0);
    public static Vector3 BlueWest = new Vector3(90, 180, -90);

    public static Vector3 OrangeNorth = new Vector3(90, -180, 180);
    public static Vector3 OrangeEast = new Vector3(90, -180, 90);
    public static Vector3 OrangeSouth = new Vector3(90, -180, 0);
    public static Vector3 OrangeWest = new Vector3(90, -180, -90);
    
    public static Vector3 CalculateVectorToDirection(Vector3 _tVector3 , bool _isBlue)
    {
        if (_isBlue)
        {
            if (_tVector3.y >= 45 && _tVector3.y < 135)
                return BlueWest;
            else if (_tVector3.y >= 135 && _tVector3.y < 225)
                return BlueNorth;
            else if (_tVector3.y >= 225 && _tVector3.y < 315)
                return BlueEast;
            else
                return BlueSouth;
        }
        else
        {
            if (_tVector3.y >= 45 && _tVector3.y < 135)
                return OrangeWest;
            else if (_tVector3.y >= 135 && _tVector3.y < 225)
                return OrangeNorth ;
            else if (_tVector3.y >= 225 && _tVector3.y < 315)
                return OrangeEast;
            else
                return OrangeSouth ;
        }
    }
}
