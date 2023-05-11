using System;
using CustomMath;
using UnityEngine;

namespace Plane
{
    public class MyPlane : IEquatable<MyPlane>
    {
        private Vec3 mNormal;
        private float mDistance;

        public Vec3 Normal
        {
            get => mNormal;
            set => mNormal = value;
        }


        public float Distance
        {
            get => mDistance;
            set => mDistance = value;
        }
    
        public MyPlane(Vec3 inNormal, Vec3 inPoint)
        {
            mNormal = inNormal.Normalized;
            mDistance = -Vec3.Dot(mNormal, inPoint);
        }


        public MyPlane(Vec3 inNormal, float d)
        {
            mNormal = inNormal.Normalized;
            mDistance = d;
        }

        public MyPlane(Vec3 a, Vec3 b, Vec3 c)
        {
            mNormal = Vec3.Cross(b - a, c - a).Normalized;
            mDistance = -Vec3.Dot(mNormal, a);
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            mNormal = inNormal.Normalized;
            mDistance = -Vec3.Dot(inNormal, inPoint);
        }
    
        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            mNormal = Vec3.Cross(b - a, c - a);
            mNormal = mNormal.Normalized;
            mDistance = -Vec3.Dot(mNormal, a);
        }

        public void Flip()
        {
            mNormal = -mNormal;
            mDistance = -mDistance;
        }
        /*
        public MyPlane Flipped
        {
            get { return gameObject.AddComponent<MyPlane>(); }
        }
            */
        public void Translate(Vec3 translation)
        {
            mDistance += Vec3.Dot(mNormal, translation);
        }

        public static MyPlane Translate(MyPlane plane, Vec3 translation)
        {
            plane.Translate(translation);
            return plane;
        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float dist = Vec3.Dot(mNormal, point) + mDistance;
            return point - mNormal * dist;
        }

        public float GetDistanceToPoint(Vec3 point)
        {
            return Vec3.Dot(mNormal, point) + mDistance;
        }

        public bool GetSide(Vec3 point)
        {
            return Vec3.Dot(mNormal, point) + mDistance > 0;
        }

        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            float d0 = Vec3.Dot(mNormal, inPt0) + mDistance;
            float d1 = Vec3.Dot(mNormal, inPt1) + mDistance;
            return (d0 > 0 && d1 > 0) || (d0 <= 0 && d1 <= 0);
        }

        public bool Equals(MyPlane other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return mNormal.Equals(other.mNormal) && mDistance.Equals(other.mDistance);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyPlane)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(mNormal, mDistance);
        }
    }
}