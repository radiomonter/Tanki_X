namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class MyShadowSystem : MonoBehaviour
    {
        public List<CharacterShadowComponent> characters;
        public Camera camera;
        private CharacterShadowCommonSettingsComponent shadowCommonSettings;
        private readonly List<CharacterShadowComponent> visibleCharacters = new List<CharacterShadowComponent>();

        private static Rect CalculateLocationInAtlas(int countX, int countY, int xIndex, int yIndex)
        {
            float x = 1f / ((float) countX);
            float y = 1f / ((float) countY);
            return new Rect(new Rect(new Vector2(((2 * xIndex) * x) * 0.5f, ((2 * yIndex) * y) * 0.5f), new Vector2(x, y)));
        }

        private Bounds CalculateProjectionBoundInLightSpace(CharacterShadowCommonSettingsComponent settings, Bounds boundsInLightSpace, float offset)
        {
            float num3 = Mathf.Max(boundsInLightSpace.size.x, boundsInLightSpace.size.y) / ((float) ((((settings.textureSize - settings.blurSize) - settings.blurSize) - 1) - 1));
            float num4 = (2 * (1 + settings.blurSize)) * num3;
            float x = boundsInLightSpace.size.x + num4;
            float y = boundsInLightSpace.size.y + num4;
            if (x > y)
            {
                y += ((Mathf.Ceil((y - 0.01f) / (num3 + num3)) * (num3 + num3)) - y) * 0.5f;
            }
            else
            {
                x += ((Mathf.Ceil((x - 0.01f) / (num3 + num3)) * (num3 + num3)) - x) * 0.5f;
            }
            Bounds bounds = new Bounds {
                size = new Vector3(x, y, boundsInLightSpace.size.z - offset)
            };
            bounds.center = new Vector3(boundsInLightSpace.center.x, boundsInLightSpace.center.y, boundsInLightSpace.max.z - bounds.extents.z);
            return bounds;
        }

        private void CalculateProjectionData(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> characters, Camera camera)
        {
            Quaternion rotation = settings.virtualLight.rotation;
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 matrixx4 = Matrix4x4.TRS(Vector3.zero, rotation, new Vector3(1f, 1f, -1f)).inverse * camera.cameraToWorldMatrix;
            int countX = settings.shadowMap.width / settings.textureSize;
            int countY = settings.shadowMap.height / settings.textureSize;
            int num3 = 0;
            foreach (CharacterShadowComponent component in characters)
            {
                int yIndex = num3 / countX;
                int xIndex = num3 - (yIndex * countX);
                Rect atlasData = CalculateLocationInAtlas(countX, countY, xIndex, yIndex);
                MyShadowInternal characterInternal = component.GetComponent<MyShadowInternal>();
                Bounds projectionBoundInLightSpace = characterInternal.ProjectionBoundInLightSpace;
                this.SetProjectorData(characterInternal, localToWorldMatrix.MultiplyPoint3x4(projectionBoundInLightSpace.center), rotation, component, atlasData);
                Vector3 center = projectionBoundInLightSpace.center;
                Matrix4x4 identity = Matrix4x4.identity;
                identity.SetColumn(3, new Vector4(-center.x, -center.y, center.z, 1f));
                Matrix4x4 matrixx6 = identity * matrixx4;
                Vector3 extents = projectionBoundInLightSpace.extents;
                Vector3 vector3 = projectionBoundInLightSpace.extents;
                float num6 = Mathf.Max(extents.x, vector3.y);
                Vector3 vector4 = projectionBoundInLightSpace.extents;
                float z = vector4.z;
                float left = ((-xIndex * 2) - 1) * num6;
                float right = (((-xIndex * 2) - 1) + (countX * 2)) * num6;
                float bottom = ((-yIndex * 2) - 1) * num6;
                float top = (((-yIndex * 2) - 1) + (countY * 2)) * num6;
                Matrix4x4 matrixx7 = Matrix4x4.Ortho(left, right, bottom, top, -z, z);
                characterInternal.CasterMaterial.SetMatrix("ViewToAtlas", GL.GetGPUProjectionMatrix(matrixx7 * matrixx6, true));
                num3++;
            }
        }

        private Projector CreateProjector(CharacterShadowCommonSettingsComponent shadowCommonSettings)
        {
            Projector projector = new GameObject("CharacterShadowProjector") { transform = { parent = shadowCommonSettings.transform } }.AddComponent<Projector>();
            projector.orthographic = true;
            projector.ignoreLayers = (int) shadowCommonSettings.ignoreLayers;
            projector.material = new Material(shadowCommonSettings.receiverShader);
            projector.material.mainTexture = shadowCommonSettings.shadowMap;
            return projector;
        }

        private void GenerateDrawCharacterCommands(CommandBuffer commandBuffer, MyShadowCaster casters, MyShadowInternal shadowInternal)
        {
            List<Renderer> renderers = casters.Renderers;
            for (int i = 0; i < renderers.Count; i++)
            {
                commandBuffer.DrawRenderer(renderers[i], shadowInternal.CasterMaterial);
            }
        }

        private void GenerateDrawCommandBuffer(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> characters)
        {
            RenderTexture shadowMap = settings.shadowMap;
            CommandBuffer commandBuffer = settings.CommandBuffer;
            commandBuffer.Clear();
            commandBuffer.GetTemporaryRT(settings.RawShadowMapNameId, shadowMap.width, shadowMap.height, 0x10, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.SetRenderTarget(settings.RawShadowMapId);
            commandBuffer.ClearRenderTarget(true, true, Color.black, 0f);
            foreach (CharacterShadowComponent component in characters)
            {
                this.GenerateDrawCharacterCommands(commandBuffer, component.GetComponent<MyShadowCaster>(), component.GetComponent<MyShadowInternal>());
            }
            commandBuffer.GetTemporaryRT(settings.BlurredShadowMapNameId, shadowMap.width, shadowMap.height, 0, FilterMode.Point, shadowMap.format, RenderTextureReadWrite.Linear);
            commandBuffer.Blit(settings.RawShadowMapId, settings.BlurredShadowMapId, settings.HorizontalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.RawShadowMapNameId);
            commandBuffer.Blit(settings.BlurredShadowMapId, new RenderTargetIdentifier(shadowMap), settings.VerticalBlurMaterial, settings.blurSize);
            commandBuffer.ReleaseTemporaryRT(settings.BlurredShadowMapNameId);
        }

        public void Init(CharacterShadowComponent characterShadow)
        {
            MyShadowInternal internal2 = characterShadow.gameObject.AddComponent<MyShadowInternal>();
            internal2.Projector = this.CreateProjector(this.shadowCommonSettings);
            internal2.BaseAlpha = characterShadow.color.a;
            internal2.CasterMaterial = new Material(this.shadowCommonSettings.casterShader);
            Debug.Log("Shadow System Init " + this.shadowCommonSettings.casterShader.name);
        }

        private void SetProjectorData(MyShadowInternal characterInternal, Vector3 position, Quaternion rotation, CharacterShadowComponent characterShadow, Rect atlasData)
        {
            Projector projector = characterInternal.Projector;
            Bounds projectionBoundInLightSpace = characterInternal.ProjectionBoundInLightSpace;
            float num = projectionBoundInLightSpace.size.x / projectionBoundInLightSpace.size.y;
            projector.transform.position = position;
            projector.transform.rotation = rotation;
            projector.orthographicSize = Mathf.Max(projectionBoundInLightSpace.extents.x, projectionBoundInLightSpace.extents.y);
            projector.aspectRatio = num;
            projector.nearClipPlane = -projectionBoundInLightSpace.extents.z;
            projector.farClipPlane = projectionBoundInLightSpace.extents.z + characterShadow.attenuation;
            float num2 = projectionBoundInLightSpace.size.z + 0.01f;
            float x = num;
            float y = (num2 + characterShadow.attenuation) / num2;
            float z = (characterShadow.attenuation <= 0f) ? 100000f : (1f / (characterShadow.attenuation / num2));
            Material material = projector.material;
            material.SetVector("Params", new Vector4(x, y, z, (characterShadow.backFadeRange <= 0f) ? 100000f : (1f / (characterShadow.backFadeRange / num2))));
            material.mainTextureOffset = atlasData.position;
            material.mainTextureScale = atlasData.size;
            material.color = characterShadow.color;
        }

        public void Start()
        {
            this.shadowCommonSettings = base.GetComponent<CharacterShadowCommonSettingsComponent>();
            foreach (CharacterShadowComponent component in this.characters)
            {
                this.Init(component);
            }
        }

        public void Update()
        {
            this.UpdateBoundsAndCullCharacters(this.visibleCharacters, this.characters, this.camera, this.shadowCommonSettings);
            this.UpdateShadowMapSize(this.shadowCommonSettings, this.visibleCharacters, this.characters);
            if (this.shadowCommonSettings.shadowMap != null)
            {
                Camera camera = this.shadowCommonSettings.Camera;
                this.CalculateProjectionData(this.shadowCommonSettings, this.visibleCharacters, camera);
                this.GenerateDrawCommandBuffer(this.shadowCommonSettings, this.visibleCharacters);
                camera.Render();
            }
            this.visibleCharacters.Clear();
        }

        private void UpdateBoundsAndCullCharacters(ICollection<CharacterShadowComponent> collector, ICollection<CharacterShadowComponent> characters, Camera camera, CharacterShadowCommonSettingsComponent settings)
        {
            Matrix4x4 localToWorldMatrix = settings.virtualLight.localToWorldMatrix;
            Matrix4x4 worldToLocalMatrix = settings.virtualLight.worldToLocalMatrix;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes((camera.projectionMatrix * camera.worldToCameraMatrix) * localToWorldMatrix);
            int num = 0;
            int maxCharactersCountInAtlas = settings.MaxCharactersCountInAtlas;
            foreach (CharacterShadowComponent component in characters)
            {
                bool flag = num < maxCharactersCountInAtlas;
                Bounds boundsInLightSpace = BoundsUtils.TransformBounds(component.GetComponent<MyShadowCaster>().BoundsInWorldSpace, worldToLocalMatrix);
                CharacterShadowComponent component2 = component;
                MyShadowInternal internal2 = component.GetComponent<MyShadowInternal>();
                internal2.ProjectionBoundInLightSpace = this.CalculateProjectionBoundInLightSpace(settings, boundsInLightSpace, component2.offset);
                Bounds projectionBoundInLightSpace = internal2.ProjectionBoundInLightSpace;
                projectionBoundInLightSpace.max += new Vector3(0f, 0f, component2.attenuation);
                flag &= GeometryUtility.TestPlanesAABB(planes, projectionBoundInLightSpace);
                internal2.Projector.enabled = flag;
                if (flag)
                {
                    collector.Add(component);
                }
                num++;
            }
        }

        private void UpdateShadowMapSize(CharacterShadowCommonSettingsComponent settings, ICollection<CharacterShadowComponent> visibleCharacters, ICollection<CharacterShadowComponent> allCharacters)
        {
            int num = (settings.shadowMap != null) ? settings.shadowMap.width : 0;
            int num2 = Mathf.CeilToInt(Mathf.Sqrt((float) visibleCharacters.Count));
            int width = Mathf.NextPowerOfTwo(settings.textureSize * num2);
            int num4 = Mathf.CeilToInt(Mathf.Sqrt((float) allCharacters.Count));
            if ((width > num) | ((2 * Mathf.NextPowerOfTwo(settings.textureSize * num4)) < num))
            {
                Destroy(settings.shadowMap);
                settings.shadowMap = null;
                if (width > 0)
                {
                    settings.shadowMap = new RenderTexture(width, width, 0, settings.ShadowMapTextureFormat, RenderTextureReadWrite.Linear);
                    settings.shadowMap.isPowerOfTwo = true;
                    settings.shadowMap.filterMode = FilterMode.Bilinear;
                    settings.shadowMap.useMipMap = false;
                }
                foreach (CharacterShadowComponent component in allCharacters)
                {
                    component.GetComponent<MyShadowInternal>().Projector.material.mainTexture = settings.shadowMap;
                }
            }
        }
    }
}

