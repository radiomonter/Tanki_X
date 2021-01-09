namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class SimpleLayoutUtility
    {
        private static List<Component> components = new List<Component>(3);
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<ISimpleLayoutElement, float> <>f__am$cache5;

        public static float GetFlexibleHeight(RectTransform rect)
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = e => e.flexibleHeight;
            }
            return GetLayoutProperty(rect, <>f__am$cache1, 0f);
        }

        public static float GetFlexibleSize(RectTransform rect, int axis) => 
            (axis != 0) ? GetFlexibleHeight(rect) : GetFlexibleWidth(rect);

        public static float GetFlexibleWidth(RectTransform rect)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = e => e.flexibleWidth;
            }
            return GetLayoutProperty(rect, <>f__am$cache0, 0f);
        }

        public static float GetLayoutProperty(RectTransform rect, Func<ISimpleLayoutElement, float> property, float defaultValue)
        {
            ISimpleLayoutElement element;
            return GetLayoutProperty(rect, property, defaultValue, out element);
        }

        public static float GetLayoutProperty(RectTransform rect, Func<ISimpleLayoutElement, float> property, float defaultValue, out ISimpleLayoutElement source)
        {
            source = null;
            if (rect == null)
            {
                return 0f;
            }
            float num = defaultValue;
            int num2 = -2147483648;
            rect.GetComponents(typeof(ISimpleLayoutElement), components);
            for (int i = 0; i < components.Count; i++)
            {
                ISimpleLayoutElement element = components[i] as ISimpleLayoutElement;
                if (!(element is Behaviour) || ((element as Behaviour).enabled && (element as Behaviour).isActiveAndEnabled))
                {
                    int layoutPriority = element.layoutPriority;
                    if (layoutPriority >= num2)
                    {
                        float num5 = property(element);
                        if (num5 >= 0f)
                        {
                            if (layoutPriority > num2)
                            {
                                num = num5;
                                num2 = layoutPriority;
                                source = element;
                            }
                            else if (num5 > num)
                            {
                                num = num5;
                                source = element;
                            }
                        }
                    }
                }
            }
            components.Clear();
            return num;
        }

        public static float GetMaxHeight(RectTransform rect)
        {
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = e => e.maxHeight;
            }
            return GetLayoutProperty(rect, <>f__am$cache5, 0f);
        }

        public static float GetMaxSize(RectTransform rect, int axis) => 
            (axis != 0) ? GetMaxHeight(rect) : GetMaxWidth(rect);

        public static float GetMaxWidth(RectTransform rect)
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = e => e.maxWidth;
            }
            return GetLayoutProperty(rect, <>f__am$cache3, 0f);
        }

        public static float GetMinHeight(RectTransform rect)
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = e => e.minHeight;
            }
            return GetLayoutProperty(rect, <>f__am$cache4, 0f);
        }

        public static float GetMinSize(RectTransform rect, int axis) => 
            (axis != 0) ? GetMinHeight(rect) : GetMinWidth(rect);

        public static float GetMinWidth(RectTransform rect)
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = e => e.minWidth;
            }
            return GetLayoutProperty(rect, <>f__am$cache2, 0f);
        }
    }
}

