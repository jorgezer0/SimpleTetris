using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    [SerializeField] private Vector3 rotationPoint;
    private float _previousTime;
    public static float fallTime = 0.8f;
    public static int Height = 20;
    public static int Width = 10;
    
    private static Transform[,] _grid = new Transform[Width,Height];

    private void Start()
    {
        EventBus.OnLevelUp += OnLevelUp;

        if (ValidMove()) return;
        Debug.Log("GAME OVER!!!");
        EventBus.RaiseGameOver(this);
        enabled = false;
    }

    private void OnLevelUp(object sender, EventArgs e)
    {
        fallTime -= 0.025f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            if(!ValidMove())
                transform.position -= Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= Vector3.right;
            if(!ValidMove())
                transform.position += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if(!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Dropdown();
        }

        if (Time.time - _previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime/10 : fallTime))
        {
            transform.position += Vector3.down;
            if (!ValidMove())
            {
                transform.position += Vector3.up;
                EndBlockMove();
            }

            _previousTime = Time.time;
        }
    }

    private void Dropdown()
    {
        bool isDroped = false;
        while (!isDroped)
        {
            transform.position += Vector3.down;
            if (ValidMove()) continue;
            transform.position += Vector3.up;
            EndBlockMove();
            isDroped = true;
        }
    }
    
    private void EndBlockMove()
    {
        EventBus.RaiseBlockDrop(this);
        AddToGrid();
        ClearTransform();
        CheckLines();
        this.enabled = false;
        EventBus.RaiseCallNewBlock(this);
        EventBus.OnLevelUp -= OnLevelUp;
    }

    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var xCoord = Mathf.RoundToInt(children.position.x);
            var yCoord = Mathf.RoundToInt(children.position.y);

            if (xCoord < 0 || xCoord >= Width || yCoord < 0 || yCoord >= Height)
            {
                return false;
            }

            if (_grid[xCoord, yCoord] != null)
                return false;
        }

        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            var xCoord = Mathf.RoundToInt(children.position.x);
            var yCoord = Mathf.RoundToInt(children.position.y);

            _grid[xCoord, yCoord] = children;
        }
    }

    private void ClearTransform()
    {
        foreach (Transform children in transform.Cast<Transform>().ToList())
        {
            Debug.Log($"{children.name}");
            children.SetParent(null);
        }
        Destroy(gameObject);
    }

    private void CheckLines()
    {
        var combo = 0;
        for (var i = Height-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                RemoveLine(i, combo);
                DownRow(i);
                combo++;
            }
        }

        if (combo > 0)
            EventBus.RaiseAddPoints(this, combo);
    }

    private bool HasLine(int i)
    {
        for (var j = 0; j < Width; j++)
        {
            if (_grid[j, i] == null)
                return false;
        }

        return true;
    }

    private void RemoveLine(int i, int combo)
    {
        for (var j = 0; j < Width; j++)
        {
            Destroy(_grid[j,i].gameObject);
            _grid[j, i] = null;
        }
    }

    private void DownRow(int i)
    {
        for (var y = i; y < Height; y++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (_grid[j, y] == null) continue;
                _grid[j, y - 1] = _grid[j, y];
                _grid[j, y] = null;
                _grid[j,y-1].position += Vector3.down;
            }
        }
    }
}
