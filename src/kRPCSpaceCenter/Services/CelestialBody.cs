using System;
using System.Collections.Generic;
using KRPC.Service.Attributes;
using KRPC.Utils;
using KRPCSpaceCenter.ExtensionMethods;
using UnityEngine;
using Tuple3 = KRPC.Utils.Tuple<double, double, double>;
using Tuple4 = KRPC.Utils.Tuple<double, double, double, double>;

namespace KRPCSpaceCenter.Services
{
    /// <summary>
    /// Represents a celestial body (such as a planet or moon).
    /// </summary>
    [KRPCClass (Service = "SpaceCenter")]
    public sealed class CelestialBody : Equatable<CelestialBody>
    {
        Orbit orbit;

        public CelestialBody (global::CelestialBody body)
        {
            InternalBody = body;
            // TODO: better way to check for orbits?
            if (body.name != "Sun")
                orbit = new Orbit (body);
        }

        public global::CelestialBody InternalBody { get; private set; }

        public override bool Equals (CelestialBody obj)
        {
            return InternalBody == obj.InternalBody;
        }

        public override int GetHashCode ()
        {
            return InternalBody.GetHashCode ();
        }

        /// <summary>
        /// The name of the body.
        /// </summary>
        [KRPCProperty]
        public string Name {
            get { return InternalBody.name; }
        }

        /// <summary>
        /// A list of celestial bodies that are in orbit around this celestial body.
        /// </summary>
        [KRPCProperty]
        public IList<CelestialBody> Satellites {
            get {
                var allBodies = SpaceCenter.Bodies;
                var bodies = new List<CelestialBody> ();
                foreach (var body in InternalBody.orbitingBodies) {
                    bodies.Add (allBodies [body.name]);
                }
                return bodies;
            }
        }

        /// <summary>
        /// The mass of the body, in kilograms.
        /// </summary>
        [KRPCProperty]
        public float Mass {
            get { return (float)InternalBody.Mass; }
        }

        /// <summary>
        /// The <a href="http://en.wikipedia.org/wiki/Standard_gravitational_parameter">standard
        /// gravitational parameter</a> of the body in <math>m^3s^{-2}</math>.
        /// </summary>
        [KRPCProperty]
        public float GravitationalParameter {
            get { return (float)InternalBody.gravParameter; }
        }

        /// <summary>
        /// The acceleration due to gravity at sea level (mean altitude) on the body, in <math>m/s^2</math>.
        /// </summary>
        [KRPCProperty]
        public float SurfaceGravity {
            get { return (float)InternalBody.GeeASL * 9.81f; }
        }

        /// <summary>
        /// The rotational period of the body, in seconds.
        /// </summary>
        [KRPCProperty]
        public float RotationalPeriod {
            get { return (float)InternalBody.rotationPeriod; }
        }

        /// <summary>
        /// The rotational speed of the body, in radians per second.
        /// </summary>
        [KRPCProperty]
        public float RotationalSpeed {
            get { return (float)(2f * Math.PI) / RotationalPeriod; }
        }

        /// <summary>
        /// The equatorial radius of the body, in meters.
        /// </summary>
        [KRPCProperty]
        public float EquatorialRadius {
            get { return (float)InternalBody.Radius; }
        }

        /// <summary>
        /// The radius of the sphere of influence of the body, in meters.
        /// </summary>
        [KRPCProperty]
        public float SphereOfInfluence {
            get { return (float)InternalBody.sphereOfInfluence; }
        }

        /// <summary>
        /// The orbit of the body.
        /// </summary>
        [KRPCProperty]
        public Orbit Orbit {
            get { return orbit; }
        }

        /// <summary>
        /// <c>true</c> if the body has an atmosphere.
        /// </summary>
        [KRPCProperty]
        public bool HasAtmosphere {
            get { return InternalBody.atmosphere; }
        }

        /// <summary>
        /// The depth of the atmosphere, in meters.
        /// </summary>
        [KRPCProperty]
        public float AtmosphereDepth {
            get { return (float)InternalBody.atmosphereDepth; }
        }

        /// <summary>
        /// <c>true</c> if there is oxygen in the atmosphere, required for air-breathing engines.
        /// </summary>
        [KRPCProperty]
        public bool HasAtmosphericOxygen {
            get { return InternalBody.atmosphereContainsOxygen; }
        }

        /// <summary>
        /// The reference frame that is fixed relative to the celestial body.
        /// <list type="bullet">
        /// <item><description>The origin is at the center of the body.
        /// </description></item>
        /// <item><description>The axes rotate with the body.</description></item>
        /// <item><description>The x-axis points from the center of the body
        /// towards the intersection of the prime meridian and equator (the
        /// position at 0° longitude, 0° latitude).</description></item>
        /// <item><description>The y-axis points from the center of the body
        /// towards the north pole.</description></item>
        /// <item><description>The z-axis points from the center of the body
        /// towards the equator at 90°E longitude.</description></item>
        /// </list>
        /// </summary>
        [KRPCProperty]
        public ReferenceFrame ReferenceFrame {
            get { return ReferenceFrame.Object (InternalBody); }
        }

        /// <summary>
        /// The reference frame that is fixed relative to this celestial body, and
        /// orientated in a fixed direction (it does not rotate with the body).
        /// <list type="bullet">
        /// <item><description>The origin is at the center of the body.</description></item>
        /// <item><description>The axes do not rotate.</description></item>
        /// <item><description>The x-axis points in an arbitrary direction through the
        /// equator.</description></item>
        /// <item><description>The y-axis points from the center of the body towards
        /// the north pole.</description></item>
        /// <item><description>The z-axis points in an arbitrary direction through the
        /// equator.</description></item>
        /// </list>
        /// </summary>
        [KRPCProperty]
        public ReferenceFrame NonRotatingReferenceFrame {
            get { return ReferenceFrame.NonRotating (InternalBody); }
        }

        /// <summary>
        /// Gets the reference frame that is fixed relative to this celestial body, but
        /// orientated with the body's orbital prograde/normal/radial directions.
        /// <list type="bullet">
        /// <item><description>The origin is at the center of the body.
        /// </description></item>
        /// <item><description>The axes rotate with the orbital prograde/normal/radial
        /// directions.</description></item>
        /// <item><description>The x-axis points in the orbital anti-radial direction.
        /// </description></item>
        /// <item><description>The y-axis points in the orbital prograde direction.
        /// </description></item>
        /// <item><description>The z-axis points in the orbital normal direction.
        /// </description></item>
        /// </list>
        /// </summary>
        [KRPCProperty]
        public ReferenceFrame OrbitalReferenceFrame {
            get { return ReferenceFrame.Orbital (InternalBody); }
        }

        /// <summary>
        /// Returns the position vector of the center of the body in the specified reference frame.
        /// </summary>
        /// <param name="referenceFrame"></param>
        [KRPCMethod]
        public Tuple3 Position (ReferenceFrame referenceFrame)
        {
            return referenceFrame.PositionFromWorldSpace (InternalBody.position).ToTuple ();
        }

        /// <summary>
        /// Returns the velocity vector of the body in the specified reference frame.
        /// </summary>
        /// <param name="referenceFrame"></param>
        [KRPCMethod]
        public Tuple3 Velocity (ReferenceFrame referenceFrame)
        {
            return referenceFrame.VelocityFromWorldSpace (InternalBody.position, InternalBody.GetWorldVelocity ()).ToTuple ();
        }

        /// <summary>
        /// Returns the rotation of the body in the specified reference frame.
        /// </summary>
        /// <param name="referenceFrame"></param>
        [KRPCMethod]
        public Tuple4 Rotation (ReferenceFrame referenceFrame)
        {
            var up = Vector3.up;
            var right = InternalBody.GetRelSurfacePosition (0, 0, 1).normalized;
            var forward = Vector3.Cross (right, up);
            Vector3.OrthoNormalize (ref forward, ref up);
            var rotation = Quaternion.LookRotation (forward, up);
            return referenceFrame.RotationFromWorldSpace (rotation).ToTuple ();
        }

        /// <summary>
        /// Returns the direction in which the north pole of the celestial body is
        /// pointing, as a unit vector, in the specified reference frame.
        /// </summary>
        /// <param name="referenceFrame"></param>
        [KRPCMethod]
        public Tuple3 Direction (ReferenceFrame referenceFrame)
        {
            return referenceFrame.DirectionFromWorldSpace (InternalBody.transform.up).ToTuple ();
        }

        //TODO: default argument value?
        /// <summary>
        /// Returns the angular velocity of the body in the specified reference
        /// frame. The magnitude of the vector is the rotational speed of the body, in
        /// radians per second, and the direction of the vector indicates the axis of
        /// rotation, using the right-hand rule.
        /// </summary>
        /// <param name="referenceFrame"></param>
        [KRPCMethod]
        public Tuple3 AngularVelocity (ReferenceFrame referenceFrame)
        {
            return referenceFrame.AngularVelocityFromWorldSpace (InternalBody.angularVelocity).ToTuple ();
        }
    }
}
