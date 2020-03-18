namespace ioSS.Util.Maths.Geometry
{
    public struct Rect
    {
        #region Fields

        #endregion Fields

        #region Constructors

        public Rect(float xMin, float yMin, float width, float height)
        {
            x = xMin;
            y = yMin;
            this.width = width;
            this.height = height;
        }

        public Rect(Vector2 minPosition, Vector2 size)
        {
            x = minPosition.x;
            y = minPosition.y;
            width = size.x;
            height = size.y;
        }

        public Rect(Rect source)
        {
            x = source.x;
            y = source.y;
            width = source.width;
            height = source.height;
        }

        #endregion Constructors

        #region Properties

        public static Rect zero => new Rect(0.0f, 0.0f, 0.0f, 0.0f);

        public Vector2 center
        {
            get => new Vector2(x + width / 2f, y + height / 2f);
            set
            {
                x = value.x - width / 2f;
                y = value.y - height / 2f;
            }
        }

        public float height { get; set; }

        public Vector2 max
        {
            get => new Vector2(xMax, yMax);
            set
            {
                xMax = value.x;
                yMax = value.y;
            }
        }

        public Vector2 min
        {
            get => new Vector2(xMin, yMin);
            set
            {
                xMin = value.x;
                yMin = value.y;
            }
        }

        public Vector2 position
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public Vector2 size
        {
            get => new Vector2(width, height);
            set
            {
                width = value.x;
                height = value.y;
            }
        }

        public float width { get; set; }

        public float x { get; set; }

        public float xMax
        {
            get => width + x;
            set => width = value - x;
        }

        public float xMin
        {
            get => x;
            set
            {
                var xMax = this.xMax;
                x = value;
                width = xMax - x;
            }
        }

        public float y { get; set; }

        public float yMax
        {
            get => height + y;
            set => height = value - y;
        }

        public float yMin
        {
            get => y;
            set
            {
                var yMax = this.yMax;
                y = value;
                height = yMax - y;
            }
        }

        #endregion Properties

        #region Methods

        public static bool operator !=(Rect lhs, Rect rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(Rect lhs, Rect rhs)
        {
            return lhs.x == (double) rhs.x && lhs.y == (double) rhs.y && lhs.width == (double) rhs.width &&
                   lhs.height == (double) rhs.height;
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= (double) xMin && point.x < (double) xMax && point.y >= (double) yMin &&
                   point.y < (double) yMax;
        }


        public void Encapsulate(Vector2 _point)
        {
            if (_point.x > xMax)
                xMax = _point.x;
            if (_point.x < xMin)
                xMin = _point.x;
            if (_point.y > yMax)
                yMax = _point.y;
            if (_point.y < yMin)
                yMin = _point.y;
        }

        public override bool Equals(object other)
        {
            if (!(other is Rect))
                return false;
            var rect = (Rect) other;
            return x.Equals(rect.x) && y.Equals(rect.y) && width.Equals(rect.width) && height.Equals(rect.height);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);
        }


        /// <summary>
        ///     <para>Returns a nicely formatted string for this Rectf.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object) x, (object) y,
                (object) width, (object) height);
        }

        /// <summary>
        ///     <para>Returns a nicely formatted string for this Rectf.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", (object) x.ToString(format),
                (object) y.ToString(format), (object) width.ToString(format), (object) height.ToString(format));
        }

        #endregion Methods
    }
}