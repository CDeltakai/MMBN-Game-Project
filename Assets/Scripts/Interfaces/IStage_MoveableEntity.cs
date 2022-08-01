using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage_MoveableEntity
{

    public void checkValidTile(Vector3Int pos);
    public void moveUp();
    public void moveDown();
    public void moveRight();
    public void moveLeft();
    



}
