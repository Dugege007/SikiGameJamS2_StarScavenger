using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

namespace StarScavenger
{
    public partial class GridController : ViewController
    {
        private Grid mGrid;

        private Tilemap mUp;
        private Tilemap mDown;
        private Tilemap mLeft;
        private Tilemap mRight;
        private Tilemap mUpLeft;
        private Tilemap mUpRight;
        private Tilemap mDownLeft;
        private Tilemap mDownRight;
        private Tilemap mCenter;

        private int mAreaX = 0;
        private int mAreaY = 0;

        private void Start()
        {
            mGrid = GetComponent<Grid>();

            // 重新生成 Tilemap 边界
            Tilemap.CompressBounds();

            CreateTileMaps();
            UpdatePositions();
        }

        private void Update()
        {
            if (Player.Default && Time.frameCount % 60 == 0)
            {
                // 将角色坐标转换为 Tilemap 坐标
                Vector3Int cellPos = Tilemap.layoutGrid.WorldToCell(Player.Default.transform.position);
                mAreaX = cellPos.x / Tilemap.size.x;
                mAreaY = cellPos.y / Tilemap.size.y;
                UpdatePositions();
            }
        }

        private void UpdatePositions()
        {
            float sizeX = Tilemap.size.x * mGrid.cellSize.x;
            float sizeY = Tilemap.size.y * mGrid.cellSize.y;

            mUp.Position(new Vector3(mAreaX * sizeX, (mAreaY + 1) * sizeY));
            mDown.Position(new Vector3(mAreaX * sizeX, (mAreaY - 1) * sizeY));
            mLeft.Position(new Vector3((mAreaX - 1) * sizeX, mAreaY * sizeY));
            mRight.Position(new Vector3((mAreaX + 1) * sizeX, mAreaY * sizeY));
            mUpLeft.Position(new Vector3((mAreaX - 1) * sizeX, (mAreaY + 1) * sizeY));
            mUpRight.Position(new Vector3((mAreaX + 1) * sizeX, (mAreaY + 1) * sizeY));
            mDownLeft.Position(new Vector3((mAreaX - 1) * sizeX, (mAreaY - 1) * sizeY));
            mDownRight.Position(new Vector3((mAreaX + 1) * sizeX, (mAreaY - 1) * sizeY));
            mCenter.Position(new Vector3(mAreaX * sizeX, mAreaY * sizeY));
        }

        private void CreateTileMaps()
        {
            mUp = Tilemap.InstantiateWithParent(transform);
            mDown = Tilemap.InstantiateWithParent(transform);
            mLeft = Tilemap.InstantiateWithParent(transform);
            mRight = Tilemap.InstantiateWithParent(transform);
            mUpLeft = Tilemap.InstantiateWithParent(transform);
            mUpRight = Tilemap.InstantiateWithParent(transform);
            mDownLeft = Tilemap.InstantiateWithParent(transform);
            mDownRight = Tilemap.InstantiateWithParent(transform);
            mCenter = Tilemap;
        }
    }
}
