﻿using System;
using System.Globalization;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Mathematics
{
    /// <summary>
    /// Represents a vector in 3-dimensional Cartesian space.
    /// </summary>
    /// <author>Martin Davis</author>
    public class Vector3D
    {
        #region Constructors

        public Vector3D(Coordinate v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Creates a vector, that is the difference of <paramref name="to"/> and <paramref name="from"/>
        /// </summary>
        /// <param name="from">The origin coordinate</param>
        /// <param name="to">The destination coordinate</param>
        public Vector3D(Coordinate from, Coordinate to)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
            Z = to.Z - from.Z;
        }

        /// <summary>
        /// Creates a vector with the ordinates <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/>
        /// </summary>
        /// <param name="x">The x-ordinate</param>
        /// <param name="y">The y-ordinate</param>
        /// <param name="z">The z-ordinate</param>
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating the x-ordinate.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets a value indicating the y-ordinate.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets a value indicating the z-ordinate.
        /// </summary>
        public double Z { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new vector with given <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> components.
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        /// <returns>A new vector</returns>
        public static Vector3D Create(double x, double y, double z)
        {
            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// Creates a new vector from a <see cref="Coordinate"/>.
        /// </summary>
        /// <param name="coord">The coordinate to copy</param>
        /// <returns>A new vector</returns>
        public static Vector3D Create(Coordinate coord)
        {
            return new Vector3D(coord);
        }

        /// <summary>
        /// Function to devide all dimensions of this vector by <paramref name="d"/>.
        /// </summary>
        /// <param name="d">The divisor</param>
        /// <returns>A new (divided) vector</returns>
        private Vector3D Divide(double d)
        {
            return Create(X / d, Y / d, Z / d);
        }

        /// <summary>
        /// Computes the dot product of the 3D vectors AB and CD.
        /// </summary>
        /// <param name="a">A coordinate</param>
        /// <param name="b">A coordinate</param>
        /// <param name="c">A coordinate</param>
        /// <param name="d">A coordinate</param>
        /// <returns>The dot product</returns>
        public static double Dot(Coordinate a, Coordinate b, Coordinate c, Coordinate d)
        {
            double abX = b.X - a.X;
            double abY = b.Y - a.Y;
            double abZ = b.Z - a.Z;
            double cdX = d.X - c.X;
            double cdY = d.Y - c.Y;
            double cdZ = d.Z - c.Z;
            return abX * cdX + abY * cdY + abZ * cdZ;
        }

        /// <summary>
        /// Computes the 3D dot-product of two <see cref="Coordinate"/>s
        /// </summary>
        /// <param name="v1">The 1st vector</param>
        /// <param name="v2">The 2nd vector</param>
        /// <returns>The dot product of the (coordinate) vectors</returns>
        public static double Dot(Coordinate v1, Coordinate v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// Computes the dot-product of this <see cref="Vector3D"/> and <paramref name="v"/>
        /// </summary>
        /// <paramref name="v">The 2nd vector</paramref>
        /// <returns>The dot product of the vectors</returns>
        public double Dot(Vector3D v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// Function to compute the length of vector <paramref name="v"/>.
        /// </summary>
        /// <param name="v">A coordinate, treated as vector</param>
        /// <returns>The length of <paramref name="v"/></returns>
        public static double Length(Coordinate v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }

        /// <summary>
        /// Function to compute the length of this vector.
        /// </summary>
        /// <returns>The length of this vector</returns>
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// Function to compute a normalized form of vector <paramref name="v"/>.
        /// </summary>
        /// <param name="v">A coordinate vector</param>
        /// <returns>A normalized form of <paramref name="v"/></returns>
        public static Coordinate Normalize(Coordinate v)
        {
            double len = Length(v);
            return new Coordinate(v.X / len, v.Y / len, v.Z / len);
        }

        /// <summary>
        /// Function to compute a normalized form of this vector
        /// </summary>
        /// <returns>A normalized form of this vector</returns>
        public Vector3D Normalize()
        {
            double length = Length();
            return length > 0.0 ? Divide(Length()) : Create(0.0, 0.0, 0.0);
        }

        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "[{0}, {1}, {2}]", X, Y, Z);
        }

        #endregion
    }
}