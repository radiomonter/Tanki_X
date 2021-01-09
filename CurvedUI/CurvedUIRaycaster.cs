namespace CurvedUI
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CurvedUIRaycaster : GraphicRaycaster
    {
        [SerializeField]
        private bool showDebug;
        private bool overrideEventData = true;
        private Canvas myCanvas;
        private CurvedUISettings mySettings;
        private Vector3 cyllinderMidPoint;
        private List<GameObject> objectsUnderPointer = new List<GameObject>();
        private Vector2 lastCanvasPos = Vector2.zero;
        private GameObject colliderContainer;
        private List<GameObject> selectablesUnderGaze = new List<GameObject>();
        private List<GameObject> selectablesUnderGazeLastFrame = new List<GameObject>();
        private float objectsUnderGazeLastChangeTime;
        private bool gazeClickExecuted;
        private bool pointingAtCanvas;
        [CompilerGenerated]
        private static Predicate<GameObject> <>f__am$cache0;
        [CompilerGenerated]
        private static Comparison<Graphic> <>f__am$cache1;

        private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) => 
            Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;

        protected override void Awake()
        {
            base.Awake();
            this.myCanvas = base.GetComponent<Canvas>();
            this.mySettings = base.GetComponent<CurvedUISettings>();
            this.cyllinderMidPoint = new Vector3(0f, 0f, -this.mySettings.GetCyllinderRadiusInCanvasSpace());
            if ((this.myCanvas.worldCamera == null) && (Camera.main != null))
            {
                this.myCanvas.worldCamera = Camera.main;
            }
        }

        public void Click()
        {
            for (int i = 0; i < this.GetObjectsUnderPointer().Count; i++)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(this.GetObjectsUnderPointer()[i], new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
        }

        protected void CreateCollider()
        {
            List<Collider> list = new List<Collider>();
            list.AddRange(base.GetComponents<Collider>());
            for (int i = 0; i < list.Count; i++)
            {
                Destroy(list[i]);
            }
            if (this.mySettings.BlocksRaycasts && ((this.mySettings.Shape != CurvedUISettings.CurvedUIShape.SPHERE) || (this.mySettings.PreserveAspect || (this.mySettings.VerticalAngle != 0))))
            {
                switch (this.mySettings.Shape)
                {
                    case CurvedUISettings.CurvedUIShape.CYLINDER:
                        if (!this.mySettings.ForceUseBoxCollider && ((base.GetComponent<Rigidbody>() == null) && (base.GetComponentInParent<Rigidbody>() == null)))
                        {
                            this.SetupMeshColliderUsingMesh(this.CreateCyllinderColliderMesh(false));
                        }
                        else
                        {
                            if (this.colliderContainer != null)
                            {
                                Destroy(this.colliderContainer);
                            }
                            this.colliderContainer = this.CreateConvexCyllinderCollider(false);
                        }
                        return;

                    case CurvedUISettings.CurvedUIShape.RING:
                        base.gameObject.AddComponent<BoxCollider>().size = new Vector3((float) this.mySettings.RingExternalDiameter, (float) this.mySettings.RingExternalDiameter, 1f);
                        return;

                    case CurvedUISettings.CurvedUIShape.SPHERE:
                        if ((base.GetComponent<Rigidbody>() != null) || (base.GetComponentInParent<Rigidbody>() != null))
                        {
                            Debug.LogWarning("CurvedUI: Sphere shape canvases as children of rigidbodies do not support user input. Switch to Cyllinder shape or remove the rigidbody from parent.", base.gameObject);
                        }
                        this.SetupMeshColliderUsingMesh(this.CreateSphereColliderMesh());
                        return;

                    case CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL:
                        if (!this.mySettings.ForceUseBoxCollider && ((base.GetComponent<Rigidbody>() == null) && (base.GetComponentInParent<Rigidbody>() == null)))
                        {
                            this.SetupMeshColliderUsingMesh(this.CreateCyllinderColliderMesh(true));
                        }
                        else
                        {
                            if (this.colliderContainer != null)
                            {
                                Destroy(this.colliderContainer);
                            }
                            this.colliderContainer = this.CreateConvexCyllinderCollider(true);
                        }
                        return;
                }
            }
        }

        private unsafe GameObject CreateConvexCyllinderCollider(bool vertical = false)
        {
            GameObject obj2 = new GameObject("_CurvedUIColliders") {
                layer = base.gameObject.layer
            };
            obj2.transform.SetParent(base.transform);
            obj2.transform.ResetTransform();
            Mesh mesh = new Mesh();
            Vector3[] fourCornersArray = new Vector3[4];
            (this.myCanvas.transform as RectTransform).GetWorldCorners(fourCornersArray);
            mesh.vertices = fourCornersArray;
            if (vertical)
            {
                fourCornersArray[0] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[1]);
                fourCornersArray[1] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[2]);
                fourCornersArray[2] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[0]);
                fourCornersArray[3] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[3]);
            }
            else
            {
                fourCornersArray[0] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[1]);
                fourCornersArray[1] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[0]);
                fourCornersArray[2] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[2]);
                fourCornersArray[3] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[3]);
            }
            mesh.vertices = fourCornersArray;
            List<Vector3> list = new List<Vector3>();
            int num = Mathf.Max(8, Mathf.RoundToInt(((float) (this.mySettings.BaseCircleSegments * Mathf.Abs(this.mySettings.Angle))) / 360f));
            for (int i = 0; i < num; i++)
            {
                list.Add(Vector3.Lerp(mesh.vertices[0], mesh.vertices[2], (i * 1f) / ((float) (num - 1))));
            }
            if (this.mySettings.Angle != 0)
            {
                Rect rect = this.myCanvas.GetComponent<RectTransform>().rect;
                float cyllinderRadiusInCanvasSpace = this.mySettings.GetCyllinderRadiusInCanvasSpace();
                for (int k = 0; k < list.Count; k++)
                {
                    Vector3 vector = list[k];
                    if (vertical)
                    {
                        float f = ((list[k].y / rect.size.y) * this.mySettings.Angle) * 0.01745329f;
                        vector.y = Mathf.Sin(f) * cyllinderRadiusInCanvasSpace;
                        Vector3* vectorPtr1 = &vector;
                        vectorPtr1->z += (Mathf.Cos(f) * cyllinderRadiusInCanvasSpace) - cyllinderRadiusInCanvasSpace;
                        list[k] = vector;
                    }
                    else
                    {
                        float f = ((list[k].x / rect.size.x) * this.mySettings.Angle) * 0.01745329f;
                        vector.x = Mathf.Sin(f) * cyllinderRadiusInCanvasSpace;
                        Vector3* vectorPtr2 = &vector;
                        vectorPtr2->z += (Mathf.Cos(f) * cyllinderRadiusInCanvasSpace) - cyllinderRadiusInCanvasSpace;
                        list[k] = vector;
                    }
                }
            }
            for (int j = 0; j < (list.Count - 1); j++)
            {
                GameObject obj3 = new GameObject("Box collider") {
                    layer = base.gameObject.layer
                };
                obj3.transform.SetParent(obj2.transform);
                obj3.transform.ResetTransform();
                obj3.AddComponent<BoxCollider>();
                if (vertical)
                {
                    obj3.transform.localPosition = new Vector3(0f, (list[j + 1].y + list[j].y) * 0.5f, (list[j + 1].z + list[j].z) * 0.5f);
                    obj3.transform.localScale = new Vector3(0.1f, Vector3.Distance(fourCornersArray[0], fourCornersArray[1]), Vector3.Distance(list[j + 1], list[j]));
                    obj3.transform.localRotation = Quaternion.LookRotation(list[j + 1] - list[j], fourCornersArray[0] - fourCornersArray[1]);
                }
                else
                {
                    obj3.transform.localPosition = new Vector3((list[j + 1].x + list[j].x) * 0.5f, 0f, (list[j + 1].z + list[j].z) * 0.5f);
                    obj3.transform.localScale = new Vector3(0.1f, Vector3.Distance(fourCornersArray[0], fourCornersArray[1]), Vector3.Distance(list[j + 1], list[j]));
                    obj3.transform.localRotation = Quaternion.LookRotation(list[j + 1] - list[j], fourCornersArray[0] - fourCornersArray[1]);
                }
            }
            return obj2;
        }

        private unsafe Mesh CreateCyllinderColliderMesh(bool vertical = false)
        {
            Mesh mesh = new Mesh();
            Vector3[] fourCornersArray = new Vector3[4];
            (this.myCanvas.transform as RectTransform).GetWorldCorners(fourCornersArray);
            mesh.vertices = fourCornersArray;
            if (vertical)
            {
                fourCornersArray[0] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[1]);
                fourCornersArray[1] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[2]);
                fourCornersArray[2] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[0]);
                fourCornersArray[3] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[3]);
            }
            else
            {
                fourCornersArray[0] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[1]);
                fourCornersArray[1] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[0]);
                fourCornersArray[2] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[2]);
                fourCornersArray[3] = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(mesh.vertices[3]);
            }
            mesh.vertices = fourCornersArray;
            List<Vector3> list = new List<Vector3>();
            int num = Mathf.Max(8, Mathf.RoundToInt(((float) (this.mySettings.BaseCircleSegments * Mathf.Abs(this.mySettings.Angle))) / 360f));
            for (int i = 0; i < num; i++)
            {
                list.Add(Vector3.Lerp(mesh.vertices[0], mesh.vertices[2], (i * 1f) / ((float) (num - 1))));
                list.Add(Vector3.Lerp(mesh.vertices[1], mesh.vertices[3], (i * 1f) / ((float) (num - 1))));
            }
            if (this.mySettings.Angle != 0)
            {
                Rect rect = this.myCanvas.GetComponent<RectTransform>().rect;
                float cyllinderRadiusInCanvasSpace = base.GetComponent<CurvedUISettings>().GetCyllinderRadiusInCanvasSpace();
                for (int k = 0; k < list.Count; k++)
                {
                    Vector3 vector = list[k];
                    if (vertical)
                    {
                        float f = ((list[k].y / rect.size.y) * this.mySettings.Angle) * 0.01745329f;
                        vector.y = Mathf.Sin(f) * cyllinderRadiusInCanvasSpace;
                        Vector3* vectorPtr1 = &vector;
                        vectorPtr1->z += (Mathf.Cos(f) * cyllinderRadiusInCanvasSpace) - cyllinderRadiusInCanvasSpace;
                        list[k] = vector;
                    }
                    else
                    {
                        float f = ((list[k].x / rect.size.x) * this.mySettings.Angle) * 0.01745329f;
                        vector.x = Mathf.Sin(f) * cyllinderRadiusInCanvasSpace;
                        Vector3* vectorPtr2 = &vector;
                        vectorPtr2->z += (Mathf.Cos(f) * cyllinderRadiusInCanvasSpace) - cyllinderRadiusInCanvasSpace;
                        list[k] = vector;
                    }
                }
            }
            mesh.vertices = list.ToArray();
            List<int> list2 = new List<int>();
            for (int j = 0; j < ((list.Count / 2) - 1); j++)
            {
                if (vertical)
                {
                    list2.Add(j * 2);
                    list2.Add((j * 2) + 1);
                    list2.Add((j * 2) + 2);
                    list2.Add((j * 2) + 1);
                    list2.Add((j * 2) + 3);
                    list2.Add((j * 2) + 2);
                }
                else
                {
                    list2.Add((j * 2) + 2);
                    list2.Add((j * 2) + 1);
                    list2.Add(j * 2);
                    list2.Add((j * 2) + 2);
                    list2.Add((j * 2) + 3);
                    list2.Add((j * 2) + 1);
                }
            }
            mesh.triangles = list2.ToArray();
            return mesh;
        }

        private Mesh CreateSphereColliderMesh()
        {
            Mesh mesh = new Mesh();
            Vector3[] fourCornersArray = new Vector3[4];
            (this.myCanvas.transform as RectTransform).GetWorldCorners(fourCornersArray);
            List<Vector3> verts = new List<Vector3>(fourCornersArray);
            for (int i = 0; i < verts.Count; i++)
            {
                verts[i] = this.mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(verts[i]);
            }
            if (this.mySettings.Angle != 0)
            {
                int count = verts.Count;
                int vertexIndex = 0;
                while (true)
                {
                    if (vertexIndex >= count)
                    {
                        verts.RemoveRange(0, count);
                        float verticalAngle = this.mySettings.VerticalAngle;
                        float angle = this.mySettings.Angle;
                        Vector2 size = (this.myCanvas.transform as RectTransform).rect.size;
                        float cyllinderRadiusInCanvasSpace = this.mySettings.GetCyllinderRadiusInCanvasSpace();
                        if (this.mySettings.PreserveAspect)
                        {
                            verticalAngle = this.mySettings.Angle * (size.y / size.x);
                        }
                        else
                        {
                            cyllinderRadiusInCanvasSpace = size.x / 2f;
                        }
                        for (int k = 0; k < verts.Count; k++)
                        {
                            Vector3 vector2 = verts[k];
                            float f = ((float) (vector2.x / size.x)).Remap(((float) -0.5f), ((float) 0.5f), ((float) (((180f - angle) / 2f) - 90f)), ((float) ((180f - ((180f - angle) / 2f)) - 90f))) * 0.01745329f;
                            Vector3 vector3 = verts[k];
                            float num9 = ((float) (vector3.y / size.y)).Remap(((float) -0.5f), ((float) 0.5f), ((float) ((180f - verticalAngle) / 2f)), ((float) (180f - ((180f - verticalAngle) / 2f)))) * 0.01745329f;
                            verts[k] = new Vector3((Mathf.Sin(num9) * Mathf.Sin(f)) * cyllinderRadiusInCanvasSpace, -cyllinderRadiusInCanvasSpace * Mathf.Cos(num9), ((Mathf.Sin(num9) * Mathf.Cos(f)) * cyllinderRadiusInCanvasSpace) + (!this.mySettings.PreserveAspect ? 0f : -cyllinderRadiusInCanvasSpace));
                        }
                        break;
                    }
                    this.ModifyQuad(verts, vertexIndex, this.mySettings.GetTesslationSize(true));
                    vertexIndex += 4;
                }
            }
            mesh.vertices = verts.ToArray();
            List<int> list2 = new List<int>();
            for (int j = 0; j < verts.Count; j += 4)
            {
                list2.Add(j);
                list2.Add(j + 1);
                list2.Add(j + 2);
                list2.Add(j + 3);
                list2.Add(j);
                list2.Add(j + 2);
            }
            mesh.triangles = list2.ToArray();
            return mesh;
        }

        protected static GameObject FindCommonRoot(GameObject g1, GameObject g2)
        {
            if ((g1 != null) && (g2 != null))
            {
                Transform parent = g1.transform;
                while (parent != null)
                {
                    Transform parent = g2.transform;
                    while (true)
                    {
                        if (parent == null)
                        {
                            parent = parent.parent;
                            break;
                        }
                        if (parent == parent)
                        {
                            return parent.gameObject;
                        }
                        parent = parent.parent;
                    }
                }
            }
            return null;
        }

        private LayerMask GetLayerMaskForMyLayer()
        {
            int num = -1;
            if (this.mySettings.RaycastMyLayerOnly)
            {
                num = 1 << (base.gameObject.layer & 0x1f);
            }
            return num;
        }

        public List<GameObject> GetObjectsHitByRay(Ray ray)
        {
            Vector2 vector;
            List<GameObject> list = new List<GameObject>();
            if (this.GetScreenSpacePointByRay(ray, out vector))
            {
                List<Graphic> list2 = new List<Graphic>();
                IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(this.myCanvas);
                for (int i = 0; i < graphicsForCanvas.Count; i++)
                {
                    Graphic item = graphicsForCanvas[i];
                    if ((item.depth != -1) && (item.raycastTarget && (RectTransformUtility.RectangleContainsScreenPoint(item.rectTransform, vector, this.eventCamera) && item.Raycast(vector, this.eventCamera))))
                    {
                        list2.Add(item);
                    }
                }
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = (g1, g2) => g2.depth.CompareTo(g1.depth);
                }
                list2.Sort(<>f__am$cache1);
                for (int j = 0; j < list2.Count; j++)
                {
                    list.Add(list2[j].gameObject);
                }
                list2.Clear();
            }
            return list;
        }

        public List<GameObject> GetObjectsUnderPointer()
        {
            this.objectsUnderPointer ??= new List<GameObject>();
            return this.objectsUnderPointer;
        }

        public List<GameObject> GetObjectsUnderScreenPos(Vector2 screenPos, Camera eventCamera = null)
        {
            if (eventCamera == null)
            {
                eventCamera = this.myCanvas.worldCamera;
            }
            return this.GetObjectsHitByRay(eventCamera.ScreenPointToRay((Vector3) screenPos));
        }

        private bool GetScreenSpacePointByRay(Ray ray, out Vector2 o_positionOnCanvas)
        {
            switch (this.mySettings.Shape)
            {
                case CurvedUISettings.CurvedUIShape.CYLINDER:
                    return this.RaycastToCyllinderCanvas(ray, out o_positionOnCanvas, false);

                case CurvedUISettings.CurvedUIShape.RING:
                    return this.RaycastToRingCanvas(ray, out o_positionOnCanvas, false);

                case CurvedUISettings.CurvedUIShape.SPHERE:
                    return this.RaycastToSphereCanvas(ray, out o_positionOnCanvas, false);

                case CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL:
                    return this.RaycastToCyllinderVerticalCanvas(ray, out o_positionOnCanvas, false);
            }
            o_positionOnCanvas = Vector2.zero;
            return false;
        }

        protected void HandlePointerExitAndEnter(PointerEventData currentPointerData, GameObject newEnterTarget)
        {
            if ((newEnterTarget == null) || (currentPointerData.pointerEnter == null))
            {
                int num = 0;
                while (true)
                {
                    if (num >= currentPointerData.hovered.Count)
                    {
                        currentPointerData.hovered.Clear();
                        if (newEnterTarget != null)
                        {
                            break;
                        }
                        currentPointerData.pointerEnter = newEnterTarget;
                        return;
                    }
                    ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[num], currentPointerData, ExecuteEvents.pointerExitHandler);
                    num++;
                }
            }
            if ((currentPointerData.pointerEnter != newEnterTarget) || !newEnterTarget)
            {
                GameObject obj2 = FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
                if (currentPointerData.pointerEnter != null)
                {
                    for (Transform transform = currentPointerData.pointerEnter.transform; (transform != null) && ((obj2 == null) || (obj2.transform != transform)); transform = transform.parent)
                    {
                        ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
                        currentPointerData.hovered.Remove(transform.gameObject);
                    }
                }
                currentPointerData.pointerEnter = newEnterTarget;
                if (newEnterTarget != null)
                {
                    for (Transform transform2 = newEnterTarget.transform; (transform2 != null) && (transform2.gameObject != obj2); transform2 = transform2.parent)
                    {
                        ExecuteEvents.Execute<IPointerEnterHandler>(transform2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
                        currentPointerData.hovered.Add(transform2.gameObject);
                    }
                }
            }
        }

        private void ModifyQuad(List<Vector3> verts, int vertexIndex, Vector2 requiredSize)
        {
            List<Vector3> quad = new List<Vector3>();
            for (int i = 0; i < 4; i++)
            {
                quad.Add(verts[vertexIndex + i]);
            }
            Vector3 vector = quad[2] - quad[1];
            Vector3 vector2 = quad[1] - quad[0];
            int num2 = Mathf.CeilToInt(vector.magnitude * (1f / Mathf.Max(1f, requiredSize.x)));
            int num3 = Mathf.CeilToInt(vector2.magnitude * (1f / Mathf.Max(1f, requiredSize.y)));
            float y = 0f;
            int num5 = 0;
            while (num5 < num3)
            {
                float num6 = (num5 + 1f) / ((float) num3);
                float x = 0f;
                int num8 = 0;
                while (true)
                {
                    if (num8 >= num2)
                    {
                        y = num6;
                        num5++;
                        break;
                    }
                    float num9 = (num8 + 1f) / ((float) num2);
                    verts.Add(this.TesselateQuad(quad, x, y));
                    verts.Add(this.TesselateQuad(quad, x, num6));
                    verts.Add(this.TesselateQuad(quad, num9, num6));
                    verts.Add(this.TesselateQuad(quad, num9, y));
                    x = num9;
                    num8++;
                }
            }
        }

        private void ProcessGazeTimedClick()
        {
            if ((this.selectablesUnderGazeLastFrame.Count == 0) || (this.selectablesUnderGazeLastFrame.Count != this.selectablesUnderGaze.Count))
            {
                this.ResetGazeTimedClick();
            }
            else
            {
                for (int i = 0; (i < this.selectablesUnderGazeLastFrame.Count) && (i < this.selectablesUnderGaze.Count); i++)
                {
                    if (this.selectablesUnderGazeLastFrame[i].GetInstanceID() != this.selectablesUnderGaze[i].GetInstanceID())
                    {
                        this.ResetGazeTimedClick();
                        return;
                    }
                }
                if (!this.gazeClickExecuted && (Time.time > ((this.objectsUnderGazeLastChangeTime + CurvedUIInputModule.Instance.GazeClickTimer) + CurvedUIInputModule.Instance.GazeClickTimerDelay)))
                {
                    this.Click();
                    this.gazeClickExecuted = true;
                }
            }
        }

        protected virtual void ProcessMove(PointerEventData pointerEvent)
        {
            GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
            this.HandlePointerExitAndEnter(pointerEvent, gameObject);
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            Ray customControllerRay;
            PointerEventData data;
            if (!this.mySettings.Interactable)
            {
                return;
            }
            if (this.myCanvas.worldCamera == null)
            {
                Debug.LogWarning("CurvedUIRaycaster requires Canvas to have a world camera reference to process events!", this.myCanvas.gameObject);
            }
            Camera worldCamera = this.myCanvas.worldCamera;
            switch (CurvedUIInputModule.ControlMethod)
            {
                case CurvedUIInputModule.CUIControlMethod.MOUSE:
                    customControllerRay = worldCamera.ScreenPointToRay((Vector3) eventData.position);
                    goto TR_0014;

                case CurvedUIInputModule.CUIControlMethod.GAZE:
                    break;

                case CurvedUIInputModule.CUIControlMethod.WORLD_MOUSE:
                    customControllerRay = new Ray(worldCamera.transform.position, this.mySettings.CanvasToCurvedCanvas((Vector3) CurvedUIInputModule.Instance.WorldSpaceMouseInCanvasSpace) - this.myCanvas.worldCamera.transform.position);
                    goto TR_0014;

                case CurvedUIInputModule.CUIControlMethod.CUSTOM_RAY:
                case CurvedUIInputModule.CUIControlMethod.VIVE:
                case CurvedUIInputModule.CUIControlMethod.OCULUS_TOUCH:
                    customControllerRay = CurvedUIInputModule.CustomControllerRay;
                    this.UpdateSelectedObjects(eventData);
                    goto TR_0014;

                case CurvedUIInputModule.CUIControlMethod.GOOGLEVR:
                    Debug.LogError("CURVEDUI: Missing GoogleVR support code. Enable GoogleVR control method on CurvedUISettings component.");
                    break;

                default:
                    customControllerRay = new Ray();
                    goto TR_0014;
            }
            customControllerRay = new Ray(worldCamera.transform.position, worldCamera.transform.forward);
            this.UpdateSelectedObjects(eventData);
        TR_0014:
            data = new PointerEventData(EventSystem.current);
            if (!this.overrideEventData)
            {
                data.pointerEnter = eventData.pointerEnter;
                data.rawPointerPress = eventData.rawPointerPress;
                data.pointerDrag = eventData.pointerDrag;
                data.pointerCurrentRaycast = eventData.pointerCurrentRaycast;
                data.pointerPressRaycast = eventData.pointerPressRaycast;
                data.hovered = new List<GameObject>();
                data.hovered.AddRange(eventData.hovered);
                data.eligibleForClick = eventData.eligibleForClick;
                data.pointerId = eventData.pointerId;
                data.position = eventData.position;
                data.delta = eventData.delta;
                data.pressPosition = eventData.pressPosition;
                data.clickTime = eventData.clickTime;
                data.clickCount = eventData.clickCount;
                data.scrollDelta = eventData.scrollDelta;
                data.useDragThreshold = eventData.useDragThreshold;
                data.dragging = eventData.dragging;
                data.button = eventData.button;
            }
            if ((this.mySettings.Angle != 0) && this.mySettings.enabled)
            {
                Vector2 position = eventData.position;
                switch (this.mySettings.Shape)
                {
                    case CurvedUISettings.CurvedUIShape.CYLINDER:
                        if (this.RaycastToCyllinderCanvas(customControllerRay, out position, false))
                        {
                            break;
                        }
                        return;

                    case CurvedUISettings.CurvedUIShape.RING:
                        if (this.RaycastToRingCanvas(customControllerRay, out position, false))
                        {
                            break;
                        }
                        return;

                    case CurvedUISettings.CurvedUIShape.SPHERE:
                        if (this.RaycastToSphereCanvas(customControllerRay, out position, false))
                        {
                            break;
                        }
                        return;

                    case CurvedUISettings.CurvedUIShape.CYLINDER_VERTICAL:
                        if (this.RaycastToCyllinderVerticalCanvas(customControllerRay, out position, false))
                        {
                            break;
                        }
                        return;

                    default:
                        break;
                }
                this.pointingAtCanvas = true;
                PointerEventData data2 = !this.overrideEventData ? data : eventData;
                if (data2.pressPosition == data2.position)
                {
                    data2.pressPosition = position;
                }
                data2.position = position;
                if (CurvedUIInputModule.ControlMethod == CurvedUIInputModule.CUIControlMethod.VIVE)
                {
                    data2.delta = position - this.lastCanvasPos;
                    this.lastCanvasPos = position;
                }
            }
            this.objectsUnderPointer = eventData.hovered;
            this.Raycast(!this.overrideEventData ? data : eventData, resultAppendList);
        }

        public virtual bool RaycastToCyllinderCanvas(Ray ray3D, out Vector2 o_canvasPos, bool OutputInCanvasSpace = false)
        {
            if (this.showDebug)
            {
                Debug.DrawLine(ray3D.origin, ray3D.GetPoint(1000f), Color.red);
            }
            RaycastHit hitInfo = new RaycastHit();
            if (!Physics.Raycast(ray3D, out hitInfo, float.PositiveInfinity, (int) this.GetLayerMaskForMyLayer()))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            if (this.overrideEventData && ((hitInfo.collider.gameObject != base.gameObject) && ((this.colliderContainer == null) || (hitInfo.collider.transform.parent != this.colliderContainer.transform))))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            Vector3 vector = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            Vector3 normalized = (vector - this.cyllinderMidPoint).normalized;
            Vector2 size = this.myCanvas.GetComponent<RectTransform>().rect.size;
            Vector2 vector5 = new Vector3(0f, 0f, 0f) {
                x = -this.AngleSigned(normalized.ModifyY(0f), (this.mySettings.Angle >= 0) ? Vector3.forward : Vector3.back, Vector3.up).Remap(((float) -this.mySettings.Angle) / 2f, ((float) this.mySettings.Angle) / 2f, -size.x / 2f, size.x / 2f),
                y = vector.y
            };
            o_canvasPos = !OutputInCanvasSpace ? this.myCanvas.worldCamera.WorldToScreenPoint(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3) vector5)) : vector5;
            if (this.showDebug)
            {
                Debug.DrawLine(hitInfo.point, hitInfo.point.ModifyY(hitInfo.point.y + 10f), Color.green);
                Debug.DrawLine(hitInfo.point, this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4(this.cyllinderMidPoint), Color.yellow);
            }
            return true;
        }

        public virtual bool RaycastToCyllinderVerticalCanvas(Ray ray3D, out Vector2 o_canvasPos, bool OutputInCanvasSpace = false)
        {
            if (this.showDebug)
            {
                Debug.DrawLine(ray3D.origin, ray3D.GetPoint(1000f), Color.red);
            }
            RaycastHit hitInfo = new RaycastHit();
            if (!Physics.Raycast(ray3D, out hitInfo, float.PositiveInfinity, (int) this.GetLayerMaskForMyLayer()))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            if (this.overrideEventData && ((hitInfo.collider.gameObject != base.gameObject) && ((this.colliderContainer == null) || (hitInfo.collider.transform.parent != this.colliderContainer.transform))))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            Vector3 vector = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            Vector3 normalized = (vector - this.cyllinderMidPoint).normalized;
            Vector2 size = this.myCanvas.GetComponent<RectTransform>().rect.size;
            Vector2 vector5 = new Vector3(0f, 0f, 0f) {
                y = -this.AngleSigned(normalized.ModifyX(0f), (this.mySettings.Angle >= 0) ? Vector3.forward : Vector3.back, Vector3.left).Remap(((float) -this.mySettings.Angle) / 2f, ((float) this.mySettings.Angle) / 2f, -size.y / 2f, size.y / 2f),
                x = vector.x
            };
            o_canvasPos = !OutputInCanvasSpace ? this.myCanvas.worldCamera.WorldToScreenPoint(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3) vector5)) : vector5;
            if (this.showDebug)
            {
                Debug.DrawLine(hitInfo.point, hitInfo.point.ModifyY(hitInfo.point.y + 10f), Color.green);
                Debug.DrawLine(hitInfo.point, this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4(this.cyllinderMidPoint), Color.yellow);
            }
            return true;
        }

        public virtual bool RaycastToRingCanvas(Ray ray3D, out Vector2 o_canvasPos, bool OutputInCanvasSpace = false)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (!Physics.Raycast(ray3D, out hitInfo, float.PositiveInfinity, (int) this.GetLayerMaskForMyLayer()))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            if (this.overrideEventData && ((hitInfo.collider.gameObject != base.gameObject) && ((this.colliderContainer == null) || (hitInfo.collider.transform.parent != this.colliderContainer.transform))))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            Vector3 trans = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            Vector3 normalized = trans.ModifyZ(0f).normalized;
            Vector2 size = this.myCanvas.GetComponent<RectTransform>().rect.size;
            float num = -this.AngleSigned(normalized.ModifyZ(0f), Vector3.up, Vector3.back);
            Vector2 vector5 = new Vector2(0f, 0f);
            if (this.showDebug)
            {
                Debug.Log("angle: " + num);
            }
            vector5.x = (num >= 0f) ? num.Remap(360f, ((float) (360 - this.mySettings.Angle)), (-size.x / 2f), (size.x / 2f)) : num.Remap(0f, ((float) -this.mySettings.Angle), (-size.x / 2f), (size.x / 2f));
            vector5.y = trans.magnitude.Remap((this.mySettings.RingExternalDiameter * 0.5f) * (1f - this.mySettings.RingFill), this.mySettings.RingExternalDiameter * 0.5f, (-size.y * 0.5f) * (!this.mySettings.RingFlipVertical ? ((float) 1) : ((float) (-1))), (size.y * 0.5f) * (!this.mySettings.RingFlipVertical ? ((float) 1) : ((float) (-1))));
            o_canvasPos = !OutputInCanvasSpace ? this.myCanvas.worldCamera.WorldToScreenPoint(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3) vector5)) : vector5;
            return true;
        }

        public virtual bool RaycastToSphereCanvas(Ray ray3D, out Vector2 o_canvasPos, bool OutputInCanvasSpace = false)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (!Physics.Raycast(ray3D, out hitInfo, float.PositiveInfinity, (int) this.GetLayerMaskForMyLayer()))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            if (this.overrideEventData && ((hitInfo.collider.gameObject != base.gameObject) && ((this.colliderContainer == null) || (hitInfo.collider.transform.parent != this.colliderContainer.transform))))
            {
                o_canvasPos = Vector2.zero;
                return false;
            }
            Vector2 size = this.myCanvas.GetComponent<RectTransform>().rect.size;
            float f = !this.mySettings.PreserveAspect ? (size.x / 2f) : this.mySettings.GetCyllinderRadiusInCanvasSpace();
            Vector3 vector2 = this.myCanvas.transform.worldToLocalMatrix.MultiplyPoint3x4(hitInfo.point);
            Vector3 v = new Vector3(0f, 0f, !this.mySettings.PreserveAspect ? 0f : -f);
            Vector3 normalized = (vector2 - v).normalized;
            Vector3 n = Vector3.Cross(normalized, normalized.ModifyY(0f)).normalized * ((normalized.y >= 0f) ? ((float) (-1)) : ((float) 1));
            float num2 = -this.AngleSigned(normalized.ModifyY(0f), (this.mySettings.Angle <= 0) ? Vector3.back : Vector3.forward, (this.mySettings.Angle <= 0) ? Vector3.down : Vector3.up);
            float num3 = -this.AngleSigned(normalized, normalized.ModifyY(0f), n);
            float num4 = Mathf.Abs(this.mySettings.Angle) * 0.5f;
            float num5 = Mathf.Abs(!this.mySettings.PreserveAspect ? (this.mySettings.VerticalAngle * 0.5f) : ((num4 * size.y) / size.x));
            Vector2 vector8 = new Vector2(num2.Remap(-num4, num4, -size.x * 0.5f, size.x * 0.5f), num3.Remap(-num5, num5, -size.y * 0.5f, size.y * 0.5f));
            if (this.showDebug)
            {
                object[] objArray1 = new object[] { "h: ", num2, " / v: ", num3, " poc: ", vector8 };
                Debug.Log(string.Concat(objArray1));
                Debug.DrawRay(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4(v), this.myCanvas.transform.localToWorldMatrix.MultiplyVector(normalized) * Mathf.Abs(f), Color.red);
                Debug.DrawRay(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4(v), this.myCanvas.transform.localToWorldMatrix.MultiplyVector(n) * 300f, Color.magenta);
            }
            o_canvasPos = !OutputInCanvasSpace ? this.myCanvas.worldCamera.WorldToScreenPoint(this.myCanvas.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3) vector8)) : vector8;
            return true;
        }

        public void RebuildCollider()
        {
            this.cyllinderMidPoint = new Vector3(0f, 0f, -this.mySettings.GetCyllinderRadiusInCanvasSpace());
            this.CreateCollider();
        }

        private void ResetGazeTimedClick()
        {
            this.objectsUnderGazeLastChangeTime = Time.time;
            this.gazeClickExecuted = false;
        }

        private void SetupMeshColliderUsingMesh(Mesh meshie)
        {
            MeshCollider collider = base.gameObject.AddComponent<MeshCollider>();
            this.AddComponentIfMissing<MeshFilter>().mesh = meshie;
            collider.sharedMesh = meshie;
        }

        private bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold) => 
            useDragThreshold ? ((pressPos - currentPos).sqrMagnitude >= (threshold * threshold)) : true;

        protected override void Start()
        {
            this.CreateCollider();
        }

        private Vector3 TesselateQuad(List<Vector3> quad, float x, float y)
        {
            Vector3 zero = Vector3.zero;
            List<float> list = new List<float> {
                (1f - x) * (1f - y),
                (1f - x) * y,
                x * y,
                x * (1f - y)
            };
            for (int i = 0; i < 4; i++)
            {
                zero += quad[i] * list[i];
            }
            return zero;
        }

        protected virtual void Update()
        {
            if (this.pointingAtCanvas && ((CurvedUIInputModule.ControlMethod == CurvedUIInputModule.CUIControlMethod.GAZE) && CurvedUIInputModule.Instance.GazeUseTimedClick))
            {
                this.ProcessGazeTimedClick();
                this.selectablesUnderGazeLastFrame.Clear();
                this.selectablesUnderGazeLastFrame.AddRange(this.selectablesUnderGaze);
                this.selectablesUnderGaze.Clear();
                this.selectablesUnderGaze.AddRange(this.objectsUnderPointer);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = obj => obj.GetComponent<Selectable>() == null;
                }
                this.selectablesUnderGaze.RemoveAll(<>f__am$cache0);
                if (CurvedUIInputModule.Instance.GazeTimedClickProgressImage != null)
                {
                    if (CurvedUIInputModule.Instance.GazeTimedClickProgressImage.type != Image.Type.Filled)
                    {
                        CurvedUIInputModule.Instance.GazeTimedClickProgressImage.type = Image.Type.Filled;
                    }
                    CurvedUIInputModule.Instance.GazeTimedClickProgressImage.fillAmount = (Time.time - this.objectsUnderGazeLastChangeTime).RemapAndClamp(CurvedUIInputModule.Instance.GazeClickTimerDelay, CurvedUIInputModule.Instance.GazeClickTimer + CurvedUIInputModule.Instance.GazeClickTimerDelay, 0f, 1f);
                }
            }
            this.pointingAtCanvas = false;
        }

        protected void UpdateSelectedObjects(PointerEventData eventData)
        {
            bool flag = false;
            foreach (GameObject obj2 in eventData.hovered)
            {
                if (obj2 == eventData.selectedObject)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                eventData.selectedObject = null;
            }
            foreach (GameObject obj3 in eventData.hovered)
            {
                if (obj3 != null)
                {
                    Graphic component = obj3.GetComponent<Graphic>();
                    if ((obj3.GetComponent<Selectable>() != null) && ((component != null) && ((component.depth != -1) && component.raycastTarget)))
                    {
                        if (eventData.selectedObject != obj3)
                        {
                            eventData.selectedObject = obj3;
                        }
                        break;
                    }
                }
            }
            if ((this.mySettings.ControlMethod == CurvedUIInputModule.CUIControlMethod.GAZE) && (eventData.IsPointerMoving() && ((eventData.pointerDrag != null) && (!eventData.dragging && this.ShouldStartDrag(eventData.pressPosition, eventData.position, (float) EventSystem.current.pixelDragThreshold, eventData.useDragThreshold)))))
            {
                ExecuteEvents.Execute<IBeginDragHandler>(eventData.pointerDrag, eventData, ExecuteEvents.beginDragHandler);
                eventData.dragging = true;
            }
        }
    }
}

