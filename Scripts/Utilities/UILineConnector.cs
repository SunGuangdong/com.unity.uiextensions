﻿/// Credit Alastair Aitchison
/// Sourced from - https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/issues/123/uilinerenderer-issues-with-specifying

namespace UnityEngine.UI.Extensions
{
    [RequireComponent(typeof(UILineRenderer))]
    [ExecuteInEditMode]
    public class UILineConnector : MonoBehaviour
    {

        // The elements between which line segments should be drawn
        public RectTransform[] transforms;
        private RectTransform canvas;
        private RectTransform rt;
        private UILineRenderer lr;

        private void Awake()
        {
            canvas = GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
            rt = GetComponent<RectTransform>();
            lr = GetComponent<UILineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transforms == null || transforms.Length < 1)
            {
                return;
            }

            // Get the pivot points
            Vector2 thisPivot = rt.pivot;
            Vector2 canvasPivot = canvas.pivot;

            // Set up some arrays of coordinates in various reference systems
            Vector3[] worldSpaces = new Vector3[transforms.Length];
            Vector3[] canvasSpaces = new Vector3[transforms.Length];
            Vector2[] points = new Vector2[transforms.Length];

            // First, convert the pivot to worldspace
            for (int i = 0; i < transforms.Length; i++)
            {
                worldSpaces[i] = transforms[i].TransformPoint(thisPivot);
            }

            // Then, convert to canvas space
            for (int i = 0; i < transforms.Length; i++)
            {
                canvasSpaces[i] = canvas.InverseTransformPoint(worldSpaces[i]);
            }

            // Calculate delta from the canvas pivot point
            for (int i = 0; i < transforms.Length; i++)
            {
                points[i] = new Vector2(canvasSpaces[i].x, canvasSpaces[i].y);
            }

            // And assign the converted points to the line renderer
            lr.Points = points;
            lr.relativeSize = false;
            lr.drivenExternally = true;
        }
    }
}