using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using SimpleGame;
using SimpleGame.Math;

namespace SimpleGame.GameFramework {


public class Actor{

   public bool Enabled {get; set;}

    public string StaticMeshId {get; set;}

    public AffineMat4  Transform= new AffineMat4();

    public AffineMat4 ModelView=new AffineMat4();

    public AffineMat4 NormalTransform = new AffineMat4();

    public Vector3 Scale;

    public Actor(){
        this.Enabled=false;
        this.StaticMeshId="";
    }

    public void SetTransform(Vector3 positionVector, Vector3 axisVector, double angle, Vector3 scale){
        this.Scale = scale;
        Transform.Rotation((float)angle,axisVector);
        Transform.Scale(scale);
        Transform.Translation(positionVector);
    }
 
}
public class Level{

public Dictionary<string,Actor> ActorCollection {get; set;}

public Vector3 PlayerStartPosition {get; set;}

public double PlayerStartRotationAngle {get;set;}

public Vector3 PlayerStartRotationAxis {get;set;}
public Level(){

ActorCollection=new Dictionary<string,Actor>();
PlayerStartPosition=new Vector3(0.0f,0.0f,0.0f);
PlayerStartRotationAngle=0;
PlayerStartRotationAxis=new Vector3(0.0f,1.0f,0.0f);
}

public Level(HttpClient httpClient, string path){
_httpClient = httpClient;
_levelPath = path;
ActorCollection=new Dictionary<string,Actor>();
PlayerStartPosition=new Vector3(0.0f,0.0f,0.0f);
PlayerStartRotationAngle=0;
PlayerStartRotationAxis=new Vector3(0.0f,1.0f,0.0f);

}

public List<string> GetActiveMeshes(){
    List<string> activeIds=new List<string>();
    foreach (var keyval in ActorCollection){
        if(keyval.Value.Enabled){
            if(!activeIds.Contains(keyval.Value.StaticMeshId))
                activeIds.Add(keyval.Value.StaticMeshId);
        }
    }
    return activeIds;
}
public  async Task RetrieveLevel(Dictionary<string,RetrievedMesh> AssetCollection){

_retrievedLevel = await _httpClient.GetFromJsonAsync<RetrievedLevel>(_levelPath);

for(int i=0;i<_retrievedLevel.mesh_list.Length;i++){

    RetrievedMeshMeta meshMeta=_retrievedLevel.mesh_list[i];
        
    RetrievedMesh retMesh=await _httpClient.GetFromJsonAsync<RetrievedMesh>(meshMeta.file);
    retMesh.usindices = new ushort[retMesh.indices.Length];
    retMesh.usindices = Array.ConvertAll<int,ushort>(retMesh.indices,delegate(int val){return (ushort)val;});

    AssetCollection.Add(meshMeta.id,retMesh);

}

for(int i=0;i<_retrievedLevel.actor_list.Length;i++){
    RetrievedActor retActor=_retrievedLevel.actor_list[i];
    Actor actor = new Actor();
    actor.Enabled=retActor.enabled;
    actor.StaticMeshId=retActor.sm;
    actor.SetTransform(new Vector3(retActor.position),new Vector3(retActor.orientation.axis),retActor.orientation.angle, new Vector3(retActor.scale));
    ActorCollection.Add(retActor.id,actor);
}

PlayerStartPosition = new Vector3(_retrievedLevel.playerstartposition);
PlayerStartRotationAngle = _retrievedLevel.playerstartrotationangle;
PlayerStartRotationAxis= new Vector3(_retrievedLevel.playerstartrotationaxis); 

}


private string _levelPath;
private HttpClient _httpClient;
private RetrievedLevel _retrievedLevel;


}
}