namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public static class SmoothedTime
    {
        private static float MIN_FRAME_TIME = 0.01f;
        private static float MAX_FRAME_TIME = 0.5f;
        private static float LERP_FACTOR = 0.1f;
        private static int FRAME_COUNT = 30;
        private static int NOISE_COUNT = 5;
        private static float[] lastTimes = new float[FRAME_COUNT];
        private static float[] sortedTimes = new float[FRAME_COUNT];
        private static float lastFrameDeltaTime = 0f;
        private static int lastCalculatedFrame = 0;

        static SmoothedTime()
        {
            for (int i = 0; i < FRAME_COUNT; i++)
            {
                lastTimes[i] = MIN_FRAME_TIME;
            }
        }

        public static float GetSmoothDeltaTime()
        {
            if (Time.frameCount != lastCalculatedFrame)
            {
                if (lastFrameDeltaTime == 0f)
                {
                    lastFrameDeltaTime = Time.deltaTime;
                    return lastFrameDeltaTime;
                }
                float deltaTime = Time.deltaTime;
                if (deltaTime > MIN_FRAME_TIME)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= (FRAME_COUNT - 1))
                        {
                            lastTimes[0] = deltaTime;
                            int index = 0;
                            while (true)
                            {
                                if (index >= FRAME_COUNT)
                                {
                                    Array.Sort<float>(sortedTimes);
                                    float num4 = 0f;
                                    int num5 = 0;
                                    int num6 = NOISE_COUNT;
                                    while (true)
                                    {
                                        if (num6 >= (FRAME_COUNT - NOISE_COUNT))
                                        {
                                            if (num5 > 0)
                                            {
                                                float num7 = num4 / ((float) num5);
                                                deltaTime = (LERP_FACTOR * num7) + ((1f - LERP_FACTOR) * lastFrameDeltaTime);
                                            }
                                            break;
                                        }
                                        num4 += sortedTimes[num6];
                                        num5++;
                                        num6++;
                                    }
                                    break;
                                }
                                sortedTimes[index] = lastTimes[index];
                                index++;
                            }
                            break;
                        }
                        lastTimes[(FRAME_COUNT - num2) - 1] = lastTimes[(FRAME_COUNT - num2) - 2];
                        num2++;
                    }
                }
                lastFrameDeltaTime = deltaTime;
                lastCalculatedFrame = Time.frameCount;
            }
            return lastFrameDeltaTime;
        }
    }
}

