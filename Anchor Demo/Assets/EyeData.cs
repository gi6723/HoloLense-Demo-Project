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


    public EyeData(float time, Dictionary<string, InputAction> vectorData, Dictionary<string, InputAction> quaternionData)
    {
        this.time = time;
        this.angularVelocity = quaternionData["angularVelocity"].ReadValue<Vector3>();
        this.position = vectorData["position"].ReadValue<Vector3>();
        this.velocity = vectorData["velocity"].ReadValue<Vector3>();
        this.rightEyePosition = vectorData["rightEyePosition"].ReadValue<Vector3>();
        this.leftEyePosition = vectorData["leftEyePosition"].ReadValue<Vector3>();
        this.centerEyePosition = vectorData["centerEyePosition"].ReadValue<Vector3>();
        this.centerEyeRotation = quaternionData["centerEyeRotation"].ReadValue<Quaternion>();
        this.leftEyeRotation = quaternionData["leftEyeRotation"].ReadValue<Quaternion>();
        this.rightEyeRotation = quaternionData["rightEyeRotation"].ReadValue<Quaternion>();
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