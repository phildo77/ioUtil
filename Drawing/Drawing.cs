using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ioSS.Util.Maths.Geometry;

namespace ioSS.Util.Drawing
{
    public class Canvas
    {
        public readonly int Height;
        public readonly int Width;
        public Color[] Bitmap;

        public Canvas(int _width, int _height)
        {
            Width = _width;
            Height = _height;
            Bitmap = new Color[Width * Height];
        }

        private void Paint(Pixel _p, Brush _brush)
        {
            foreach (var coord in _brush.Footprint)
            {
                var x = _p.x + (int) coord.x;
                var y = _p.y + (int) coord.y;
                if (x < 0 || x >= Width) continue;
                if (y < 0 || y >= Height) continue;
                Paint(x, y, _brush.Color);
            }
        }

        public static HashSet<Pixel> StageTriangle(Pixel _p1, Pixel _p2, Pixel _p3)
        {
            var vertices = new[]
            {
                _p1, _p2, _p3
            };

            // Sort by y
            Array.Sort(vertices, (_a, _b) => _a.y.CompareTo(_b.y));

            var bhLong = new Bresenham(vertices[0], vertices[2]);
            var bhShortA = new Bresenham(vertices[0], vertices[1]);
            var bhShortB = new Bresenham(vertices[1], vertices[2]);

            var curY = bhLong.Current.y;
            var stagedPixels = new HashSet<Pixel> {bhLong.Current};
            do
            {
                if (!bhShortA.End)
                {
                    do bhLong.MoveNext(); while (bhLong.Current.y == curY && !bhLong.End);
                    do bhShortA.MoveNext(); while (bhShortA.Current.y == curY && !bhShortA.End);
                    //Debug
                    if(bhLong.Previous.y != bhShortA.Previous.y)
                        throw new Exception("Y Mismatch");
                    stagedPixels.UnionWith(StageLineHorizontal(curY, bhLong.Previous.x, bhShortA.Previous.x));
                    curY = bhLong.Current.y;
                }
                else
                {
                    do bhShortB.MoveNext(); while (bhShortB.Current.y != bhLong.Current.y && !bhShortB.End);
                    //Debug
                    if(bhLong.Previous.y != bhShortB.Previous.y)
                        throw new Exception("Y Mismatch");
                    stagedPixels.UnionWith(StageLineHorizontal(curY, bhLong.Previous.x, bhShortB.Previous.x));
                    
                    do bhLong.MoveNext(); while (bhLong.Current.y == curY && !bhLong.End);
                    curY = bhLong.Current.y;
                }
            } while (!bhLong.End);

            return stagedPixels;
        }

        public static HashSet<Pixel> StageLineHorizontal(int _y, int _x0, int _x1)
        {
            var startX = _x0;
            var endX = _x1;
            if (_x0 > _x1)
            {
                startX = _x1;
                endX = _x0;
            }
            var pixels = new HashSet<Pixel>();
            for(int x = startX; x <= endX; ++x)
                pixels.Add(new Pixel(x, _y));
            return pixels;
        }
        
        public static HashSet<Pixel> StageLine(Pixel _p1, Pixel _p2)
        {
            var pixels = new HashSet<Pixel>();
            var bh = new Bresenham(_p1, _p2);
            do
            {
                pixels.Add(bh.Current);
            } while (bh.MoveNext());

            return pixels;
        }

        public void PaintLine(Pixel _p1, Pixel _p2, Color _color)
        {
            var pixels = StageLine(_p1, _p2);
            Paint(pixels, _color);
        }
        
        public void Paint(int _x, int _y, Color _color)
        {
            if (_x < 0 || _x >= Width || _y < 0 || _y >= Height)
            {
                Trace.WriteLine("Canvas Paint out of range " + new Pixel(_x,_y));
                return;
            }
            Bitmap[_y * Width + _x] = _color;
        }

        public void Paint(Pixel _pixel, Color _color)
        {
            Paint(_pixel.x, _pixel.y, _color);
        }

        public void Paint(IEnumerable<Pixel> _pixels, Color _color)
        {
            foreach (var pixel in _pixels)
                Paint(pixel, _color);
        }
        
        public void BrushLine(Canvas.Pixel _p1, Canvas.Pixel _p2, Canvas.Brush _brush)
        {
            var bh = new Bresenham(_p1, _p2);
            do
            {
                Paint(bh.Current, _brush);
            } while (bh.MoveNext());
        }

        public class Bresenham : IEnumerator<Pixel>
        {
            public readonly Pixel P1, P2;
            private Pixel Cur;
            public bool End;
            public int Longest, Shortest, dx1, dy1, dx2, dy2, Numerator;

           public Bresenham(Pixel _p1, Pixel _p2)
            {
                P1 = _p1;
                P2 = _p2;
                var w = P2.x - P1.x;
                var h = P2.y - P1.y;
                dx1 = dy1 = dx2 = dy2 = 0;
                if (w < 0) dx1 = -1;
                else if (w > 0) dx1 = 1;
                if (h < 0) dy1 = -1;
                else if (h > 0) dy1 = 1;
                if (w < 0) dx2 = -1;
                else if (w > 0) dx2 = 1;
                Longest = Math.Abs(w);
                Shortest = Math.Abs(h);
                if (!(Longest > Shortest))
                {
                    Longest = Math.Abs(h);
                    Shortest = Math.Abs(w);
                    if (h < 0) dy2 = -1;
                    else if (h > 0) dy2 = 1;
                    dx2 = 0;
                }

                Reset();
            }

            public int Iter { get; private set; }

            public Pixel Previous { get; private set; }


            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                Previous = Cur;
                if (Cur == P2)
                {
                    Cur = Pixel.Invalid;
                    End = true;
                    return false;
                }

                Numerator += Shortest;
                if (!(Numerator < Longest))
                {
                    Numerator -= Longest;
                    Cur.x += dx1;
                    Cur.y += dy1;
                }
                else
                {
                    Cur.x += dx2;
                    Cur.y += dy2;
                }

                Iter++;
                return true;
            }

            public void Reset()
            {
                Previous = Pixel.Invalid;
                Cur = P1;
                Iter = 0;
                Numerator = Longest >> 1;
                End = false;
            }

            public Pixel Current => Cur;
            object IEnumerator.Current => Current;
        }

        public struct Pixel : IEquatable<Pixel>
        {
            public static readonly Pixel Invalid = new Pixel(int.MaxValue, int.MaxValue);
            public int x;
            public int y;

            public Pixel(int _x, int _y)
            {
                x = _x;
                y = _y;
            }

            public static bool operator ==(Pixel _a, Pixel _b)
            {
                return _a.Equals(_b);
            }

            public static bool operator !=(Pixel _a, Pixel _b)
            {
                return !_a.Equals(_b);
            }

            public bool Equals(Pixel other)
            {
                return x == other.x && y == other.y;
            }

            public override bool Equals(object obj)
            {
                return obj is Pixel other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (x * 397) ^ y;
                }
            }
        }

        public class Brush
        {
            public enum Shape
            {
                Square,
                Circle
            }

            public Color Color;
            public List<Vector2> Footprint; //TODO make int coord storage class

            public Shape shape;
            public int Size;

            public Brush(Shape _shape, int _size, Color _color)
            {
                shape = _shape;
                Size = _size;
                Color = _color;
                Footprint = new List<Vector2>();

                var rad = Size / 2;
                var radSqr = rad * rad;

                for (var x = -rad; x <= rad; ++x)
                for (var y = -rad; y <= rad; ++y)
                {
                    var coord = new Vector2(x, y);
                    if (shape == Shape.Circle)
                        if (coord.sqrMagnitude > radSqr)
                            continue;

                    Footprint.Add(coord);
                }
            }
        }
    }


    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color(float _r, float _g, float _b, float _a = 1f)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }

        public Color(byte _r, byte _g, byte _b, byte _a = 255)
        {
            r = _r / 255f;
            g = _g / 255f;
            b = _b / 255f;
            a = _a / 255f;
        }
    }
}