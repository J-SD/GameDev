using UnityEngine;
using UnityEngine.Tilemaps;

// Tile that instantiates a GameObject on Start and assigns a random rotation to the instanced GameObject
[CreateAssetMenu]
public class PlantTile : WorldTile
{
   public override void Awake() {
        base.Awake();
        this.type = TileTypes.plant;
    }
    
}