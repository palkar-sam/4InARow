using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

namespace board
{
    public class Board : MonoBehaviour
    {
        [SerializeField][Range(3, 8)] private int maxRows;
        [SerializeField][Range(3, 8)] private int maxColls;
        [SerializeField] private Transform cellContainer;
        [SerializeField] private BoxCollider2D baseCollider;
        [SerializeField] private CellBase cellBase;
        [SerializeField] private Ball targetBall;
        [SerializeField] private Ball guestTargetBall;
        [SerializeField] private Transform targetBallCantainer;
        [SerializeField] private Text roundStatusText;
        [SerializeField] private float cellWidth = 100;
        [SerializeField] private float cellHeight = 100;
        [SerializeField] private int maxWinCount = 4;
        [SerializeField] private string playerText = "Player";
        [SerializeField] private string guestText = "Guest";

        public event Action OnBoardFull;
        public event Action<bool> OnMatchFound;

        private BoardData _boardData;
        private List<CellBase> _cellBases;
        private List<int> _cellBaseIndexes;
        private List<Ball> _dropedBalls;
        private float _boardScale = 1;
        private float _boardTotalHeight = 0;
        private bool _isBallDropping;
        private bool _isPlayer = true;
        private bool _isStartRound = true;
        private int _cellBaseFullCount;
        private GameStatus _roundType;

        public void SetVisibility(bool flag)
        {
            gameObject.SetActive(flag);
            SetRoundStatusVisibility(flag);
        }

        public void RestartGame(bool isStartNewRouund = true)
        {
            ClearData();
            _isStartRound = isStartNewRouund;
            SetRoundStatusVisibility(isStartNewRouund);
            SetRoundText(playerText);
        }

        public void PlayRound(CellBase targetObject)
        {
            if (!_isStartRound || _isBallDropping)
                return;

            CellBase cellBaseObj = _cellBases.Find(cellBaseItem => cellBaseItem.CellBaseId == targetObject.CellBaseId);
            cellBaseObj.FillCell();
            _isBallDropping = true;
            Vector3 targetPos = targetObject.transform.position;//new Vector3(0,targetObject.transform.position.y,0);
            Ball ball = Instantiate(_isPlayer ? targetBall : guestTargetBall, targetPos, Quaternion.identity, targetBallCantainer);
            ball.TargetPosition = new Vector3(cellBaseObj.CellBaseId * cellWidth, -cellBaseObj.RemaingCellCount * cellHeight, 0);
            ball.transform.localPosition += new Vector3(0, cellHeight, 0);
            ball.OnBallDropFinished += OnBallDropFinished;

            _dropedBalls[maxRows * cellBaseObj.CellBaseId + cellBaseObj.RemaingCellCount] = ball;
            _boardData.UpdateCell(_isPlayer, cellBaseObj.CellBaseId, cellBaseObj.RemaingCellCount);
        }

        public void StartRound(GameStatus roundType)
        {
            _isStartRound = true;
            _roundType = roundType;
            SetVisibility(true);
            SetRoundText(playerText);
        }
        private void SetRoundText(string str)
        {
            roundStatusText.text = "Round Turn : " + str;
        }

        private void ClearData()
        {
            _isBallDropping = false;
            _isPlayer = true;
            _boardData.ResetData();

            Ball tempBall = null;
            for (int i = 0; i < _dropedBalls.Count; i++)
            {
                tempBall = _dropedBalls[i];
                if (tempBall != null)
                {
                    _dropedBalls[i].Clear();
                    _dropedBalls[i].OnBallDropFinished -= OnBallDropFinished;
                    Destroy(_dropedBalls[i].gameObject);
                }

                _dropedBalls[i] = null;
            }

            tempBall = null;
            _cellBaseIndexes.Clear();
            _cellBaseFullCount = 0;

            for (int i = 0; i < _cellBases.Count; i++)
            {
                _cellBaseIndexes.Add(i);
                _cellBases[i].ResetData();
            }
        }

        [Obsolete]
        private CellBase GetRondomCellBase()
        {
            int index = UnityEngine.Random.RandomRange(0, _cellBaseIndexes.Count - 1);
            CellBase cellBaseItem = _cellBases[_cellBaseIndexes[index]];
            if (cellBaseItem.IsFull)
            {
                _cellBaseIndexes.RemoveAt(index);
                cellBaseItem = GetRondomCellBase();
            }

            return cellBaseItem;
        }

        private IEnumerator PlayGuest()
        {
            yield return new WaitForSeconds(0.2f);
            CellBase cellbaseObj = GetRondomCellBase();
            PlayRound(cellbaseObj);
        }

        private void OnBallDropFinished(Ball ball)
        {
            ball.OnBallDropFinished -= OnBallDropFinished;

            if (_boardData.IsMatchFound)
            {
                _isStartRound = false;
                SetRoundStatusVisibility(false);
                OnMatchFound?.Invoke(_isPlayer);
                PlayMatchAnimation();
            }
            else if (_cellBaseFullCount == _cellBases.Count)
            {
                _isStartRound = false;
                SetRoundStatusVisibility(false);
                OnBoardFull?.Invoke();
            }
            else
            {
                _isBallDropping = false;

                if (_isPlayer)
                    StartCoroutine(PlayGuest());

                SetRoundText(_isPlayer ? guestText : playerText);
                _isPlayer = !_isPlayer;
            }
        }

        private void CreateBoard()
        {
            _boardData = new BoardData();
            _boardData.WinCount = maxWinCount;
            _boardData.CreateBoard(maxRows, maxColls);

            _cellBases = new List<CellBase>();
            _dropedBalls = new List<Ball>();
            _cellBaseIndexes = new List<int>();

            for (int i = 0; i < maxColls; i++)
            {
                CellBase tempCellBase = Instantiate(cellBase, cellContainer);
                tempCellBase.SetData(i, maxRows, cellWidth, cellHeight);
                tempCellBase.OnCellBaseFull += OnCellBaseFull;
                _cellBases.Add(tempCellBase);
                _cellBaseIndexes.Add(i);
            }

            float scaleX = cellContainer.localScale.x;
            float scaleY = cellContainer.localScale.y;
            float xPos = -(cellWidth * (maxColls - 1)) / 2 * scaleX;
            float yPos = (cellHeight * (maxRows - 1)) / 2 * scaleY;
            Vector3 newPos = new Vector3(xPos, yPos, 0);
            cellContainer.localPosition = targetBallCantainer.localPosition = newPos;


            baseCollider.size = new Vector2(cellWidth * maxColls, baseCollider.size.y);
            baseCollider.transform.localPosition = new Vector3(0, -yPos - cellHeight / 2 + baseCollider.size.y, 0);

            for (var i = 0; i < maxColls; i++)
            {
                for (var j = 0; j < maxRows; j++)
                {
                    _dropedBalls.Add(null);
                }
            }
        }

        private void OnCellBaseFull()
        {
            _cellBaseFullCount++;
        }

        private void PlayMatchAnimation()
        {
            for (int i = 0; i < _boardData.MatchedIndexes.Count; i++)
            {
                _dropedBalls[_boardData.MatchedIndexes[i]].PlayCollectAnim();
            }
        }

        private void Start()
        {
            if (maxWinCount > maxRows || maxWinCount > maxColls)
            {
                maxWinCount = Mathf.Min(maxRows, maxColls);
            }

            CreateBoard();
            SetRoundStatusVisibility(_isStartRound);
        }

        private void SetRoundStatusVisibility(bool flag)
        {
            roundStatusText.gameObject.SetActive(flag);
        }
    }
}


