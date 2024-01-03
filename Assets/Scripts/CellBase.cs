using System;
using System.Collections.Generic;
using UnityEngine;

namespace board
{
    public class CellBase : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private Cell cell;

        public int CellBaseId => _id;
        public int RemaingCellCount => _cells.Count - _filledCellCount;
        public bool IsFull => _isFull;

        public event Action OnCellBaseFull;

        private int _id;
        private List<Cell> _cells;
        private int _filledCellCount = 0;
        private bool _isFull;

        public void SetData(int id, int noOfCells, float cellWidth, float cellHeight)
        {
            _id = id;
            _cells = new List<Cell>();

            for(int i = 0; i<noOfCells;i++)
            {
                Cell tempCell = Instantiate(cell, transform);
                _cells.Add(tempCell);
            }

            float totalHeight = cellHeight * noOfCells;
            SetBoxColliderData(-(totalHeight/2 - cellHeight/2), totalHeight);
        }

        public void FillCell()
        {
            _filledCellCount++;
            _isFull = _cells.Count == _filledCellCount;

            if(_isFull)
            {
                OnCellBaseFull?.Invoke();
            }
        } 

        public void ResetData()
        {
            _isFull = false;
            _filledCellCount = 0;
        }

        private void SetBoxColliderData(float offSetY, float sizeYValue)
        {
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, offSetY);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, sizeYValue);
        }
    }
}


