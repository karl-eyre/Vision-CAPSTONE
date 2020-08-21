using InDevelopment.Mechanics.LineOfSight;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(LineOfSight))]
    
    public class LineOfSightEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            LineOfSight fov = (LineOfSight) target;
            Handles.color = Color.cyan;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewDistance);
            Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
            Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewDistance);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewDistance);

        }
    }
}
