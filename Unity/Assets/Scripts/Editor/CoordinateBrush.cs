using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
	public class CoordinateBrush : GridBrush {
		public int z = 0;

        public override void Paint(IGridLayout grid, Vector3Int position)
		{
            var zPosition = new Vector3Int(position.x, position.y, z);
			base.Paint(grid, zPosition);
		}
        
        public override void PaintPreview(IGridLayout grid, BoundsInt position)
        {
            var zPosition = new Vector3Int(position.min.x, position.min.y, z);
            BoundsInt newPosition = new BoundsInt(zPosition, position.size);
            base.PaintPreview(grid, newPosition);
        }
        
		public override void Erase(IGridLayout grid, Vector3Int position)
		{
            var zPosition = new Vector3Int(position.x, position.y, z);
			base.Erase(grid, zPosition);
		}
        
        public override void FloodFill(IGridLayout grid, Vector3Int position)
        {
            var zPosition = new Vector3Int(position.x, position.y, z);
            base.FloodFill(grid, zPosition);
        }

		public override void OnPaintInspectorGUI()
		{
			base.OnPaintInspectorGUI();
			z = EditorGUILayout.IntField("Z", z);
		}
        
        public override void OnPaintSceneGUI(IGridLayout grid, BoundsInt position, Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(grid, position, tool, executing);
            if (z != 0)
            {
                var zPosition = new Vector3Int(position.min.x, position.min.y, z);
                BoundsInt newPosition = new BoundsInt(zPosition, position.size);
                Vector3[] cellLocals = new Vector3[]
                {
                    grid.CellToLocal(new Vector3Int(newPosition.min.x, newPosition.min.y, newPosition.min.z)),
                    grid.CellToLocal(new Vector3Int(newPosition.max.x, newPosition.min.y, newPosition.min.z)),
                    grid.CellToLocal(new Vector3Int(newPosition.max.x, newPosition.max.y, newPosition.min.z)),
                    grid.CellToLocal(new Vector3Int(newPosition.min.x, newPosition.max.y, newPosition.min.z))
                };
                Handles.color = z > 0 ? Color.blue : Color.red;
                int i = 0;
                for (int j = cellLocals.Length - 1; i < cellLocals.Length; j = i++)
                {
                    Handles.DrawLine(cellLocals[j], cellLocals[i]);
                }
            }
            Handles.Label(grid.CellToWorld(new Vector3Int(position.x, position.y, z)), new Vector3Int(position.x, position.y, z).ToString());
        }

		[MenuItem("Assets/Create/Coordinate Brush")]
		public static void CreateBrush()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Coordinate Brush", "New Coordinate Brush", "asset", "Save Coordinate Brush", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CoordinateBrush>(), path);
		}
	}
}
