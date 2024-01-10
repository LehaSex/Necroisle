using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Necroisle
{

    /// <summary>
    /// XY coordinates of chunks
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    public struct XY {
        public int x;
        public int y;
    };

    /// <summary>
    /// Weight coefficients for each color
    /// </summary>
    /// <param name="w1">Red weight</param>
    /// <param name="w2">Green weight</param>
    /// <param name="w3">Blue weight</param>
    
    public struct WeightCoef {
        //r
        public double w1;
        //g
        public double w2;
        //b
        public double w3;
    };
    /// <summary>
    /// Color of chunk
    /// </summary>
    /// <param name="r">Red</param>
    /// <param name="g">Green</param>
    /// <param name="b">Blue</param>
    public struct RGB {
        public double r;
        public double g;
        public double b;
    };

    /// <summary>
    /// Self Organizing Kohonen Map Chunk
    /// </summary>
    public class MapChunk {
        
        private RGB color;
        private WeightCoef weights;
        private XY coordinates;

        /// <summary>
        /// Chunk's colors and coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="random">Seed</param>
        public MapChunk(int x, int y, Random random)
        {
            this.coordinates = new XY
            {
                x = x,
                y = y
            };
            this.color.r = 255;
            this.color.g = 255;
            this.color.b = 255;
            initNeuronWeights(random);
        }

        /// <summary>
        /// Initialize chunk's weights
        /// </summary>
        /// <param name="random">Seed</param>

        public void initNeuronWeights(Random random)
        {
            this.weights.w1 = random.NextDouble();
            this.weights.w2 = random.NextDouble();
            this.weights.w3 = random.NextDouble();
        }

        /// <summary>
        /// Renew chunk's color
        /// </summary>
        public void recolor()
        {

            this.color.r = 255 * this.weights.w1;
            this.color.g = 255 * this.weights.w2;
            this.color.b = 255 * this.weights.w3;
        }
        
        public double getW1()
        {
            return this.weights.w1;
        }

        public double getW2()
        {
            return this.weights.w2;
        }

        public double getW3()
        {
            return this.weights.w3;
        }

        public void setW1(double w1)
        {
            this.weights.w1 = w1;
        }

        public void setW2(double w2)
        {
            this.weights.w2 = w2;
        }

        public void setW3(double w3)
        {
            this.weights.w3 = w3;
        }

        public RGB GetColor()
        {
            return this.color;
        }

        public int X { get => coordinates.x; set => coordinates.x = value; }
        public int Y { get => coordinates.y; set => coordinates.y = value; }

    }

    public class MapGenerator {

        /// <summary>
        /// Height of map
        /// </summary>
        private static int height = 500;
        /// <summary>
        /// Width of map
        /// </summary>
        private static int width = 500;

        /// <summary>
        /// Get and set height of map
        /// </summary>
        public static int Height { get => height; set => height = value; }
        /// <summary>
        /// Get and set width of map
        /// </summary>
        public static int Width { get => width; set => width = value; }

        /// <summary>
        /// The initial value of the dynamic (shrinking) radius of the neighbourhood function
        /// </summary>
        public float sigma0 = Math.Max(Width, Height) / 2;

        private static RGB Red = new() { r = 1, g = 0, b = 0 };
        private static RGB Green = new() { r = 0, g = 1, b = 0 };
        private static RGB Blue = new() { r = 0, g = 0, b = 1 };
        private static RGB White = new() { r = 1, g = 1, b = 1 };

        /// <summary>
        /// Number of colors
        /// </summary>
        private int countOfColors = 4;
        public RGB[] test = { Red,
                            Green,
                            Blue,
                            White };

        private int seed;
        MapChunk[,] neurons;
        List<MapChunk> adjacents;
        Random random;

        /// <summary>
        /// Set seed for random, height and width of map
        /// </summary>
        /// <param name="seed">Seed</param>
        /// <param name="height">Height of map</param>
        /// <param name="width">Width of map</param>
        public MapGenerator(int seed, int height, int width)
        {
            this.seed = seed;
            Height = height;
            Width = width;
            random = new Random(seed);
        }

        /// <summary>
        /// Initialize all chunks
        /// </summary>
        public void InitChunks()
        {
            neurons = new MapChunk[Height, Width];
            adjacents = new List<MapChunk>();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {

                    neurons[i, j] = new MapChunk(i, j, random);
                }
            }
        }
        
        /// <summary>
        /// Find winner chunk
        /// </summary>
        /// <param name="arr">Color of chunk</param>
        public XY FindWinner(RGB arr)
        {
            double min = Double.MaxValue;
            double distance;
            XY winner;
            winner.x = 0;
            winner.y = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {

                    distance = Math.Sqrt(Math.Pow(arr.r - neurons[i, j].getW1(), 2) + Math.Pow(arr.g - neurons[i, j].getW2(), 2) + Math.Pow(arr.b - neurons[i, j].getW3(), 2));

                    if (distance < min)
                    {
                        min = distance;
                        winner.y = i;
                        winner.x = j;
                    }
                }
            }
            return winner;
        }

        /// <summary>
        /// Find adjacent chunks
        /// </summary>
        public void FindAdjacents(XY winner, double sigma)
        {
            adjacents.Clear();
            int x = winner.x;
            int y = winner.y;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Math.Sqrt(Math.Pow(j - x, 2) + Math.Pow(i - y, 2)) < sigma)
                    {
                        // Add adjacent chunk to list
                        adjacents.Add(neurons[j, i]);
                    }
                }
            }
        }

        /// <summary>
        /// Recolor all chunks
        /// </summary>
        public void recolorMap()
        {

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    neurons[i, j].recolor();
                }
            }
        }

        /// <summary>
        /// Learn map
        /// </summary>
        /// <param name="T">Number of iterations</param>
        /// <param name="L0">Initial learning rate</param>
        public void Learn(int T, double L0)
        {
            double L;
            double radius;
            double sigma;
            double theta;
            double lambda = T / Math.Log(this.sigma0);

            // Пробегаемся по тестовой выборке
            for (int z = 0; z < countOfColors; z++)
            {
                XY winner = FindWinner(test[z]);
                //Обучаем T раз на каждом примере
                for (int t = 0; t < T; t++)
                {
                    sigma = this.sigma0 * Math.Exp(-t / lambda);
                    L = L0 * Math.Exp(-t / lambda);
                    FindAdjacents(winner, sigma);

                    for (int i = 0; i < adjacents.Count; i++)
                    {
                        radius = Math.Sqrt(Math.Pow(adjacents[i].X - winner.x, 2) + Math.Pow(adjacents[i].Y - winner.y, 2));
                        theta = Math.Exp(-Math.Pow(radius, 2) / (2 * Math.Pow(sigma, 2)));


                        neurons[adjacents[i].Y, adjacents[i].X].setW1(neurons[adjacents[i].Y, adjacents[i].X].getW1() + L * theta * (test[z].r - neurons[adjacents[i].Y, adjacents[i].X].getW1()));
                        neurons[adjacents[i].Y, adjacents[i].X].setW2(neurons[adjacents[i].Y, adjacents[i].X].getW2() + L * theta * (test[z].g - neurons[adjacents[i].Y, adjacents[i].X].getW2()));
                        neurons[adjacents[i].Y, adjacents[i].X].setW3(neurons[adjacents[i].Y, adjacents[i].X].getW3() + L * theta * (test[z].b - neurons[adjacents[i].Y, adjacents[i].X].getW3()));

                    }
                }
            }
            // Перекрашиваем карту
            recolorMap();
        }

        public RGB GetChunk(int x, int y)
        {
            return neurons[x, y].GetColor();
        }

    }
}