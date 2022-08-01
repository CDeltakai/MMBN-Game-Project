using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage_MoveableEntity
{

    public bool checkValidTile(int x, int y, int z);
    public void cellMoveUp();
    public void cellMoveDown();
    public void cellMoveRight();
    public void cellMoveLeft();
    



}
