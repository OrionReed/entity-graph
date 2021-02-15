using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OrionReed
{
  public static class PoissonSampler
  {
    public static List<Vector2> GenerateSamples(System.Random rng, float radius, float width, float height, int k = 30)
    {
      List<Vector2> samples = new List<Vector2>();
      // cell size to guarantee that each cell within the accelerator grid can have at most one sample
      float cellSize = radius / (float)Math.Sqrt(radius);

      // dimensions of our accelerator grid
      int acceleratorWidth = (int)Math.Ceiling(width / cellSize);
      int acceleratorHeight = (int)Math.Ceiling(height / cellSize);

      // accelerator grid to speed up rejection of generated samples
      int[,] accelerator = new int[acceleratorHeight, acceleratorWidth];

      // initializer point right at the center
      Vector2 initializerPoint = new Vector2(width / 2, height / 2);

      // keep track of our active samples
      List<Vector2> activeSamples = new List<Vector2> { initializerPoint };

      // begin sample generation
      while (activeSamples.Count != 0)
      {
        // pop off the most recently added samples and begin generating addtional samples around it
        int index = rng.Next(0, activeSamples.Count);
        Vector2 currentOrigin = activeSamples[index];
        bool isValid = false; // need to keep track whether or not the sample we have meets our criteria

        // attempt to randomly place a point near our current origin up to _k rejections
        for (int i = 0; i < k; i++)
        {
          // create a random direction to place a new sample at
          float angle = (float)(rng.NextDouble() * Math.PI * 2);
          Vector2 direction;
          direction.x = (float)Math.Sin(angle);
          direction.y = (float)Math.Cos(angle);

          // create a random distance between r and 2r away for that direction
          float distance = rng.NextFloat(radius, 2 * radius);
          direction.x *= distance;
          direction.y *= distance;

          // create our generated sample from our currentOrigin plus our new direction vector
          Vector2 generatedSample = new Vector2(currentOrigin.x + direction.x, currentOrigin.y + direction.y);

          int cellX = (int)(generatedSample.x / cellSize);
          int cellY = (int)(generatedSample.y / cellSize);

          isValid = IsGeneratedSampleValid(generatedSample, cellX, cellY, width, height, radius, samples, accelerator);

          if (isValid)
          {
            activeSamples.Add(generatedSample); // we may be able to add more samples around this valid generated sample later
            samples.Add(generatedSample);
            // mark the generated sample as "taken" on our accelerator
            accelerator[cellX, cellY] = samples.Count;
            break; // restart since we successfully generated a point
          }
        }

        if (!isValid)
        {
          activeSamples.RemoveAt(index);
        }
      }
      return samples;
    }

    private static bool IsGeneratedSampleValid(Vector2 sample, int cellX, int cellY, float width, float height, float radius, List<Vector2> samples, int[,] accelerator)
    {
      // is our generated sample within our boundaries?
      if (sample.x < 0 || sample.x >= height || sample.y < 0 || sample.y >= width)
      {
        return false; // out of bounds
      }

      // TODO - The +/- 2 might need to be 3, apparently, check the code this is derived from to try and see why
      // create our search area bounds
      int startX = Math.Max(0, cellX - 2);
      int endX = Math.Min(cellX + 2, accelerator.GetLength(0) - 1);

      int startY = Math.Max(0, cellY - 2);
      int endY = Math.Min(cellY + 2, accelerator.GetLength(1) - 1);

      // search within our boundaries for another sample
      for (int x = startX; x <= endX; x++)
      {
        for (int y = startY; y <= endY; y++)
        {
          int index = accelerator[x, y] - 1; // index of sample at this point (if there is one)

          if (index >= 0) // in each point for the accelerator where we have a sample we put the current size of the number of samples
          {
            // compute Euclidean distance squared (more performant as there is no square root)
            float distance = (sample - samples[index]).sqrMagnitude;
            if (distance < radius * radius)
            {
              return false; // too close to another point
            }
          }
        }
      }
      return true; // this is a valid generated sample as there are no other samples too close to it
    }
  }
}