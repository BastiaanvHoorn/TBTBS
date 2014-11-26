using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    static class Util
    {

        /// <summary>
        /// Returns the index of wich the given vector3 in the given array.
        /// Returns -1 if Vector3 is not found
        /// </summary>
        /// <param name="arr">The array wich has to be search</param>
        /// <param name="vector">The vector3 wich index must be returned</param>
        /// <returns></returns>
        public static int index_of(Vector3[] arr, Vector3 vector)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == vector)
                {
                    return i; 
                }
            }
            return -1;
        }

        public static int index_of(Vector2[] arr, Vector2 vector)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == vector)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Converts a vector2 to vector3 where the given axis will be zero
        /// </summary>
        /// <param name="v2"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Vector3 v2_to_v3(Vector2 v2, string axis = "z")
        {
            switch (axis)
            {
                case "x":
                    return new Vector3(0, v2.y, v2.x);
                case "y":
                    return new Vector3(v2.x, 0, v2.y);
                case "z":
                    return new Vector3(v2.x, v2.y, 0);
            }
            return new Vector3();
        }

        /// <summary>
        /// Convert vector3 to vector2
        /// </summary>
        /// <param name="axis">the axis wich must be ignored during conversion</param>
        /// <returns>Returns the Vector3 stripped of the given axis</returns>
        public static Vector2 v3_to_v2(Vector3 v3, string axis)
        {
            switch (axis)
            {
                case"x":
                    return new Vector2(v3.z, v3.y);
                case"y":
                    return new Vector2(v3.x, v3.z);
                case"z":
                    return new Vector2(v3.x, v3.y);
            }
            return new Vector2();
        }

        /// <summary>
        /// Convert vector3 to vector2
        /// <para>Returns the Vector3 stripped of the axis wich was 0.
        /// If no axis is zero an empty vector2 will be returned</para>
        /// </summary>
        public static Vector2 v3_to_v2(Vector3 v3)
        {
            if (v3.x == 0)
            {
                return v3_to_v2(v3, "x");
            }
            else if (v3.y == 0)
            {
                return v3_to_v2(v3, "y");
            }
            else if(v3.z == 0)
            {
                return v3_to_v2(v3, "z");
            }
            return new Vector2();
        }

        public static Vector2[] v3_to_v2(Vector3[] v3arr, string axis)
        {
            Vector2[] v2arr = new Vector2[v3arr.Length];
            for(int i = 0; i<v3arr.Length; i++)
            {
                v2arr[i] = Util.v3_to_v2(v3arr[i], axis);
            }
            return v2arr;
        }
        
        /// <summary>
        /// Returns an array containing the slope and the y-offset of linear function in the form of (y = ax + b),
        /// where a the slope is and b the y-offset.
        /// The first entry in the array is the slope
        /// The second entry is the y-offset
        /// </summary>
        /// <param name="first">The first point the line goes through</param>
        /// <param name="second">The second point the line goes through</param>
        /// <returns></returns>
        public static float[] get_line(Vector2 a, Vector2 b)
        {
            
            float slope = (b.y - a.y) / (b.x - a.x);
            float offset = a.y - slope * a.x;
            return new float[2]{slope, offset};
        }
    }
}
