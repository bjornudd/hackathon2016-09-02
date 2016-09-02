using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
	public class LineBrush : GridBrush {
        public bool lineStartActive = false;
		public Vector3Int lineStart = Vector3Int.zero;

        public override void Paint(IGridLayout grid, Vector3Int position)
		{
            if (lineStartActive)
            {
                Vector2Int startPos = new Vector2Int(lineStart.x, lineStart.y);
                Vector2Int endPos = new Vector2Int(position.x, position.y);
                if (startPos == endPos)
                    base.Paint(grid, position);    
                else
                {
                    foreach (var point in GetPointsOnLine(startPos, endPos))
                    {
                        Vector3Int paintPos = new Vector3Int(point.x, point.y, position.z);
                        base.Paint(grid, paintPos);
                    }
                }
                lineStartActive = false;
            }
            else
            {
                lineStart = position;
                lineStartActive = true;
            }
		}
        
        public override void OnPaintSceneGUI(IGridLayout grid, BoundsInt position, Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(grid, position, tool, executing);
            if (lineStartActive)
            {
                // Draw preview tiles for tilemap
                if (tilemap != null)
                {
                    tilemap.ClearAllEditorPreviewTiles();
                    Vector2Int startPos = new Vector2Int(lineStart.x, lineStart.y);
                    Vector2Int endPos = new Vector2Int(position.x, position.y);
                    if (startPos == endPos)
                        base.PaintPreview(grid, position);    
                    else
                    {
                        foreach (var point in GetPointsOnLine(startPos, endPos))
                        {
                            Vector3Int paintPos = new Vector3Int(point.x, point.y, position.z);
                            BoundsInt previewPos = new BoundsInt(paintPos, position.size);
                            base.PaintPreview(grid, previewPos);
                        }
                    }
                }
                
                if (Event.current.type == EventType.Repaint)
                {
                    var min = lineStart;
                    var max = lineStart + position.size;
                    
                    // Draws a box on the picked starting position
                    GL.PushMatrix();
                    GL.MultMatrix(GUI.matrix);
                    GL.Begin(GL.LINES);
                    Handles.color = Color.blue;
                    Handles.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z));
                    Handles.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z));
                    Handles.DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(min.x, max.y, min.z));
                    Handles.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, min.y, min.z));
                    GL.End();
                    GL.PopMatrix();
                }
            }
        }

		[MenuItem("Assets/Create/Line Brush")]
		public static void CreateBrush()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Line Brush", "New Line Brush", "asset", "Save Line Brush", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<LineBrush>(), path);
		}
        
        // http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
		private static IEnumerable<Vector2Int> GetPointsOnLine(Vector2Int p1, Vector2Int p2)
		{
			int x0 = p1.x;
			int y0 = p1.y;
			int x1 = p2.x;
			int y1 = p2.y;

			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep)
			{
				int t;
				t = x0; // swap x0 and y0
				x0 = y0;
				y0 = t;
				t = x1; // swap x1 and y1
				x1 = y1;
				y1 = t;
			}
			if (x0 > x1)
			{
				int t;
				t = x0; // swap x0 and x1
				x0 = x1;
				x1 = t;
				t = y0; // swap y0 and y1
				y0 = y1;
				y1 = t;
			}
			int dx = x1 - x0;
			int dy = Math.Abs(y1 - y0);
			int error = dx / 2;
			int ystep = (y0 < y1) ? 1 : -1;
			int y = y0;
			for (int x = x0; x <= x1; x++)
			{
				yield return new Vector2Int((steep ? y : x), (steep ? x : y));
				error = error - dy;
				if (error < 0)
				{
					y += ystep;
					error += dx;
				}
			}
			yield break;
		}
	}
}
