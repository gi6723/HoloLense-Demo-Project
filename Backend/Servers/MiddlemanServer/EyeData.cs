﻿using System;

namespace DefaultNamespace;
public class EyeData
{
    public float TimeStamp { get; set; }
    public float[] AngularVelocity { get; set; }  // Replacing Vector3 with array of floats
    public float[] Position { get; set; }         // Replacing Vector3 with array of floats
    public float[] RightEyePosition { get; set; }
    public float[] LeftEyePosition { get; set; }
    public float[] CenterEyePosition { get; set; }
    public float[] CenterEyeRotation { get; set; }  // Replacing Quaternion with array of floats
    public float[] LeftEyeRotation { get; set; }
    public float[] RightEyeRotation { get; set; }

    // Optional: CSV representation method if needed for direct conversion
    public string ToCsv()
    {
        return $"{TimeStamp},{AngularVelocity[0]},{AngularVelocity[1]},{AngularVelocity[2]},{Position[0]},{Position[1]},{Position[2]},{RightEyePosition[0]},{RightEyePosition[1]},{RightEyePosition[2]},{LeftEyePosition[0]},{LeftEyePosition[1]},{LeftEyePosition[2]},{CenterEyePosition[0]},{CenterEyePosition[1]},{CenterEyePosition[2]},{CenterEyeRotation[0]},{CenterEyeRotation[1]},{CenterEyeRotation[2]},{CenterEyeRotation[3]},{LeftEyeRotation[0]},{LeftEyeRotation[1]},{LeftEyeRotation[2]},{LeftEyeRotation[3]},{RightEyeRotation[0]},{RightEyeRotation[1]},{RightEyeRotation[2]},{RightEyeRotation[3]}";
    }

    public static string CsvHeader()
    {
        return "TimeStamp,Angular Velocity X,Angular Velocity Y,Angular Velocity Z,Position X,Position Y,Position Z,Right Eye Position X,Right Eye Position Y,Right Eye Position Z,Left Eye Position X,Left Eye Position Y,Left Eye Position Z,Center Eye Position X,Center Eye Position Y,Center Eye Position Z,Center Eye Rotation X,Center Eye Rotation Y,Center Eye Rotation Z,Center Eye Rotation W,Left Eye Rotation X,Left Eye Rotation Y,Left Eye Rotation Z,Left Eye Rotation W,Right Eye Rotation X,Right Eye Rotation Y,Right Eye Rotation Z,Right Eye Rotation W";
    }
}