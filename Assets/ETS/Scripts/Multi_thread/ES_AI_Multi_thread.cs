using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
//
public static class UCTMath
{
    //
    public static float CalculateVector3Distance(Vector3 currentposition, Vector3 targetposition)
    {
        Vector3 offset = targetposition - currentposition;
        float sqrlen = offset.sqrMagnitude;
        return sqrlen;
    }
    //
    public static float PI(float value)
    {
        float v = math.PI * value;
        return v;
    }
    //
    public static void DrawAWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Matrix4x4 CubeTranform = Matrix4x4.TRS(position, rotation, scale);

        Matrix4x4 OldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= CubeTranform;

        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = OldGizmosMatrix;
    }
}
//
public class ES_AI_Multi_thread : MonoBehaviour
{
    [Tooltip("Will not calculate humans movement")]
    public bool DoNotBurstHumans = false;
    public enum AITrackingSystem
    {
        UseBurst,
        ByIndividualAI
    }

    [Header("DEBUG")]
    [Header("Version 2.0.0F")]
    [Tooltip("On slower device pls use Individual ")]
    public AITrackingSystem trackingSystem = AITrackingSystem.ByIndividualAI;
    public List<Transform> vehicleslist = new List<Transform>(1);
    public List<Transform> pedestrainCtrls = new List<Transform>(1);
    public ESSpawnManager spawnmanager;
    public ESSpawnHuman spawnHuman;
    //private;

    // Start is called before the first frame update
    void Start()
    {
        spawnmanager = spawnmanager == null ? spawnmanager = GameObject.FindObjectOfType<ESSpawnManager>() : spawnmanager;
        vehicleslist = new List<Transform>();
        if (spawnmanager != null)
        {
            for (int i = 0; i < spawnmanager.SpawnedVeh.Count; i++)
            {
                if (trackingSystem == AITrackingSystem.ByIndividualAI)
                {
                    spawnmanager.SpawnedVeh[i].GetComponent<ESVehicleAI>().IndividualTracker = true;
                }
                else
                {
                    spawnmanager.SpawnedVeh[i].GetComponent<ESVehicleAI>().IndividualTracker = false;
                }
                vehicleslist.Add(spawnmanager.SpawnedVeh[i].transform);
            }
            //
        }
        pedestrainCtrls = new List<Transform>();
        spawnHuman = spawnHuman == null ? spawnHuman = GameObject.FindObjectOfType<ESSpawnHuman>() : spawnHuman;
        //ESPedestrainCtrl[] _pedesctrl = GameObject.FindObjectsOfType<ESPedestrainCtrl>();
        if (spawnHuman != null)
        {
            for (int i = 0; i < spawnHuman.SpawnedHumans.Count; ++i)
            {
                pedestrainCtrls.Add(spawnHuman.SpawnedHumans[i].transform);
            }
        }
        //add spawned vehicles an accessable list 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region threading
        float val = new float();
        if (trackingSystem == AITrackingSystem.ByIndividualAI)
        {
            val = 1;
        }
        else
        {
            val = 0;
        }

        NativeArray<float3> tarpos = new NativeArray<float3>(vehicleslist.Count, Allocator.TempJob);
        TransformAccessArray behavorarray = new TransformAccessArray(vehicleslist.Count);


        NativeArray<float> _checkfisrtdist = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float3> _velocity = new NativeArray<float3>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _magnitude = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _topspeed = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _currentspeed = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<Vector3> _relativevec = new NativeArray<Vector3>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _newsteer = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _playerdist = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _turnspeed = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<Vector3> _playerpos = new NativeArray<Vector3>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<Vector3> _Triggerpos = new NativeArray<Vector3>(vehicleslist.Count, Allocator.TempJob);
        NativeArray<float> _Triggerdist = new NativeArray<float>(vehicleslist.Count, Allocator.TempJob);
        //Humans
        if (!DoNotBurstHumans)
        {
            TransformAccessArray spawnarray = new TransformAccessArray(pedestrainCtrls.Count);
            NativeArray<Vector3> _targetobjpos = new NativeArray<Vector3>(pedestrainCtrls.Count, Allocator.TempJob);
            NativeArray<Vector3> _playerposdist = new NativeArray<Vector3>(pedestrainCtrls.Count, Allocator.TempJob);
            NativeArray<float> _targetobjdistapt = new NativeArray<float>(pedestrainCtrls.Count, Allocator.TempJob);
            NativeArray<float> _playerdistapart = new NativeArray<float>(pedestrainCtrls.Count, Allocator.TempJob);
            //NativeArray<Vector3> _foward = new NativeArray<Vector3>(pedestrainCtrls.Count, Allocator.TempJob);
            NativeArray<float> _walkspeed = new NativeArray<float>(pedestrainCtrls.Count, Allocator.TempJob);


            if (pedestrainCtrls.Count > 0)
            {
                for (int i = 0; i < pedestrainCtrls.Count; i++)
                {
                    _targetobjpos[i] = pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().target != null ? pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().target.position : Vector3.zero;
                    _targetobjdistapt[i] = pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().UpdateDistance;
                    //_foward[i] = pedestrainCtrls[i].transform.forward;
                    _walkspeed[i] = pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().WalkSpeed;
                    _playerposdist[i] = pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().Player != null ?
                      pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().Player.position : new Vector3(1, 1, 1);
                    spawnarray.Add(pedestrainCtrls[i].transform);
                }
            }
            HumanBehavoiur humanBehavoiur = new HumanBehavoiur
            {
                targetdist = _targetobjdistapt,
                targetobjpos = _targetobjpos,
                playerdist = _playerdistapart,
                playerpos = _playerposdist,
                deltaTime = Time.deltaTime,
                WalkSpeed = _walkspeed,
                //  foward = _foward
            };
            //
            JobHandle Hjobhandle = humanBehavoiur.Schedule(spawnarray);
            Hjobhandle.Complete();
            //
            if (pedestrainCtrls.Count > 0)
            {
                for (int i = 0; i < pedestrainCtrls.Count; i++)
                {
                    pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().UpdateDistance = _targetobjdistapt[i];
                    pedestrainCtrls[i].GetComponent<ESPedestrainCtrl>().PlayerDist = _playerdistapart[i];
                    //pedestrainCtrls[i].transform.position = _position[i];
                }
            }
            //
            _targetobjpos.Dispose();
            _targetobjdistapt.Dispose();
            _playerdistapart.Dispose();
            _playerposdist.Dispose();
            _walkspeed.Dispose();
            spawnarray.Dispose();
        }
        //
        if (vehicleslist.Count > 0)
        {
            for (int i = 0; i < vehicleslist.Count; i++)
            {
                tarpos[i] = vehicleslist[i].GetComponent<ESVehicleAI>().TargetNode == null ? Vector3.zero : vehicleslist[i].GetComponent<ESVehicleAI>().Tar;
                _checkfisrtdist[i] = vehicleslist[i].GetComponent<ESVehicleAI>().checkfirstdist;
                _currentspeed[i] = vehicleslist[i].GetComponent<ESVehicleAI>().currentspeed;
                _topspeed[i] = vehicleslist[i].GetComponent<ESVehicleAI>().topspeed;
                //
                _magnitude[i] = vehicleslist[i].GetComponent<ESVehicleAI>().CarRb == null ? 0.00f
                 : vehicleslist[i].GetComponent<ESVehicleAI>().CarRb.velocity.magnitude;
                //
                _velocity[i] = vehicleslist[i].GetComponent<ESVehicleAI>().CarRb == null ?
                    Vector3.zero : vehicleslist[i].GetComponent<ESVehicleAI>().CarRb.velocity;
                _relativevec[i] = vehicleslist[i].GetComponent<ESVehicleAI>().relativevec;
                _newsteer[i] = vehicleslist[i].GetComponent<ESVehicleAI>().newsteer;
                //_lerpang[i] = vehicleslist[i].GetComponent<ESVehicleAI>().lerpsteerang;
                _turnspeed[i] = vehicleslist[i].GetComponent<ESVehicleAI>().turnspeed;
                _playerdist[i] = vehicleslist[i].GetComponent<ESVehicleAI>().playerdist;
                _playerpos[i] = vehicleslist[i].GetComponent<ESVehicleAI>().player != null ?
                 vehicleslist[i].GetComponent<ESVehicleAI>().player.position : Vector3.zero;
                //
                _Triggerpos[i] = vehicleslist[i].GetComponent<ESVehicleAI>().TriggerObject != null ?
                vehicleslist[i].GetComponent<ESVehicleAI>().TriggerObject.position
                 : new Vector3(1, 1, 1);

                behavorarray.Add(vehicleslist[i].transform);
            }
        }
        AIBehaviour behave = new AIBehaviour
        {
            deltatime = Time.deltaTime,
            Ignoredistancecal = val,
            targetnodepos = tarpos,
            //direction = dir,
            //tempangle = tempang,
            checkfirstdist = _checkfisrtdist,
            velocity = _velocity,
            magnitude = _magnitude,
            topspeed = _topspeed,
            currentspeed = _currentspeed,
            relativevec = _relativevec,
            newsteer = _newsteer,
            playerdist = _playerdist,
            playerpos = _playerpos,
            //lerpsteer = _lerpang,
            turnspeed = _turnspeed,
            triggerdist = _Triggerdist,
            triggerpos = _Triggerpos,
        };
        //
        JobHandle jobhandle = behave.Schedule(behavorarray);
        jobhandle.Complete();

        //
        //
        for (int i = 0; i < vehicleslist.Count; i++)
        {
            //vehicleslist[i].GetComponent<ESVehicleAI>().direction = dir[i];
            //vehicleslist[i].GetComponent<ESVehicleAI>().tempangle = tempang[i];
            if (trackingSystem == AITrackingSystem.UseBurst)
            {
                vehicleslist[i].GetComponent<ESVehicleAI>().checkfirstdist = _checkfisrtdist[i];
            }
            vehicleslist[i].GetComponent<ESVehicleAI>().currentspeed = _currentspeed[i];
            vehicleslist[i].GetComponent<ESVehicleAI>().relativevec = _relativevec[i];
            vehicleslist[i].GetComponent<ESVehicleAI>().newsteer = _newsteer[i];
            vehicleslist[i].GetComponent<ESVehicleAI>().playerdist = _playerdist[i];
            vehicleslist[i].GetComponent<ESVehicleAI>().TriggerDistance = _Triggerdist[i];
            //vehicleslist[i].GetComponent<ESVehicleAI>().lerpsteerang = _lerpang[i];
            if (vehicleslist[i].GetComponent<ESVehicleAI>().CarRb != null)
            {
                if (vehicleslist[i].GetComponent<ESVehicleAI>().returntriggerstay == false)
                    vehicleslist[i].GetComponent<ESVehicleAI>().CarRb.velocity = _velocity[i];
            }
            //vehicleslist[i].GetComponent<ESVehicleAI>().checkdistforsmartdelete = checkdistdelete[i];
        }
        //end
        //dir.Dispose();

        //
        tarpos.Dispose();
        _currentspeed.Dispose();
        _magnitude.Dispose();
        _velocity.Dispose();
        _topspeed.Dispose();
        //tempang.Dispose();
        _checkfisrtdist.Dispose();
        _relativevec.Dispose();
        _newsteer.Dispose();
        _playerdist.Dispose();
        _playerpos.Dispose();
        //_lerpang.Dispose();
        _turnspeed.Dispose();
        _Triggerdist.Dispose();
        _Triggerpos.Dispose();
        //_foward.Dispose();

        //checkdistdelete.Dispose();
        behavorarray.Dispose();

        #endregion

    }
}
//
[BurstCompile]
public struct HumanBehavoiur : IJobParallelForTransform
{
    public NativeArray<Vector3> targetobjpos, playerpos;
    public NativeArray<float> targetdist, WalkSpeed, playerdist;

    public float deltaTime;
    public void Execute(int index, TransformAccess Vehicletransform)
    {
        float3 Hpos = Vehicletransform.position;
        float3 tar = targetobjpos[index];
        float3 playerPos = playerpos[index];
        targetdist[index] = targetobjpos[index] != Vector3.zero ? Vector3.Distance(Hpos, tar) : 0.0f;
        playerdist[index] = Vector3.Distance(playerPos, Hpos);
        ///position[index] = Vector3.Slerp(Hpos, tar, WalkSpeed[index] * deltaTime);
        float3 Ftar = targetobjpos[index];
        float3 relativepos = Ftar - Hpos;

        Vehicletransform.rotation = quaternion.LookRotation(relativepos, Vector3.up);
    }
}
//
[BurstCompile]
public struct AIBehaviour : IJobParallelForTransform
{
    public NativeArray<float3> targetnodepos, velocity;
    public NativeArray<Vector3> relativevec, playerpos, triggerpos;
    public NativeArray<float> checkfirstdist, magnitude, currentspeed, topspeed,
     newsteer, turnspeed, playerdist, triggerdist;

    [ReadOnly]
    public float deltatime;
    public float Ignoredistancecal;
    //public Vector3 targetnodepos;
    //public Vector3 direction;
    //
    public void Execute(int index, TransformAccess Vehicletransform)
    {
        //get for target node and current ai vehicle;
        float3 vehpos = Vehicletransform.position;
        //
        triggerdist[index] = triggerpos[index] != new Vector3(1, 1, 1) ? Vector3.Distance(triggerpos[index], vehpos) : 0.0f;
        if (Ignoredistancecal == 0)
        {
            checkfirstdist[index] = Vector3.Distance(vehpos, targetnodepos[index]);
        }
        playerdist[index] = Vector3.Distance(Vehicletransform.position, playerpos[index]);
        relativevec[index] = (relativevec[index] / relativevec[index].magnitude);
        newsteer[index] = (relativevec[index].x / relativevec[index].magnitude) * 50;
        //
        //
        float Pi = UCTMath.PI(1.15f);
        Vector3 vec = velocity[index];
        currentspeed[index] = magnitude[index] * Pi;
        if (currentspeed[index] > topspeed[index])
        {
            vec = (topspeed[index] / Pi) * vec.normalized;
            velocity[index] = vec;
        }
    }

}

