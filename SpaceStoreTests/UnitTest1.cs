using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceIndex;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceStoreTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var rng = new Random();
            var pts = new List<Point3d>();
            for (int i = 0; i < 5000000; i++)
            {
                var x = (rng.NextDouble() - 0.5) * 1024;
                var y = (rng.NextDouble() - 0.5) * 1024;
                var z = (rng.NextDouble() - 0.5) * 1024;
                var pt = new Point3d(x, y, z);
                pts.Add(pt);
            }
            var sw = new Stopwatch();
            sw.Start();

            var t1 = true;
            var t2= true;
            var L = 12.0;
            foreach (var pt in pts)
            { 
                var chunk1 = new SpaceChunk(pt, L);
                var chunk2 = new SpaceChunk(pt, L*2);
                var chunk4 = new SpaceChunk(pt, L * 4);
                var chunk2alt = chunk1.Parent;
                var chunk4alt = chunk2.Parent;
                t1 = t1 && chunk2.Equals( chunk2alt);
                t2 = t2 && chunk4.Equals(chunk4alt);
            }
            sw.Stop();

            Console.WriteLine($"Time to do chunk stuff = {sw.ElapsedMilliseconds} ms");
            sw.Restart();
            var d = new Dictionary<SpaceChunk, List<Point3d>>(pts.Count);

            foreach (var pt in pts)
            {
                var chunk = new SpaceChunk(pt, L);
                List<Point3d> res;
                if (d.TryGetValue(chunk, out res))
                { 
                    res.Add(pt);
                }
                else
                {
                    d[chunk] = new List<Point3d>() { pt };
                }
            }

            sw.Stop();

            Console.WriteLine($"Time to do fill dict = {sw.ElapsedMilliseconds} ms");

            sw.Restart();

            Parallel.ForEach(pts, pt =>
            {
                var chunk = new SpaceChunk(pt, L);
                if (d.TryGetValue(chunk, out List<Point3d> res))
                {
                    var dist = Point3d.DistanceSquaredBetween(pt, res[0]);
                    for (int i = 1; i < res.Count; i++)
                    {
                        dist = Math.Min(dist, Point3d.DistanceSquaredBetween(pt, res[i]));
                    }
                }
            });
            sw.Stop();
            Console.WriteLine($"Time to do seek in dict = {sw.ElapsedMilliseconds} ms");
           

            Assert.IsTrue(t1 && t2);
        }
    }
}
