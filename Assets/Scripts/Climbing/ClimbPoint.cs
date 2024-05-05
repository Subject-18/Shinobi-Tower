using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{   
    [SerializeField] bool mountPoint;
    [SerializeField] List<Neighbour> neighbours;
   

   void Awake()
   {
        var twowayneighbours = neighbours.Where(n => n.isTwoWay);
        foreach ( var neighbour in twowayneighbours)
        {
            neighbour.climbPoint?.CreateConnections(this, -neighbour.direction,
             neighbour.connectiontype, neighbour.isTwoWay);
        }
   }

   public void CreateConnections( ClimbPoint Point, Vector2 dir, 
   Connection connection, bool istwoWay = true)
   {
        var neighbour = new Neighbour()
        {
            climbPoint = Point,
            direction = dir,
            connectiontype = connection,
            isTwoWay = istwoWay
        };

        neighbours.Add(neighbour);
   }

   public Neighbour GetNeighbour(Vector2 direction)
   {
        Neighbour neighbour = null;
        if(direction.y != 0)
        neighbour = neighbours.FirstOrDefault(n=> n.direction.y == direction.y);

        if(neighbour == null && direction.x!=0)
        neighbour = neighbours.FirstOrDefault(n=> n.direction.x == direction.x);

        return neighbour;
   }

   void OnDrawGizmos()
   {
        Debug.DrawRay(transform.position, transform.forward, Color.blue );
        foreach (var neighbour in neighbours)
        {
            if(neighbour.climbPoint != null)
            {
                Debug.DrawLine(transform.position, neighbour.climbPoint.transform.position, (neighbour.isTwoWay)? Color.white : Color.green );
            }
        }
   }
   
    public bool MountPoint => mountPoint;

}
[System.Serializable]
public class Neighbour{

    public ClimbPoint climbPoint;
    public Vector2 direction;

    public Connection connectiontype;

    public bool isTwoWay = true;
}

public enum Connection{ jump, move}