using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ioSS.Util.Maths.Geometry;

namespace ioSS.Util.Maths
{
    public static class Geom
    {
        /*
        public static bool Circumcircle2(Vector2 p0, Vector2 p1, Vector2 p2, out Vector2 center,
            out float radiusSqr)
        {
            double dp0x = p0.x;
            double dp0y = p0.y;
            double dp1x = p1.x;
            double dp1y = p1.y;
            double dp2x = p2.x;
            double dp2y = p2.y;
            var det = (dp0x - dp2x) * (dp1y - dp2y) - (dp1x - dp2x) * (dp0y - dp2y);
            if (det == 0) //TODO use epsilon / approx
            {
                center = new Vector2(float.NaN, float.NaN);
                radiusSqr = float.NaN;
                return false;
            }

            var cent_x = (((dp0x - dp2x) * (dp0x + dp2x) + (dp0y - dp2y) * (dp0y + dp2y)) / 2 * (dp1y - dp2y)
                          - ((dp1x - dp2x) * (dp1x + dp2x) + (dp1y - dp2y) * (dp1y + dp2y)) / 2 * (dp0y - dp2y))
                         / det;

            var cent_y = (((dp1x - dp2x) * (dp1x + dp2x) + (dp1y - dp2y) * (dp1y + dp2y)) / 2 * (dp0x - dp2x)
                          - ((dp0x - dp2x) * (dp0x + dp2x) + (dp0y - dp2y) * (dp0y + dp2y)) / 2 * (dp1x - dp2x))
                         / det;

            center = new Vector2((float) cent_x, (float) cent_y);
            radiusSqr = (float) ((dp2x - cent_x) * (dp2x - cent_x) + (dp2y - cent_y) * (dp2y - cent_y));
            return true;
        }
        */

        public static Vector2 CentroidOfPoly(IEnumerable<Vector2> _pts)
        {
            var count = 0;
            float xSum = 0;
            float ySum = 0;
            foreach (var pt in _pts)
            {
                count++;
                xSum = xSum + pt.x;
                ySum = ySum + pt.y;
            }

            return new Vector2(xSum / count, ySum / count);
        }
        
        public static Vector3 CentroidOfPoly(IEnumerable<Vector3> _pts)
        {
            var count = 0;
            float xSum = 0;
            float ySum = 0;
            float zSum = 0;
            foreach (var pt in _pts)
            {
                count++;
                xSum += pt.x;
                ySum += pt.y;
                zSum += pt.z;
            }

            return new Vector3(xSum / count, ySum / count, zSum / count);
        }
        
        


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Circumcircle(Vector2 a, Vector2 b, Vector2 c,
            out float _centX, out float _centY, out float radiusSqr)
        {
            var A = b.x - a.x;
            var B = b.y - a.y;
            var C = c.x - a.x;
            var D = c.y - a.y;
            var E = A * (a.x + b.x) + B * (a.y + b.y);
            var F = C * (a.x + c.x) + D * (a.y + c.y);
            var G = 2 * (A * (c.y - b.y) - B * (c.x - b.x));

            float dx, dy;

            _centX = (D * E - B * F) / G;
            _centY = (A * F - C * E) / G;
            dx = _centX - a.x;
            dy = _centY - a.y;

            radiusSqr = dx * dx + dy * dy;

            /* If the points of the triangle are collinear, then just find the
             * extremes and use the midpoint as the center of the circumcircle. */

            /*
            float minx, miny;
            if(Math.Abs(G) < 0.000001)
            {
                minx = Min(a.x, b.x, c.x);
                miny = Min(a.y, b.y, c.y);
                dx = (Max(a.x, b.x, c.x) - minx) * 0.5f;
                dy = (Max(a.y, b.y, c.y) - miny) * 0.5f;

                _centX = minx + dx;
                _centY = miny + dy;
                radiusSqr = dx * dx + dy * dy;
            } else {
                _centX = (D * E - B * F) / G;
                _centY = (A * F - C * E) / G;
                dx = _centX - a.x;
                dy = _centY - a.y;

                radiusSqr = dx * dx + dy * dy;
            }

            return true;
            */
        }

        public static bool AreColinear(Vector2 _v0, Vector2 _v1, Vector2 _v2, float _epsilon)
        {
            return ((_v0.y - _v1.y) * (_v0.x - _v2.x)).ApproxEqual((_v0.y - _v2.y) * (_v0.x - _v1.x), _epsilon);
        }
        
        public static Vector2 IntersectRays(Vector2 _rayA, Vector2 _rayB, Vector2 _originA, Vector2 _originB)
        {
            var dx = _originB.x - _originA.x;
            var dy = _originB.y - _originA.y;
            var det = _rayB.x * _rayA.y - _rayB.y * _rayA.x;
            if (det == 0)
                return Vector2.positiveInfinity;
            var u = (dy * _rayB.x - dx * _rayB.y) / det;
            var v = (dy * _rayA.x - dx * _rayA.y) / det;
            if (u < 0 || v < 0)
                return Vector2.positiveInfinity;
            return new Vector2(_originA.x + _rayA.x * u, _originA.y + _rayA.y * u);
        }
        
        public static Vector2 Intersect(Vector2 _ptA1, Vector2 _ptA2, Vector2 _ptB1, Vector2 _ptB2) 
        { 
            // Line AB represented as a1x + b1y = c1 
            double a1 = _ptA2.y - _ptA1.y; 
            double b1 = _ptA1.x - _ptA2.x; 
            double c1 = a1*(_ptA1.x) + b1*(_ptA1.y); 
  
            // Line CD represented as a2x + b2y = c2 
            double a2 = _ptB2.y - _ptB1.y; 
            double b2 = _ptB1.x - _ptB2.x; 
            double c2 = a2*(_ptB1.x)+ b2*(_ptB1.y); 
  
            double determinant = a1*b2 - a2*b1; 
  
            if (determinant == 0) 
            { 
                // The lines are parallel. This is simplified 
                // by returning a pair of FLT_MAX 
                return Vector2.positiveInfinity; 
            } 
            double x = (b2*c1 - b1*c2)/determinant; 
            double y = (a1*c2 - a2*c1)/determinant;
            return new Vector2((float)x, (float)y);
            
        } 

        public static void SplitTriangle(Vector2[] _triangle, out Vector2[] _topTri, out Vector2[] _botTri)
        {
            // See http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html
            //Sort by y
            var vertList = new List<Vector2>(_triangle);
            vertList.Sort((_a, _b) => _a.y.CompareTo(_b.y)); //TODO confirm 0 is smallest
        
            //TODO see if flat already
            //Split into two triangles with flats
            var vLow = vertList[0];
            var vMid = vertList[1];
            var vHi = vertList[2];
            
            var v4 = Intersect(vHi, vLow, vMid, Vector2.right);

            _topTri = new [] {vHi,v4,vMid};
            _botTri = new [] {vMid,v4,vLow};
        }
        
        public static bool PointInTriangle(Vector2 p, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
            var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

            if (s < 0 != t < 0)
                return false;

            var A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;
            if (A < 0.0)
            {
                s = -s;
                t = -t;
                A = -A;
            }

            return s > 0 && t > 0 && s + t <= A;
        }

        
        //Don't have to worry about inf or close to zero cases
        public static bool ApproxEqual(this float _a, float _b, float _epsilon)
        {
            return Math.Abs(_a - _b) < _epsilon;
        }
        
    }
}