﻿using System;
using DotSpatial.Topology.Algorithm.Locate;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Shape.Random
{
    /// <summary>
    /// Creates random point sets contained in a
    /// region defined by either a rectangular or a polygonal extent.
    /// </summary>
    /// <author>mbdavis</author>
    public class RandomPointsBuilder : GeometricShapeBuilder
    {
        #region Fields

        protected static readonly System.Random Rnd = new System.Random();
        private IPointOnGeometryLocator _extentLocator;
        private IGeometry _maskPoly;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a shape factory which will create shapes using the default
        /// <see cref="IGeometryFactory"/>.
        /// </summary>
        public RandomPointsBuilder()
            : this(new GeometryFactory())
        {
        }

        /// <summary>
        /// Create a shape factory which will create shapes using the given
        /// <see cref="IGeometryFactory"/>
        /// </summary>
        /// <param name="geomFact">The factory to use</param>
        public RandomPointsBuilder(IGeometryFactory geomFact)
            : base(geomFact)
        {
        }

        #endregion

        #region Methods

        /*
         * Same functionality in base class
         *
        protected override Coordinate CreateCoord(double x, double y)
        {
  	        Coordinate pt = new Coordinate(x, y);
  	        geomFactory.getPrecisionModel().makePrecise(pt);
            return pt;
        }
        */

        protected Coordinate CreateRandomCoord(IEnvelope env)
        {
            var x = env.Minimum.X + env.Width * Rnd.NextDouble();
            var y = env.Minimum.Y + env.Height * Rnd.NextDouble();

            return CreateCoord(x, y);
        }

        public override IGeometry GetGeometry()
        {
            var pts = new Coordinate[NumPoints];
            int i = 0;
            while (i < NumPoints)
            {
                var p = CreateRandomCoord(Extent);
                if (_extentLocator != null && !IsInExtent(p))
                    continue;
                pts[i++] = p;
            }
            return GeomFactory.CreateMultiPoint(pts);
        }

        protected bool IsInExtent(Coordinate p)
        {
            if (_extentLocator != null)
                return _extentLocator.Locate(p) != LocationType.Exterior;
            return Extent.Contains(p);
        }

        /// <summary>
        /// Sets a polygonal mask.
        /// </summary>
        /// <exception cref="ArgumentException">if the mask is not polygonal</exception>
        public void SetExtent(IGeometry mask)
        {
            if (!(mask is IPolygonal))
                throw new ArgumentException("Only polygonal extents are supported");

            _maskPoly = mask;
            Extent = mask.EnvelopeInternal;
            _extentLocator = new IndexedPointInAreaLocator(mask);
        }

        #endregion
    }
}