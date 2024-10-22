using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//TODO: Clean up velocity code. Decide if we want to include it!
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


    
    
    //copy constructor
    public EyeData(Vector3 angularVelocity, Vector3 position, Vector3 velocity, Vector3 rightEyePosition, Vector3 leftEyePosition, Vector3 centerEyePosition, Quaternion centerEyeRotation, Quaternion leftEyeRotation, Quaternion rightEyeRotation, float time)
    {
        this.angularVelocity = angularVelocity;
        this.position = position;
        this.velocity = velocity;
        this.rightEyePosition = rightEyePosition;
        this.leftEyePosition = leftEyePosition;
        this.centerEyePosition = centerEyePosition;
        this.centerEyeRotation = centerEyeRotation;
        this.leftEyeRotation = leftEyeRotation;
        this.rightEyeRotation = rightEyeRotation;
        this.time = time;
    }

    
    //reading values from Input actions to retrieve data frame by frame. Previous frames data is passed to be used in angular velocty calculation, may have more uses down the line.
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
        this.angularVelocity = CalculateAngularVelocity(prevTime.centerEyeRotation, this.centerEyeRotation, time - prevTime.time);
    }
    
    //Quaternion math to calculate angular velocity as represented by a rotation scalar w denoted by angularVelocityMagnitude multiplied by the axis of rotation represented by 3d normalized vector stemming from the relative origin of the object itself.
    //Note that this angular velocity is NOT normalized because it is a normalized vector * a scalar of the rotation. 
    Vector3 CalculateAngularVelocity(Quaternion q1, Quaternion q2, float deltaTime)
    {
        // Step 1: Find the relative rotation
        Quaternion relativeRotation = q2 * Quaternion.Inverse(q1);
    
        // Step 2: Extract the angle (in radians)
        float angleInRadians;
        Vector3 axis;
        relativeRotation.ToAngleAxis(out angleInRadians, out axis);
    
        // Step 3: Compute angular velocity (rate of rotation)
        float angularVelocityMagnitude = angleInRadians / deltaTime;
    
        // Step 4: Return the angular velocity vector
        return axis * angularVelocityMagnitude;
    }
    
    public override string ToString()
    {
        return $"Time: {time}Angular Velocity: {angularVelocity}\nPosition: {position}\nRight Eye Position: {rightEyePosition}\nLeft Eye Position: {leftEyePosition}\nCenter Eye Position: {centerEyePosition}\nCenter Eye Rotation: {centerEyeRotation}\nLeft Eye Rotation: {leftEyeRotation}\nRight Eye Rotation: {rightEyeRotation}";
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
        return $"{{\"TimeStamp\": {time},\"Angular Velocity\": {angularVelocity},\"Position\": {position},\"Right Eye Position\": {rightEyePosition},\"Left Eye Position\": {leftEyePosition},\"Center Eye Position\": {centerEyePosition},\"Center Eye Rotation\": {centerEyeRotation},\"Left Eye Rotation\": {leftEyeRotation},\"Right Eye Rotation\": {rightEyeRotation}}}";
    }

    
}