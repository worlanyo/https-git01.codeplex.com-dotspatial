using System;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Strtree;

namespace DotSpatial.Topology.Operation.Valid
{
    /**
     * Tests whether any of a set of {@link LinearRing}s are
     * nested inside another ring in the set, using a spatial
     * index to speed up the comparisons.
     *
     * @version 1.7
     */
    public class IndexedNestedRingTester
    {
        #region Fields

        private readonly GeometryGraph _graph;  // used to find non-node vertices
        private readonly IList<ILineString> _rings = new List<ILineString>();
        private readonly Envelope _totalEnv = new Envelope();
        private ISpatialIndex<ILineString> _index;
        private Coordinate _nestedPt;

        #endregion

        #region Constructors

        public IndexedNestedRingTester(GeometryGraph graph)
        {
            _graph = graph;
        }

        #endregion

        #region Properties

        public Coordinate NestedPoint { get { return _nestedPt; } }

        #endregion

        #region Methods

        public void Add(ILinearRing ring)
        {
            _rings.Add(ring);
            _totalEnv.ExpandToInclude(ring.EnvelopeInternal);
        }

        private void BuildIndex()
        {
            _index = new StRtree<ILineString>();

            for (int i = 0; i < _rings.Count; i++)
            {
                var ring = (ILinearRing)_rings[i];
                var env = ring.EnvelopeInternal;
                _index.Insert(env, ring);
            }
        }

        public bool IsNonNested()
        {
            BuildIndex();

            for (int i = 0; i < _rings.Count; i++)
            {
                var innerRing = (ILinearRing)_rings[i];
                var innerRingPts = innerRing.Coordinates;

                var results = _index.Query(innerRing.EnvelopeInternal);
                for (int j = 0; j < results.Count; j++)
                {
                    var searchRing = (ILinearRing)results[j];
                    var searchRingPts = searchRing.Coordinates;

                    if (innerRing == searchRing)
                        continue;

                    if (!innerRing.EnvelopeInternal.Intersects(searchRing.EnvelopeInternal))
                        continue;

                    Coordinate innerRingPt = IsValidOp.FindPointNotNode(innerRingPts, searchRing, _graph);
                    // Diego Guidi: removed => see Issue 121
                    //Assert.IsTrue(innerRingPt != null, "Unable to find a ring point not a node of the search ring");
                    /**
                     * If no non-node pts can be found, this means
                     * that the searchRing touches ALL of the innerRing vertices.
                     * This indicates an invalid polygon, since either
                     * the two holes create a disconnected interior, 
                     * or they touch in an infinite number of points 
                     * (i.e. along a line segment).
                     * Both of these cases are caught by other tests,
                     * so it is safe to simply skip this situation here.
                     */
                    if (innerRingPt == null)
                        continue;

                    Boolean isInside = CgAlgorithms.IsPointInRing(innerRingPt, searchRingPts);
                    if (isInside)
                    {
                        _nestedPt = innerRingPt;
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion
    }
}