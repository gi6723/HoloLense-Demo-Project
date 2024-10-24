using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

[System.Serializable]
public class SerializableEyeData
{
    public float TimeStamp;
    public float[] AngularVelocity;
    public float[] Position;
    public float[] RightEyePosition;
    public float[] LeftEyePosition;
    public float[] CenterEyePosition;
    public float[] CenterEyeRotation;
    public float[] LeftEyeRotation;
    public float[] RightEyeRotation;
}
// TODO: Clean up velocity code. Decide if we want to include it!
public class EyeData
{
    private Vector3 angularVelocity { get; set; }
    private Vector3 position { get; set; }
    private Vector3 velocity { get; set; }
    private Vector3 rightEyePosition { get; set; }
    private Vector3 leftEyePosition { get; set; }
    private Vector3 centerEyePosition { get; set; }
    private Quaternion centerEyeRotation { get; set; }
    private Quaternion leftEyeRotation { get; set; }
    private Quaternion rightEyeRotation { get; set; }
    private float time { get; set; }

    // Single constructor with null check
    public EyeData(float time, Dictionary<string, InputAction> vectorData, Dictionary<string, InputAction> quaternionData, EyeData prevTime)
    {
        this.time = time;
        this.position = vectorData["position"].ReadValue<Vector3>();
        this.velocity = vectorData["velocity"].ReadValue<Vector3>();
        this.rightEyePosition = vectorData["rightEyePosition"].ReadValue<Vector3>();
        this.leftEyePosition = vectorData["leftEyePosition"].ReadValue<Vector3>();
        this.centerEyePosition = vectorData["centerEyePosition"].ReadValue<Vector3>();
        this.centerEyeRotation = quaternionData["centerEyeRotation"].ReadValue<Quaternion>();
        this.leftEyeRotation = quaternionData["leftEyeRotation"].ReadValue<Quaternion>();
        this.rightEyeRotation = quaternionData["rightEyeRotation"].ReadValue<Quaternion>();

        // Check if prevTime is null before calculating angular velocity
        if (prevTime != null)
        {
            this.angularVelocity = CalculateAngularVelocity(prevTime.centerEyeRotation, this.centerEyeRotation, time - prevTime.time);
        }
        else
        {
            // Initialize angularVelocity to zero if there's no previous frame
            this.angularVelocity = Vector3.zero;
        }
    }

    // Quaternion math to calculate angular velocity
    Vector3 CalculateAngularVelocity(Quaternion q1, Quaternion q2, float deltaTime)
    {
        Quaternion relativeRotation = q2 * Quaternion.Inverse(q1);
        relativeRotation.ToAngleAxis(out float angleInRadians, out Vector3 axis);
        float angularVelocityMagnitude = angleInRadians / deltaTime;
        return axis * angularVelocityMagnitude;
    }

    public override string ToString()
    {
        return $"Time: {time} Angular Velocity: {angularVelocity}\nPosition: {position}\nRight Eye Position: {rightEyePosition}\nLeft Eye Position: {leftEyePosition}\nCenter Eye Position: {centerEyePosition}\nCenter Eye Rotation: {centerEyeRotation}\nLeft Eye Rotation: {leftEyeRotation}\nRight Eye Rotation: {rightEyeRotation}";
    }

    public string ToCsv()
    {
        return $"{time},{angularVelocity.x},{angularVelocity.y},{angularVelocity.z},{position.x},{position.y},{position.z},{rightEyePosition.x},{rightEyePosition.y},{rightEyePosition.z},{leftEyePosition.x},{leftEyePosition.y},{leftEyePosition.z},{centerEyePosition.x},{centerEyePosition.y},{centerEyePosition.z},{centerEyeRotation.x},{centerEyeRotation.y},{centerEyeRotation.z},{centerEyeRotation.w},{leftEyeRotation.x},{leftEyeRotation.y},{leftEyeRotation.z},{leftEyeRotation.w},{rightEyeRotation.x},{rightEyeRotation.y},{rightEyeRotation.z},{rightEyeRotation.w}";
    }

    public static string CsvHeader()
    {
        return "TimeStamp, Angular Velocity X,Angular Velocity Y,Angular Velocity Z,Angular Velocity, WPosition X,Position Y,Position Z,Right Eye Position X,Right Eye Position Y,Right Eye Position Z,Left Eye Position X,Left Eye Position Y,Left Eye Position Z,Center Eye Position X,Center Eye Position Y,Center Eye Position Z,Center Eye Rotation X,Center Eye Rotation Y,Center Eye Rotation Z,Center Eye Rotation W,Device Rotation X,Device Rotation Y,Device Rotation Z,Device Rotation W,Left Eye Rotation X,Left Eye Rotation Y,Left Eye Rotation Z,Left Eye Rotation W,Right Eye Rotation X,Right Eye Rotation Y,Right Eye Rotation Z,Right Eye Rotation W";
    }

    public string ToJson()
    {
        SerializableEyeData serializableData = new SerializableEyeData
        {
            TimeStamp = this.time,
            AngularVelocity = new float[] { this.angularVelocity.x, this.angularVelocity.y, this.angularVelocity.z },
            Position = new float[] { this.position.x, this.position.y, this.position.z },
            RightEyePosition = new float[] { this.rightEyePosition.x, this.rightEyePosition.y, this.rightEyePosition.z },
            LeftEyePosition = new float[] { this.leftEyePosition.x, this.leftEyePosition.y, this.leftEyePosition.z },
            CenterEyePosition = new float[] { this.centerEyePosition.x, this.centerEyePosition.y, this.centerEyePosition.z },
            CenterEyeRotation = new float[] { this.centerEyeRotation.x, this.centerEyeRotation.y, this.centerEyeRotation.z, this.centerEyeRotation.w },
            LeftEyeRotation = new float[] { this.leftEyeRotation.x, this.leftEyeRotation.y, this.leftEyeRotation.z, this.leftEyeRotation.w },
            RightEyeRotation = new float[] { this.rightEyeRotation.x, this.rightEyeRotation.y, this.rightEyeRotation.z, this.rightEyeRotation.w }
        };

        return JsonConvert.SerializeObject(serializableData);  // Serialize into JSON format
    }
}
