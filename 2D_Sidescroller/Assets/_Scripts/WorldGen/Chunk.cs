using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chunk
{
    public int width;
    public int height;
    public int horizontalIndex;
    public int verticalIndex;

    public int[,] ground;
    public int[,] plants;
    public int[,] enemies;

    public Chunk(int width, int height, int horizontalIndex, int verticalIndex) : this()
    {
        this.width = width;
        this.height = height;
        this.horizontalIndex = horizontalIndex;
        this.verticalIndex = verticalIndex;
    }
}
