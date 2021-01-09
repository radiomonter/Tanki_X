namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    internal class TrackMarksSystem : ECSSystem
    {
        private const float MAX_DIRECTION_CHANGE_COS = 0.75f;
        private const float MAX_NORMAL_CHANGE_COS = 0.95f;
        private const string LEFT_PREFIX = "left_";
        private const string RIGHT_PREFIX = "right_";
        private const float SIDE_RAYCAST_SHIFT = 0.1f;
        private const float MAIN_RAYCAST_SHIFT = 0.2f;
        private const float MAIN_RAYCAST_MULTIPLIER = 2f;
        private const float CHECK_EXTRA_CONTACTS_MULTIPILER = 1.5f;
        private const int TRACK_COUNT = 2;
        private const int LEFT_TRACK = 0;
        private const int RIGHT_TRACK = 1;
        private const int IMPORTANT_WHEELS_COUNT = 3;
        private const float MIN_LAST_TRACK_UPDATE = 0.1f;
        private const float MARK_ROTATION_THRESHOLD = 0.95f;
        private const float MARK_ROTATION_MAX = 0.8f;
        private const float MAX_DRIFT_FACTOR = 2f;
        private static readonly int TRACK_LAYER_MASK = LayerMasks.VISUAL_STATIC;

        private void AddSectorToRender(ref TrackRenderData data, Vector3 startPosition, Vector3 startForward, Vector3 endPosition, Vector3 endForward, Vector3 normal, float width, float textureWidth, float rotationCos, bool contiguous)
        {
            data.lastSectorIndex++;
            data.lastSectorIndex = data.lastSectorIndex % data.maxSectorCountPerTrack;
            data.sectors[data.lastSectorIndex] = new TrackSector { 
                startPosition = startPosition,
                startForward = startForward,
                endPosition = endPosition,
                endForward = endForward,
                normal = normal,
                width = width,
                rotationCos = rotationCos,
                textureWidth = textureWidth,
                contiguous = contiguous
            };
            data.sectorCount++;
            if (data.sectorCount > data.maxSectorCountPerTrack)
            {
                data.sectorCount = data.maxSectorCountPerTrack;
            }
            data.currentPart++;
            data.currentPart = data.currentPart % data.parts;
        }

        private static void AllocateBuilder(TrackMarksBuilderComponent builder, TrackMarksComponent component, TrackMarksRenderComponent renderer)
        {
            builder.leftWheels = new Transform[3];
            builder.rightWheels = new Transform[3];
            builder.prevLeftWheelsData = new WheelData[3];
            builder.prevRightWheelsData = new WheelData[3];
            builder.tempLeftWheelsData = new WheelData[3];
            builder.tempRightWheelsData = new WheelData[3];
            builder.currentLeftWheelsData = new WheelData[3];
            builder.currentRightWheelsData = new WheelData[3];
            builder.positions = new Vector3[2];
            builder.nextPositions = new Vector3[2];
            builder.normals = new Vector3[2];
            builder.nextNormals = new Vector3[2];
            builder.directions = new Vector3[2];
            builder.contiguous = new bool[2];
            builder.prevHits = new bool[2];
            builder.remaingDistance = new float[2];
            builder.resetWheels = new bool[2];
            builder.side = new float[] { -1f, 1f };
            for (int i = 0; i < 2; i++)
            {
                builder.contiguous[i] = false;
                builder.prevHits[i] = false;
            }
            builder.moveStep = component.markWidth / ((float) component.parts);
        }

        private bool CheckDirectionChange(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i, ref RaycastHit hitData)
        {
            if (Mathf.Abs(Vector3.Dot(this.GetVelocity(builder).normalized, builder.directions[i])) >= 0.75f)
            {
                return true;
            }
            this.ResetTrack(builder, trackMarks, i, ref hitData);
            return false;
        }

        private bool CheckEnoughMove(TrackMarksBuilderComponent builder, int i, ref RaycastHit hit) => 
            (builder.nextPositions[i] - builder.positions[i]).magnitude >= builder.moveStep;

        private bool CheckExtraContacts(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] wheels, float hitDistance)
        {
            float maxDistance = hitDistance + 0.2f;
            return (Physics.Raycast(wheels[0].position, -trackMarks.transform.up, maxDistance, TRACK_LAYER_MASK) || !Physics.Raycast(wheels[wheels.Length - 1].position, -trackMarks.transform.up, maxDistance, TRACK_LAYER_MASK));
        }

        private bool CheckNormalChange(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i, ref RaycastHit hitData)
        {
            if (Vector3.Dot(builder.normals[i], hitData.normal) >= 0.95f)
            {
                return true;
            }
            this.ResetTrack(builder, trackMarks, i, ref hitData);
            return false;
        }

        private void CheckResetWheels(TrackMarksBuilderComponent builder, int i, Transform[] wheels, WheelData[] result)
        {
            if (builder.resetWheels[i])
            {
                this.CopyWheelsPositionFromTransforms(wheels, result);
                builder.resetWheels[i] = false;
            }
        }

        private void ComputeSectorVertices(TrackSector sector, int count, int indexByOrder, int indexInBufer, Vector3 prev0, Vector3 prev1, out float startTextureWidth, out float endTextureWidth, out Vector3 v0, out Vector3 v1, out Vector3 v2, out Vector3 v3)
        {
            this.ComputeVerticesPosition(ref sector.endPosition, ref sector.endForward, ref sector.normal, sector.width, out v0, out v1);
            Vector3 vector3 = Vector3.Cross(sector.startForward, sector.normal).normalized * (sector.width / 2f);
            startTextureWidth = endTextureWidth = sector.textureWidth;
            if (!sector.contiguous || (indexByOrder >= (count - 1)))
            {
                this.ComputeVerticesPosition(ref sector.startPosition, ref sector.startForward, ref sector.normal, sector.width, out v2, out v3);
            }
            else
            {
                v2 = prev0;
                v3 = prev1;
            }
        }

        private Vector3 ComputeTrackDiagonal(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] data, ref Vector3 left) => 
            (data[data.Length - 1].position + ((data[data.Length - 1].right * trackMarks.markWidth) / 2f)) - (data[0].position - ((data[0].right * trackMarks.markWidth) / 2f));

        private void ComputeVerticesPosition(ref Vector3 position, ref Vector3 forward, ref Vector3 normal, float width, out Vector3 v0, out Vector3 v1)
        {
            Vector3 vector3 = Vector3.Cross(forward, normal).normalized * (width / 2f);
            v0 = position + vector3;
            v1 = position - vector3;
        }

        private void CopySectorToMesh(TrackRenderData data, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal, int indexByOrder, Vector3[] positions, Vector2[] uvs, Vector3[] normals, Color[] colors, int[] triangles, int firstVertex, int firstIndex, float halfAlpha, float startTextureWidth, float endTextureWidth, float rotationCos)
        {
            positions[firstVertex] = v0;
            positions[firstVertex + 1] = v1;
            positions[firstVertex + 2] = v2;
            positions[firstVertex + 3] = v3;
            float y = (indexByOrder - (data.currentPart % data.parts)) * data.texturePart;
            float num2 = y - data.texturePart;
            float s = Mathf.Sin(Mathf.Acos(rotationCos));
            Vector2 vector = new Vector2(0f, num2);
            Vector2 vector2 = new Vector2(startTextureWidth, num2);
            Vector2 vector3 = new Vector2(0f, y);
            Vector2 vector4 = new Vector2(endTextureWidth, y);
            this.RotateTextureCoords(ref vector, ref vector2, ref vector3, ref vector4, out uvs[firstVertex], out uvs[firstVertex + 1], out uvs[firstVertex + 2], out uvs[firstVertex + 3], rotationCos, s);
            normals[firstVertex] = normal;
            normals[firstVertex + 1] = normal;
            normals[firstVertex + 2] = normal;
            normals[firstVertex + 3] = normal;
            float baseAlpha = data.baseAlpha;
            float num6 = data.baseAlpha;
            if (indexByOrder > data.firstSectorToHide)
            {
                int num7 = data.maxSectorCountPerTrack - data.firstSectorToHide;
                float num9 = 1f - (((float) (indexByOrder - data.firstSectorToHide)) / ((float) num7));
                baseAlpha *= num9 + halfAlpha;
                num6 *= num9 - halfAlpha;
            }
            Color color = Color.white * baseAlpha;
            Color color2 = Color.white * num6;
            colors[firstVertex] = color;
            colors[firstVertex + 1] = color;
            colors[firstVertex + 2] = color2;
            colors[firstVertex + 3] = color2;
            triangles[firstIndex] = firstVertex;
            triangles[firstIndex + 1] = firstVertex + 1;
            triangles[firstIndex + 2] = firstVertex + 2;
            triangles[firstIndex + 3] = firstVertex + 3;
            triangles[firstIndex + 4] = firstVertex + 2;
            triangles[firstIndex + 5] = firstVertex + 1;
        }

        private void CopyToMesh(TrackRenderData data, Vector3[] positions, Vector2[] uvs, Vector3[] normals, Color[] colors, int[] triangles, int firstVertex, int firstIndex, out int nextIndex, out int nextVertex)
        {
            int index = data.lastSectorIndex - data.sectorCount;
            if (index < 0)
            {
                index += data.maxSectorCountPerTrack;
            }
            float halfAlpha = 0.5f / ((float) (data.maxSectorCountPerTrack - data.firstSectorToHide));
            Vector3 vector = new Vector3();
            Vector3 vector2 = new Vector3();
            for (int i = data.sectorCount; i > 0; i--)
            {
                Vector3 vector3;
                Vector3 vector4;
                Vector3 vector5;
                Vector3 vector6;
                float num4;
                float num5;
                TrackSector sector = data.sectors[index];
                this.ComputeSectorVertices(sector, data.sectorCount, i, index, vector, vector2, out num4, out num5, out vector3, out vector4, out vector5, out vector6);
                this.CopySectorToMesh(data, vector3, vector4, vector5, vector6, sector.normal, i, positions, uvs, normals, colors, triangles, firstVertex, firstIndex, halfAlpha, num4, num5, sector.rotationCos);
                firstVertex += 4;
                firstIndex += 6;
                index = (index + 1) % data.maxSectorCountPerTrack;
                vector = vector3;
                vector2 = vector4;
            }
            nextIndex = firstIndex;
            nextVertex = firstVertex;
        }

        private void CopyWheelDataFromTransforms(Transform src, ref WheelData dst)
        {
            dst.position = src.position;
            dst.right = src.right;
        }

        private void CopyWheelsPositionFromTransforms(Transform[] wheels, WheelData[] data)
        {
            this.CopyWheelDataFromTransforms(wheels[0], ref data[0]);
            this.CopyWheelDataFromTransforms(wheels[wheels.Length / 2], ref data[1]);
            this.CopyWheelDataFromTransforms(wheels[wheels.Length - 1], ref data[2]);
        }

        private List<Transform> FindAllNodes(Transform root, string pattern, List<Transform> list = null)
        {
            list ??= new List<Transform>();
            for (int i = 0; i < root.childCount; i++)
            {
                Transform child = root.GetChild(i);
                if (child.name.Contains(pattern))
                {
                    list.Add(child);
                }
                list = this.FindAllNodes(child, pattern, list);
            }
            return list;
        }

        private void FindWheels(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis, TrackMarksComponent trackMarks)
        {
            List<Transform> list = this.FindAllNodes(trackMarks.transform, chassis.movingWheelName, null);
            List<Transform> list2 = new List<Transform>();
            List<Transform> list3 = new List<Transform>();
            for (int i = 0; i < list.Count; i++)
            {
                Transform item = list[i];
                if (item.name.Contains("left_"))
                {
                    list2.Add(item);
                }
                if (item.name.Contains("right_"))
                {
                    list3.Add(item);
                }
            }
            ZSorter comparer = new ZSorter();
            list2.Sort(comparer);
            list3.Sort(comparer);
            builder.leftWheels = list2.ToArray();
            builder.rightWheels = list3.ToArray();
        }

        private Vector3 GetVelocity(TrackMarksBuilderComponent builder) => 
            builder.rigidbody.velocity;

        [OnEventFire]
        public void InitBuilder(InitTrackBuilderEvent e, TrackMarksInitNode node, SingleNode<TrackMarksRenderComponent> renderer)
        {
            TrackMarksComponent trackMarks = node.trackMarks;
            TrackMarksBuilderComponent builder = new TrackMarksBuilderComponent();
            AllocateBuilder(builder, trackMarks, renderer.component);
            this.FindWheels(builder, node.chassisAnimation, trackMarks);
            node.Entity.AddComponent(builder);
        }

        private void InitTrackMarksRender(TrackMarksRenderComponent render, TrackMarksComponent trackMarks)
        {
            int num = (2 * trackMarks.maxSectorsPerTrack) * 4;
            int num2 = (2 * trackMarks.maxSectorsPerTrack) * 6;
            render.meshColors = new Color[num];
            render.meshNormals = new Vector3[num];
            render.meshPositions = new Vector3[num];
            render.meshUVs = new Vector2[num];
            render.meshTris = new int[num2];
            render.trackRenderDatas = new TrackRenderData[2];
            for (int i = 0; i < 2; i++)
            {
                render.trackRenderDatas[i] = new TrackRenderData(trackMarks.maxSectorsPerTrack, trackMarks.hideSectorsFrom, trackMarks.baseAlpha, trackMarks.parts);
            }
        }

        [OnEventFire]
        public void InitTrackMarksSystem(NodeAddedEvent evt, TrackMarksInitNode node)
        {
            base.NewEvent<InitTrackRendererEvent>().Attach(node).ScheduleDelayed(0.3f);
        }

        [OnEventFire]
        public void InitTracks(InitTrackRendererEvent e, TrackMarksInitNode node)
        {
            Transform parent = node.assembledTank.AssemblyRoot.transform;
            TrackMarksComponent trackMarks = node.trackMarks;
            TrackMarksRenderComponent render = new TrackMarksRenderComponent {
                mesh = new Mesh()
            };
            render.mesh.MarkDynamic();
            GameObject obj2 = new GameObject("Track");
            obj2.transform.SetParent(parent);
            obj2.AddComponent<MeshFilter>().mesh = render.mesh;
            trackMarks.material = new Material(trackMarks.material);
            MeshRenderer renderer = obj2.AddComponent<MeshRenderer>();
            renderer.material = trackMarks.material;
            foreach (ParticleSystem system in trackMarks.particleSystems)
            {
                ParticleSystem.ShapeModule shape = system.shape;
                shape.enabled = true;
                shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                shape.meshRenderer = renderer;
            }
            this.InitTrackMarksRender(render, trackMarks);
            node.Entity.AddComponent(render);
            base.NewEvent<InitTrackBuilderEvent>().Attach(node).ScheduleDelayed(0.3f);
        }

        private void InterpolateWheelsPosition(float delta, WheelData[] prev, WheelData[] current, WheelData[] result)
        {
            for (int i = 0; i < prev.Length; i++)
            {
                result[i].position = Vector3.Lerp(prev[i].position, current[i].position, delta);
                result[i].right = Vector3.Lerp(prev[i].right, current[i].right, delta);
            }
        }

        private bool IsHit(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, WheelData[] wheels, float hitDistance, out RaycastHit hit)
        {
            WheelData data = wheels[wheels.Length / 2];
            bool flag = Physics.Raycast(data.position, -trackMarks.transform.up, out hit, hitDistance * 2f, TRACK_LAYER_MASK);
            if (flag)
            {
                RaycastHit hit2;
                if ((hit.distance >= (hitDistance * 1.5f)) && !this.CheckExtraContacts(builder, trackMarks, wheels, hitDistance))
                {
                    return false;
                }
                float maxDistance = hit.distance + 0.1f;
                flag &= Physics.Raycast(data.position + (data.right * trackMarks.markTestShift), -trackMarks.transform.up, out hit2, maxDistance, TRACK_LAYER_MASK);
                if (flag)
                {
                    RaycastHit hit3;
                    flag &= Physics.Raycast(data.position - (data.right * trackMarks.markTestShift), -trackMarks.transform.up, out hit3, maxDistance, TRACK_LAYER_MASK);
                    hit.normal = (hit3.normal + hit3.normal) / 2f;
                    hit.point = (hit3.point + hit2.point) / 2f;
                }
            }
            return flag;
        }

        private void MakeTracks(TrackMarksRenderComponent trackMarksRender, int track, Vector3 startPosition, Vector3 startForward, Vector3 endPosition, Vector3 endForward, Vector3 normal, float width, float textureWidth, float rotationCos, bool contiguous)
        {
            this.AddSectorToRender(ref trackMarksRender.trackRenderDatas[track], startPosition, startForward, endPosition, endForward, normal, width, textureWidth, rotationCos, contiguous);
            trackMarksRender.dirty = true;
        }

        private bool NeedUpdateMarks(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks) => 
            (builder.CachedCamera.transform.position - trackMarks.transform.position).magnitude <= trackMarks.maxDistance;

        [OnEventFire]
        public void OnUpdate(UpdateEvent evt, TrackMarkUpdateNode node, [JoinAll] CameraNode cameraNode)
        {
            if (cameraNode.camera.Enabled)
            {
                int num = 6 - GraphicsSettings.INSTANCE.CurrentQualityLevel;
                if (num > 0)
                {
                    long num2;
                    node.trackMarks.tick = num2 = node.trackMarks.tick + 1L;
                    if ((num2 % ((long) num)) != 0L)
                    {
                        return;
                    }
                }
                node.trackMarks.tick = 0L;
                TrackMarksRenderComponent trackMarksRender = node.trackMarksRender;
                TrackMarksBuilderComponent trackMarksBuilder = node.trackMarksBuilder;
                ChassisAnimationComponent chassisAnimation = node.chassisAnimation;
                TrackMarksComponent trackMarks = node.trackMarks;
                trackMarksBuilder.rigidbody = node.rigidbody.Rigidbody;
                if (this.NeedUpdateMarks(trackMarksBuilder, trackMarks))
                {
                    this.UpdateSingleTrack(trackMarksBuilder, chassisAnimation, trackMarksRender, trackMarks, 0, trackMarksBuilder.leftWheels, trackMarksBuilder.prevLeftWheelsData, trackMarksBuilder.currentLeftWheelsData, trackMarksBuilder.tempLeftWheelsData);
                    this.UpdateSingleTrack(trackMarksBuilder, chassisAnimation, trackMarksRender, trackMarks, 1, trackMarksBuilder.rightWheels, trackMarksBuilder.prevRightWheelsData, trackMarksBuilder.currentRightWheelsData, trackMarksBuilder.tempRightWheelsData);
                }
                if (trackMarksRender.dirty)
                {
                    this.UpdateMesh(trackMarksRender);
                    trackMarksRender.dirty = false;
                }
            }
        }

        private void ResetBuilder(TrackMarksBuilderComponent builder)
        {
            for (int i = 0; i < 2; i++)
            {
                builder.prevHits[i] = false;
                builder.contiguous[i] = false;
                builder.resetWheels[i] = true;
            }
        }

        private void ResetTrack(TrackMarksBuilderComponent builder, TrackMarksComponent trackMarks, int i, ref RaycastHit hitData)
        {
            builder.directions[i] = trackMarks.transform.forward;
            builder.nextPositions[i] = builder.positions[i] = hitData.point;
            builder.nextNormals[i] = builder.normals[i] = hitData.normal;
            builder.contiguous[i] = false;
            builder.remaingDistance[i] = 0f;
        }

        [OnEventFire]
        public void ResetTrackMarks(NodeAddedEvent e, SemiActiveTankTrackMarkNode trackNode)
        {
            this.ResetBuilder(trackNode.trackMarksBuilder);
            trackNode.trackMarksRender.Clear();
            trackNode.trackMarks.material.SetAlpha(1f);
        }

        private Vector2 RotateCoord(ref Vector2 point, ref Vector2 center, float c, float s) => 
            new Vector2 { 
                x = (((point.x - center.x) * c) - ((point.y - center.y) * s)) + center.x,
                y = (((point.x - center.x) * s) + ((point.y - center.y) * c)) + center.y
            };

        private void RotateTextureCoords(ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3, ref Vector2 uv4, out Vector2 r1, out Vector2 r2, out Vector2 r3, out Vector2 r4, float c, float s)
        {
            Vector2 center = (((uv1 + uv2) + uv3) + uv3) / 4f;
            r1 = this.RotateCoord(ref uv1, ref center, c, s);
            r2 = this.RotateCoord(ref uv2, ref center, c, s);
            r3 = this.RotateCoord(ref uv3, ref center, c, s);
            r4 = this.RotateCoord(ref uv4, ref center, c, s);
        }

        [OnEventFire]
        public void StartFadeMaterialAlpha(NodeAddedEvent e, FadeTrackMarkNode trackNode)
        {
            trackNode.trackMarks.baseAlpha *= trackNode.trackMarks.material.color.a;
        }

        [OnEventFire]
        public void UpdateFadeMaterialAlpha(UpdateEvent e, FadeTrackMarkNode trackNode)
        {
            trackNode.trackMarks.material.SetAlpha(trackNode.transparencyTransition.CurrentAlpha * trackNode.trackMarks.baseAlpha);
        }

        private void UpdateMesh(TrackMarksRenderComponent data)
        {
            int firstVertex = 0;
            int firstIndex = 0;
            for (int i = 0; i < 2; i++)
            {
                this.CopyToMesh(data.trackRenderDatas[i], data.meshPositions, data.meshUVs, data.meshNormals, data.meshColors, data.meshTris, firstVertex, firstIndex, out firstIndex, out firstVertex);
            }
            data.mesh.vertices = data.meshPositions;
            data.mesh.uv = data.meshUVs;
            data.mesh.normals = data.meshNormals;
            data.mesh.triangles = data.meshTris;
            data.mesh.colors = data.meshColors;
            data.mesh.RecalculateBounds();
        }

        private void UpdateSingleTrack(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis, TrackMarksRenderComponent render, TrackMarksComponent trackMarks, int track, Transform[] wheels, WheelData[] prevWheelsData, WheelData[] currentWheelsData, WheelData[] tempWheelsData)
        {
            this.CheckResetWheels(builder, track, wheels, prevWheelsData);
            this.CopyWheelsPositionFromTransforms(wheels, currentWheelsData);
            this.UpdateWheels(builder, chassis, render, trackMarks, track, currentWheelsData, prevWheelsData, tempWheelsData);
            Array.Copy(currentWheelsData, prevWheelsData, currentWheelsData.Length);
        }

        private void UpdateTrack(TrackMarksBuilderComponent builder, TrackMarksRenderComponent render, TrackMarksComponent trackMarks, int track, bool hit, ref RaycastHit hitData, float width, float shiftToBack)
        {
            if (!hit)
            {
                this.ResetTrack(builder, trackMarks, track, ref hitData);
            }
            else if (!builder.prevHits[track])
            {
                this.ResetTrack(builder, trackMarks, track, ref hitData);
            }
            else
            {
                Vector3 normalized = this.GetVelocity(builder).normalized;
                builder.nextPositions[track] = (hitData.point - (((trackMarks.transform.forward * shiftToBack) * trackMarks.markWidth) / 2f)) + ((normalized * shiftToBack) * trackMarks.markWidth);
                builder.nextNormals[track] = hitData.normal;
                if (this.CheckEnoughMove(builder, track, ref hitData) && (this.CheckDirectionChange(builder, trackMarks, track, ref hitData) && this.CheckNormalChange(builder, trackMarks, track, ref hitData)))
                {
                    float rotationCos = Vector3.Dot(trackMarks.transform.forward, normalized);
                    if (rotationCos > 0.95f)
                    {
                        rotationCos = 1f;
                    }
                    else if (rotationCos < 0.8f)
                    {
                        rotationCos = 0f;
                    }
                    this.MakeTracks(render, track, builder.positions[track], builder.directions[track], builder.nextPositions[track], normalized, hitData.normal, width, width / trackMarks.markWidth, rotationCos, builder.contiguous[track]);
                    builder.directions[track] = normalized;
                    builder.positions[track] = builder.nextPositions[track];
                    builder.normals[track] = builder.nextNormals[track];
                    builder.contiguous[track] = true;
                }
            }
            builder.prevHits[track] = hit;
        }

        private unsafe void UpdateWheels(TrackMarksBuilderComponent builder, ChassisAnimationComponent chassis, TrackMarksRenderComponent render, TrackMarksComponent trackMarks, int track, WheelData[] currentWheelsPositions, WheelData[] prevWheelsPosition, WheelData[] temp)
        {
            float highDistance = chassis.highDistance;
            WheelData data = prevWheelsPosition[prevWheelsPosition.Length / 2];
            WheelData data2 = currentWheelsPositions[currentWheelsPositions.Length / 2];
            float num2 = (data.position - data2.position).magnitude + builder.remaingDistance[track];
            int num3 = (int) (num2 / builder.moveStep);
            Vector3 lhs = Vector3.Cross(trackMarks.transform.up, this.GetVelocity(builder) * Time.smoothDeltaTime).normalized * builder.side[track];
            float b = Mathf.Max(Mathf.Abs(Vector3.Dot(lhs, this.ComputeTrackDiagonal(builder, trackMarks, currentWheelsPositions, ref lhs))) / 2f, trackMarks.markWidth);
            float a = Mathf.Max(Mathf.Abs(Vector3.Dot(lhs, this.ComputeTrackDiagonal(builder, trackMarks, prevWheelsPosition, ref lhs))) / 2f, trackMarks.markWidth);
            bool flag = false;
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < num3; i++)
            {
                float t = ((float) i) / ((float) num3);
                float width = Mathf.Lerp(a, b, t);
                float num9 = Mathf.Min((float) (width / trackMarks.markWidth), (float) 2f);
                this.InterpolateWheelsPosition(t, prevWheelsPosition, currentWheelsPositions, temp);
                flag = this.IsHit(builder, trackMarks, temp, highDistance, out hit);
                RaycastHit* hitPtr1 = &hit;
                hitPtr1.point -= (builder.side[track] * trackMarks.transform.right) * trackMarks.shiftToCenter;
                this.UpdateTrack(builder, render, trackMarks, track, flag, ref hit, width, num9 - 1f);
                num2 -= builder.moveStep;
            }
            builder.remaingDistance[track] = num2;
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;
        }

        public class FadeTrackMarkNode : Node
        {
            public TrackMarksComponent trackMarks;
            public TransparencyTransitionComponent transparencyTransition;
            public TankDeadStateComponent tankDeadState;
        }

        public class InitTrackBuilderEvent : Event
        {
        }

        public class InitTrackRendererEvent : Event
        {
        }

        public class SemiActiveTankTrackMarkNode : Node
        {
            public TrackMarksComponent trackMarks;
            public TankSemiActiveStateComponent tankSemiActiveState;
            public TrackMarksRenderComponent trackMarksRender;
            public TrackMarksBuilderComponent trackMarksBuilder;
        }

        public class TrackMarksInitNode : Node
        {
            public TrackMarksComponent trackMarks;
            public ChassisAnimationComponent chassisAnimation;
            public AssembledTankComponent assembledTank;
        }

        public class TrackMarkUpdateNode : Node
        {
            public RigidbodyComponent rigidbody;
            public TrackMarksComponent trackMarks;
            public TrackMarksRenderComponent trackMarksRender;
            public TrackMarksBuilderComponent trackMarksBuilder;
            public ChassisAnimationComponent chassisAnimation;
        }

        private sealed class ZSorter : IComparer<Transform>
        {
            public int Compare(Transform lhs, Transform rhs) => 
                (int) Mathf.Sign(rhs.position.z - lhs.position.z);
        }
    }
}

