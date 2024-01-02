using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardData
{
    public int WinCount { get; set; }

    public bool IsMatchFound => _matchFound;
    public List<int> MatchedIndexes => _matchFoundIndexes;

    private List<List<int>> _cells;
    private int _foundCount = 1;
    private int _maxRows = 1;
    private int _maxCols = 1;
    private bool _matchFound;
    private List<int> _matchFoundIndexes;

    public void CreateBoard(int rows, int cols)
    {
        _cells = new List<List<int>>();
        _matchFoundIndexes = new List<int>();
        _maxRows = rows;
        _maxCols = cols;

        for (var i = 0; i < cols; i++)
        {
            List<int> rowsList = new List<int>();
            for (var j = 0; j < rows; j++)
            {
                rowsList.Add(0);
            }
            _cells.Add(rowsList);
        }
    }

    public void ResetData()
    {
        RestartSearch();
         for (var i = 0; i < _maxCols; i++)
        {
            for (var j = 0; j < _maxRows; j++)
            {
                _cells[i][j] = 0;   
            }
        }
    }

    public void UpdateCell(bool isPlayer, int cols, int rows)
    {
        _cells[cols][rows] = isPlayer ? -1 : -2;
        CheckConnection(cols, rows, isPlayer ? -1 : -2);
    }

    private void CheckConnection(int colIndex, int rowIndex, int searchItem)
    {
        RestartSearch(colIndex, rowIndex);
        CheckHorizontalMatch(colIndex, rowIndex, searchItem);

        Debug.Log("Checking Match With Down : Match : " + _matchFound);
        if (!_matchFound)
        {
            RestartSearch(colIndex, rowIndex);
            CheckVerticalMatch(colIndex, rowIndex, searchItem);
        }

        Debug.Log("Checking Match With Digonally : Match : " + _matchFound);
        if (!_matchFound)
        {
            RestartSearch(colIndex, rowIndex);
            CheckDigonally(colIndex, rowIndex, searchItem);
        }

        if (!_matchFound)
            RestartSearch();
    }

    private void CheckHorizontalMatch(int colIndex, int rowIndex, int searchItem)
    {
        //Check For Right
        Debug.Log("Checking Match With Right : Match : " + _matchFound);
        if (CheckLeftOrRight(colIndex, rowIndex, searchItem, 1))
        {
            Debug.Log("Item Found Right");
            _matchFound = true;
        }

        Debug.Log("Checking Match With Left : Match : " + _matchFound);
        if (!_matchFound && CheckLeftOrRight(colIndex, rowIndex, searchItem, -1))
        {
            Debug.Log("Item Found Left");
            _matchFound = true;
        }
    }

    private void CheckVerticalMatch(int colIndex, int rowIndex, int searchItem)
    {
        if (CheckDown(colIndex, rowIndex, searchItem))
        {
            Debug.Log("Item Found Vertiaclly");
            _matchFound = true;
        }
    }


    private bool CheckLeftOrRight(int colIndex, int rowIndex, int searchItem, int dir)
    {
        string direction = dir > 0 ? "Right" : "Left";

        Debug.Log($"{direction} : ColIndex : {colIndex} : rowIndex : {rowIndex} : count : {_cells[colIndex].Count}: value : {_cells[colIndex][rowIndex]}");
        colIndex += dir;
        Debug.Log($"{direction} : ColIndex : {colIndex} : search Item : {searchItem}");

        if ((dir > 0 && colIndex < _cells.Count || dir < 0 && colIndex >= 0) && _cells[colIndex][rowIndex] == searchItem)
        {
            _foundCount++;
            _matchFoundIndexes.Add(_cells[colIndex].Count * colIndex + rowIndex);
            Debug.Log($"{direction} : Found Count : {_foundCount}");

            if (!(_foundCount == WinCount))
                CheckLeftOrRight(colIndex, rowIndex, searchItem, dir);
        }

        Debug.Log($"{direction} : Dir : {dir} ----------- ");
        return _foundCount == WinCount;
    }

    private bool CheckDown(int colIndex, int rowIndex, int searchItem)
    {
        Debug.Log($"Down : ColIndex : {colIndex} : rowIndex : {rowIndex} : count : {_cells[colIndex].Count}: value : {_cells[colIndex][rowIndex]}");
        rowIndex++;
        Debug.Log($"Down : rowIndex : {rowIndex} : search Item : {searchItem}");

        if (rowIndex < _cells[colIndex].Count && _cells[colIndex][rowIndex] == searchItem)
        {
            _foundCount++;
            _matchFoundIndexes.Add(_cells[colIndex].Count * colIndex + rowIndex);
            Debug.Log($"Down : Found Count : {_foundCount}");

            if (!(_foundCount == WinCount))
                CheckDown(colIndex, rowIndex, searchItem);
        }

        return _foundCount == WinCount;
    }

    private void CheckDigonally(int colIndex, int rowIndex, int searchItem)
    {
        if (CheckDigonallyUpSide(colIndex, rowIndex, searchItem, 1))
        {
            _matchFound = true;
            Debug.Log("Item Found Digonally Up Right");
        }

        if (!_matchFound)
        {
            RestartSearch(colIndex, rowIndex);
            if (CheckDigonallyUpSide(colIndex, rowIndex, searchItem, -1))
            {
                _matchFound = true;
                Debug.Log("Item Found Digonally Up left");
            }
        }

        if (!_matchFound)
        {
            RestartSearch(colIndex, rowIndex);
            if (CheckDigonallyDownSide(colIndex, rowIndex, searchItem, 1))
            {
                _matchFound = true;
                Debug.Log("Item Found Digonally Donw Right");
            }
        }

        if (!_matchFound)
        {
            RestartSearch(colIndex, rowIndex);
            if (CheckDigonallyDownSide(colIndex, rowIndex, searchItem, -1))
            {
                _matchFound = true;
                Debug.Log("Item Found Digonally Down left");
            }
        }


    }

    private bool CheckDigonallyUpSide(int colIndex, int rowIndex, int searchItem, int dir)
    {
        string dirString = "Left";
        if (dir > 0)
            dirString = "Right";

        Debug.Log($"Digonally {dirString} ColIndex : {colIndex} : rowIndex : {rowIndex} : count : {_cells[colIndex].Count}: value : {_cells[colIndex][rowIndex]}");
        colIndex += dir;
        if (dir > 0)
            rowIndex -= dir;
        else
            rowIndex += dir;

        Debug.Log($"Digonally {dirString} ColIndex : {colIndex} : rowIndex : {rowIndex} : search Item : {searchItem}");

        if ((dir > 0 && colIndex < _cells.Count && rowIndex >= 0
            || dir < 0 && colIndex >= 0 && rowIndex >= 0)
            && _cells[colIndex][rowIndex] == searchItem)
        {
            _foundCount++;
            _matchFoundIndexes.Add(_cells[colIndex].Count * colIndex + rowIndex);
            Debug.Log($"Digonally {dirString} Found Count : {_foundCount}");

            if (!(_foundCount == WinCount))
                CheckDigonallyUpSide(colIndex, rowIndex, searchItem, dir);
        }

        Debug.Log($"Digonally {dirString} Dir : {dir} : ----------- ");
        return _foundCount == WinCount;
    }

    private bool CheckDigonallyDownSide(int colIndex, int rowIndex, int searchItem, int dir)
    {
        string dirString = "Left";
        if (dir > 0)
            dirString = "Right";

        Debug.Log($"Digonally Down {dirString} ColIndex : {colIndex} : rowIndex : {rowIndex} : count : {_cells[colIndex].Count}: value : {_cells[colIndex][rowIndex]}");
        colIndex += dir;
        if (dir > 0)
            rowIndex += dir;
        else
            rowIndex -= dir;

        Debug.Log($"Digonally Donw {dirString} ColIndex : {colIndex} : rowIndex : {rowIndex} : search Item : {searchItem}");

        if ((dir > 0 && colIndex < _cells.Count && rowIndex < _cells[colIndex].Count
            || dir < 0 && colIndex >= 0 && rowIndex < _cells[colIndex].Count)
            && _cells[colIndex][rowIndex] == searchItem)
        {
            _foundCount++;
            _matchFoundIndexes.Add(_cells[colIndex].Count * colIndex + rowIndex);
            Debug.Log($"Digonally Donw {dirString} Found Count : {_foundCount}");

            if (!(_foundCount == WinCount))
                CheckDigonallyDownSide(colIndex, rowIndex, searchItem, dir);
        }

        Debug.Log($"Digonally Donw {dirString} Dir : {dir} : ----------- ");
        return _foundCount == WinCount;
    }


    private void RestartSearch(int colIndex = -1, int rowIndex = -1)
    {
        _foundCount = 1;
        _matchFound = false;
        _matchFoundIndexes.Clear();

        if (colIndex > -1 && rowIndex > -1)
            _matchFoundIndexes.Add(_cells[colIndex].Count * colIndex + rowIndex);
    }

}