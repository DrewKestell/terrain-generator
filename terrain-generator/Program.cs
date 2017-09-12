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
            var width = 800;
            var height = 600;
            var heightmap = new Bitmap(width, height);
            var diffusemap = new Bitmap(width, height);
            var random = new Random();
            var seed = random.Next(Int32.MaxValue);
            var fastNoise = new FastNoise(seed);
            fastNoise.SetNoiseType(NoiseType.PerlinFractal);
            fastNoise.SetFrequency(0.01f);
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

                    if (noise < 60)
                        diffuseColor = Color.FromArgb(120, 120, 120);
                    else if (noise > 60 && noise <= 80)
                        diffuseColor = Color.FromArgb(87, 165, 38);
                    else if (noise > 80 && noise <= 100)
                        diffuseColor = Color.FromArgb(112, 214, 49);
                    else if (noise > 100 && noise <= 130)
                        diffuseColor = Color.FromArgb(226, 190, 106);
                    else
                        diffuseColor = Color.FromArgb(34, 133, 214);

                    diffusemap.SetPixel(x, y, diffuseColor);
                }
            }

            heightmap.Save("height.jpg");
            diffusemap.Save("diffuse.jpg");
        }
    }
}