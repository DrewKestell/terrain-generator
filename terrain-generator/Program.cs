using System;
using System.Collections.Generic;
using System.Drawing;
using static FastNoise;

namespace NoiseMapGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var width = int.Parse(args[0]);
            var height = int.Parse(args[1]);
            var threshold = int.Parse(args[2]);

            var heightmap = new Bitmap(width, height);
            var diffusemap = new Bitmap(width, height);
            var random = new Random();
            var seed = random.Next(int.MaxValue);
            var fastNoise = new FastNoise(seed);
            fastNoise.SetNoiseType(NoiseType.Perlin);
            fastNoise.SetFrequency(0.005f);
            fastNoise.SetInterp(Interp.Quintic);
            fastNoise.SetFractalType(FractalType.FBM);
            fastNoise.SetFractalOctaves(5);
            fastNoise.SetFractalLacunarity(2.0f);
            fastNoise.SetFractalGain(0.5f);

            var noiseValues = new List<float>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseValues.Add(fastNoise.GetNoise(x, y));
                }
            }

            noiseValues.Sort();
            var min = Math.Abs(noiseValues[0]);
            var max = noiseValues[noiseValues.Count - 1] + min;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var noise = (int)(((fastNoise.GetNoise(x, y) + min) / max) * 255);
                    heightmap.SetPixel(x, y, Color.FromArgb(noise, noise, noise));

                    Color diffuseColor;

                    if (noise < threshold)
                        diffuseColor = Color.FromArgb(0, 0, 0);
                    else
                        diffuseColor = Color.FromArgb(255, 255, 255);

                    diffusemap.SetPixel(x, y, diffuseColor);
                }
            }

            heightmap.Save("height.jpg");
            diffusemap.Save("diffuse.jpg");
        }
    }
}